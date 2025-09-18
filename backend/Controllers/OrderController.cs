using backend.DAOs;
using backend.Models;
using backend.Repositories;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            string OrderListEmail = string.Empty; // For the email order confirmation

            foreach (var item in request.Order)
            {
                OrderList.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    OrderId = newOrderId,
                    Quantity = item.Quantity,
                    Size = item.Size,
                    UnitPrice = item.UnitPrice
                });

                var productInfo = await _productRepository.GetProductById(item.ProductId);
                OrderListEmail += productInfo.ProductName +
                " | Size: " + item.Size
                + " | Amount: " + item.Quantity
                 + " | Price: $" + item.UnitPrice
                 + "\n";
            }

            OrderListEmail += "Total Price: $" + newOrder.TotalPrice + "\n";

            var emailBody = $"Hello {newAddress.FirstName} {newAddress.LastName},\n\nYour order: \n{OrderListEmail} has been received!";

            // Replace temp mail with NewOrder.Email
            await _emailService.SendEmailAsync("dikofi5216@dawhe.com", "Order Confirmation", emailBody);

            await _orderItemRepository.NewOrderItems(OrderList);

            return Ok();
        }
    }
}
