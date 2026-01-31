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

            var newOrder = new Order
            {
                TotalPrice = request.PaymentAmount,
                Email = request.Email,
                AddressId = newAddressId
            };

            int newOrderId = await _orderRepository.CreateOrder(newOrder);

            var orderListEmail = new StringBuilder();

            foreach (var itemRequest in request.Order)
            {
                var product = await _productRepository.GetProductById(itemRequest.ProductId);

                var orderItem = new OrderItem
                {
                    ProductId = itemRequest.ProductId,
                    OrderId = newOrderId,
                    Quantity = itemRequest.Quantity,
                    Size = itemRequest.Size,
                    UnitPrice = itemRequest.UnitPrice
                };

                await _orderItemRepository.NewOrderItems(new List<OrderItem> { orderItem });

                orderListEmail.AppendLine($"{product.ProductName} | Size: {orderItem.Size} | Amount: {orderItem.Quantity} | Price: ${orderItem.UnitPrice}");
            }

            orderListEmail.AppendLine($"Total Price: ${newOrder.TotalPrice}");

            var emailBody = $"Hello {newAddress.FirstName} {newAddress.LastName},\n\nYour order:\n{orderListEmail} has been received!";
            
            // replace temporary email with newOrder.Email
            await _emailService.SendEmailAsync("oelezi23@epoka.edu.al", "Order Confirmation", emailBody);

            return Ok(new { orderId = newOrderId });
        }

    }
}
