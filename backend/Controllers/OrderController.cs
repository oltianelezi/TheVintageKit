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
        private readonly EmailService _emailService;

        public OrderController(
            OrderRepository orderRepository,
            AddressRepository addressRepository,
            OrderItemRepository orderItemRepository,
            EmailService emailService
            )
        {
            _orderRepository = orderRepository;
            _addressRepository = addressRepository;
            _orderItemRepository = orderItemRepository;
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

            List<OrderItem> OrderList = new List<OrderItem>();

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
            }

            var emailBody = $"Hello {newAddress.FirstName} {newAddress.LastName},\n\nYour order #{newOrderId} has been received!";

            // Replace temp mail with NewOrder.Email
            await _emailService.SendEmailAsync("yocacaw842@poesd.com", "Order Confirmation", emailBody);

            await _orderItemRepository.NewOrderItems(OrderList);


            return Ok();
        }
    }
}
