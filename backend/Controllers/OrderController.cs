using backend.DAOs;
using backend.Models;
using backend.Repositories;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace backend.Controllers
{
    [Route("order")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly OrderRepository _orderRepository;
        private readonly AddressRepository _addressRepository;
        private readonly OrderItemRepository _orderItemRepository;
        private readonly ProductRepository _productRepository;
        private readonly EmailService _emailService;

        public OrderController(
            OrderRepository orderRepository,
            AddressRepository addressRepository,
            OrderItemRepository orderItemRepository,
            EmailService emailService,
            ProductRepository productRepository
            )
        {
            _orderRepository = orderRepository;
            _addressRepository = addressRepository;
            _orderItemRepository = orderItemRepository;
            _productRepository = productRepository;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> NewOrder([FromBody] OrderRequest request)
        {
            // Add shipping address to db
            var newAddress = new Address
            {
                FirstName = request.Address.FirstName,
                LastName = request.Address.LastName,
                City = request.Address.City,
                Country = request.Address.Country,
                State = request.Address.State,
                Street = request.Address.Street,
                Zip = request.Address.Zip
            };

            int newAddressId = await _addressRepository.CreateNewAddress(newAddress);

            // Add new Order to db
            var newOrder = new Order
            {
                TotalPrice = request.PaymentAmount,
                Email = request.Email,
                AddressId = newAddressId
            };

            int newOrderId = await _orderRepository.CreateOrder(newOrder);

            List<OrderItem> OrderList = new List<OrderItem>();
            var orderListEmail = new StringBuilder();

            var orderItems = request.Order.Select(item => new OrderItem
            {
                ProductId = item.ProductId,
                OrderId = newOrderId,
                Quantity = item.Quantity,
                Size = item.Size,
                UnitPrice = item.UnitPrice
            }).ToList();

            var productTasks = request.Order.Select(item => _productRepository.GetProductById(item.ProductId)).ToList();
            var products = await Task.WhenAll(productTasks);

            for (int i = 0; i < orderItems.Count; i++)
            {
                var productInfo = products[i];
                var item = orderItems[i];

                orderListEmail.AppendLine($"{productInfo.ProductName} | Size: {item.Size} | Amount: {item.Quantity} | Price: ${item.UnitPrice}");
            }

            orderListEmail.AppendLine($"Total Price: ${newOrder.TotalPrice}");

            var emailBody = $"Hello {newAddress.FirstName} {newAddress.LastName},\n\nYour order: \n{orderListEmail} has been received!";

            // Replace temp mail with NewOrder.Email
            var emailTask = _emailService.SendEmailAsync("theicekiller16@gmail.com", "Order Confirmation", emailBody);
            var saveItemsTask = _orderItemRepository.NewOrderItems(orderItems);

            await Task.WhenAll(emailTask, saveItemsTask);

            return Ok();
        }
    }
}
