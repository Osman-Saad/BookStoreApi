using AutoMapper;
using BookStore.Api.Dtos;
using BookStore.Api.Dtos.OrderDto;
using BookStore.Api.Errors;
using BookStore.Api.Helpers;
using BookStore.Core;
using BookStore.Core.IServices;
using BookStore.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookStore.Api.Controllers
{
    
    public class orderController : BaseController
    {
        private readonly IOrderServices orderServices;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public orderController(IOrderServices orderServices,IMapper mapper,IUnitOfWork unitOfWork)
        {
            this.orderServices = orderServices;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        [HttpPost("{basketId}")]
        [Authorize(Roles =Roles.Customer)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(OrderDto),StatusCodes.Status200OK)]
        public async Task<ActionResult<OrderDto>> CreateOrder([FromRoute] string basketId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var createdOrder = await orderServices.CreateOrderAsync(basketId, userId!);
            if (createdOrder == null)
                return BadRequest(new ApiResponse(400));
            var orderDto = mapper.Map<OrderDto>(createdOrder);
            return Ok(orderDto);
        }

        [HttpGet("{orderId}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(OrderDto),StatusCodes.Status200OK)]
        public async Task<ActionResult<OrderDto>> GetOrder(int orderId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await orderServices.GetOrderForSpecificUser(orderId, userId!);
            if (order == null)
                return NotFound(new ApiResponse(404));
            var orderDto = mapper.Map<OrderDto>(order);
            return Ok(orderDto);
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IReadOnlyList<OrderDto>),StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await orderServices.GetOrdersForSpecificUser(userId!);
            var ordersDto = mapper.Map<IReadOnlyList<OrderDto>>(orders);
            return Ok(ordersDto);
        }

        [HttpPut]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(MessageResponse),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateOrderStatus([FromQuery] int orderId, [FromQuery] string orderStatus)
        {
            var order = await unitOfWork.Repository<Order>().GetByIdAsync(orderId);
            if (order == null)
                return NotFound(new ApiResponse(404));
            if (!Enum.TryParse(typeof(OrderStatus), orderStatus, out var parsedStatus))
                return BadRequest(new ApiResponse(400));
            order.OrderStatus = (OrderStatus) parsedStatus;
            await unitOfWork.CompleteAsync();
            return Ok(new MessageResponse()
            {
                Message = "Order Status Updated Successfuly"
            });
        }
    }
}
