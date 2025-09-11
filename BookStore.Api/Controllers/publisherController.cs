using AutoMapper;
using BookStore.Api.Dtos;
using BookStore.Api.Dtos.Publisher;
using BookStore.Api.Errors;
using BookStore.Api.Helpers;
using BookStore.Core;
using BookStore.Core.Models;
using BookStore.Core.Specification.PublisherSpec;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers
{

    public class publisherController : BaseController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public publisherController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(PublisherDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<PublisherDto>> CreatePublisher([FromBody] PublisherCreateDto publisherCreateDto)
        {
            var publisher = new Publisher
            {
                Name = publisherCreateDto.Name,
                Email = publisherCreateDto.Email,
                PhoneNumber = publisherCreateDto.PhoneNumber
            };
            await unitOfWork.Repository<Publisher>().AddAsync(publisher);
            var result = await unitOfWork.CompleteAsync();
            if (result <= 0) return BadRequest(new ApiResponse(400));
            var publisherDto = mapper.Map<PublisherDto>(publisher);
            return Ok(publisherDto);
        }

        [HttpPut]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(PublisherBaseDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<PublisherBaseDto>> UpdatePublisher([FromBody] PublisherUpdateDto publisherUpdateDto)
        {
            var publisherSpec = new PublisherSpecefication(publisherUpdateDto.Id);
            var publisher = await unitOfWork.Repository<Publisher>().GetByIdAsync(publisherSpec);
            if (publisher == null) return NotFound(new ApiResponse(404));
            publisher.Name = publisherUpdateDto.Name;
            publisher.Email = publisherUpdateDto.Email;
            publisher.PhoneNumber = publisherUpdateDto.PhoneNumber;
            var result = await unitOfWork.CompleteAsync();
            if (result <= 0) return BadRequest(new ApiResponse(400));
            var publisherDto = mapper.Map<PublisherDto>(publisher);
            return Ok(publisherDto);
        }

        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(PublisherBaseDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<PublisherBaseDto>> GetPublisherById(int id)
        {
            if (id <= 0) return BadRequest(new ApiResponse(400));
            var publisherSpec = new PublisherSpecefication(id);
            var publisher = await unitOfWork.Repository<Publisher>().GetByIdAsync(publisherSpec);
            if (publisher == null) return NotFound(new ApiResponse(404));
            var publisherDto = mapper.Map<PublisherDto>(publisher);
            return Ok(publisherDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<MessageResponse>> DeletePublisher(int id)
        {
            if (id <= 0) return BadRequest(new ApiResponse(400));
            var publisher = await unitOfWork.Repository<Publisher>().GetByIdAsync(id);
            if (publisher == null) return NotFound(new ApiResponse(404));
            unitOfWork.Repository<Publisher>().DeleteAsync(publisher);
            var result = await unitOfWork.CompleteAsync();
            if (result <= 0) return BadRequest(new ApiResponse(400));
            return Ok(new MessageResponse { Message = $"Publisher deleted successfully" });
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IReadOnlyList<PublisherDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<PublisherDto>>> GetAllPublisher()
        {
            var publishersSpec = new PublisherSpecefication();
            var publishers = await unitOfWork.Repository<Publisher>().GetAllAsync(publishersSpec);
            var publisherDtos = mapper.Map<IReadOnlyList<PublisherDto>>(publishers);
            return Ok(publisherDtos);
        }

    }
}
