using AutoMapper;
using BookStore.Api.Dtos;
using BookStore.Api.Dtos.Author;
using BookStore.Api.Errors;
using BookStore.Api.Helpers;
using BookStore.Core;
using BookStore.Core.Models;
using BookStore.Core.Specification.AuthorSpec;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers
{

    public class authorController : BaseController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;


        public authorController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AuthorDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<AuthorDto>> AddAuthor([FromBody] AuthorCreateDto author)
        {
            var newAuthor = new Author
            {
                Name = author.Name,
                Biography = author.Biography,
                BirthDate = author.BirthDate
            };
            await unitOfWork.Repository<Author>().AddAsync(newAuthor);
            var result = await unitOfWork.CompleteAsync();
            if (result <= 0) return BadRequest(new ApiResponse(400));
            var authorDto = mapper.Map<AuthorDto>(newAuthor);
            return Ok(authorDto);
        }

        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(AuthorDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<AuthorDto>> GetAutorById(int id)
        {
            if (id <= 0) return BadRequest(new ApiResponse(400));
            var authorSpec = new AuthorSpecefication(id);
            var author = await unitOfWork.Repository<Author>().GetByIdAsync(authorSpec);
            if (author == null) return NotFound(new ApiResponse(404));
            var authorDto = mapper.Map<AuthorDto>(author);
            return Ok(authorDto);
        }

        [HttpPut]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(AuthorDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<AuthorDto>> UpdateAuthor([FromBody] AuthorUpdateDto authorUpdate)
        {
            var authorSpec = new AuthorSpecefication(authorUpdate.Id);
            var author = await unitOfWork.Repository<Author>().GetByIdAsync(authorSpec);
            if (author == null) return NotFound(new ApiResponse(404));
            author.Biography = authorUpdate.Biography;
            author.BirthDate = authorUpdate.BirthDate;
            author.Name = authorUpdate.Name;
            unitOfWork.Repository<Author>().UpdateAsync(author);
            var result = await unitOfWork.CompleteAsync();
            if (result <= 0) return BadRequest(new ApiResponse(400));
            var authorDto = mapper.Map<AuthorDto>(author);
            return Ok(authorDto);
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IReadOnlyList<AuthorDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<AuthorDto>>> GetAllAuthors()
        {
            var authorSpec = new AuthorSpecefication();
            var authors = await unitOfWork.Repository<Author>().GetAllAsync(authorSpec);
            var authorsDto = mapper.Map<IReadOnlyList<AuthorDto>>(authors);
            return Ok(authorsDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteAuthor(int id)
        {
            if (id <= 0) return BadRequest(new ApiResponse(400));
            var author = await unitOfWork.Repository<Author>().GetByIdAsync(id);
            if (author == null) return NotFound(new ApiResponse(404));
            unitOfWork.Repository<Author>().DeleteAsync(author);
            var result = await unitOfWork.CompleteAsync();
            if (result <= 0) return BadRequest(new ApiResponse(400));
            return Ok(new MessageResponse() { Message = "Author Deleted Successfuly" });
        }
    }
}
