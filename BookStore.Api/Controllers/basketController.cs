using BookStore.Api.Dtos;
using BookStore.Api.Errors;
using BookStore.Api.Helpers;
using BookStore.Core.IRepositories;
using BookStore.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers
{

    public class basketController : BaseController
    {
        private readonly IBasketRepository basketRepository;

        public basketController(IBasketRepository basketRepository)
        {
            this.basketRepository = basketRepository;
        }

        [HttpPost]
        [Authorize(Roles = Roles.Customer)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Basket), StatusCodes.Status200OK)]
        public async Task<ActionResult<Basket>> CreateOrUpdateBasket([FromBody] Basket basket)
        {
            var createdBasket = await basketRepository.CreateOrUpdateBasketAsync(basket);
            if (createdBasket == null)
                return BadRequest(new ApiResponse(400));
            return Ok(createdBasket);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Customer)]
        [ProducesResponseType(typeof(Basket), StatusCodes.Status200OK)]
        public async Task<ActionResult<Basket>> GetBasket([FromRoute] string id)
        {
            var basket = await basketRepository.GetBasketAsync(id);
            return basket == null ? Ok(new Basket(id)) : Ok(basket);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Customer)]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MessageResponse>> DeleteBasket([FromRoute] string id)
        {
            var bakset = await basketRepository.GetBasketAsync(id);
            if (bakset == null)
                return NotFound(new ApiResponse(404));
            var isDeleted = await basketRepository.DeleteBasketAsync(id);
            return isDeleted ?
                Ok(new MessageResponse { Message = $"Basket With Id {id} Deleted Succesfully" }) : BadRequest(new ApiResponse(404));
        }
    }
}
