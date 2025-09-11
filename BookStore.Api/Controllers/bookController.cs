using AutoMapper;
using BookStore.Api.Dtos;
using BookStore.Api.Dtos.BooksDto;
using BookStore.Api.Errors;
using BookStore.Api.Helpers;
using BookStore.Core;
using BookStore.Core.Models;
using BookStore.Core.Specification.BookSpecification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers
{

    public class bookController : BaseController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly FileManager fileManager;

        public bookController(IUnitOfWork unitOfWork, IMapper mapper, FileManager fileManager)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.fileManager = fileManager;
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BookDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<BookDto>> CreateBook([FromForm] BookCreateDto bookCreateDto)
        {
            var book = new Book
            {
                Name = bookCreateDto.Name,
                Quantity = bookCreateDto.Quantity,
                PublishDate = bookCreateDto.PublishDate,
                Price = bookCreateDto.Price,
                PageCount = bookCreateDto.PageCount,
                Language = bookCreateDto.Language,
                CoverUrl = await fileManager.UploadFile(bookCreateDto.CoverImage, "uploads/bookCovers"),
                Description = bookCreateDto.Description,
                CategoryId = bookCreateDto.CategoryId,
                PublisherId = bookCreateDto.PublisherId,
            };
            if (bookCreateDto.AuthorsIds.Any())
            {
                foreach (var authorId in bookCreateDto.AuthorsIds)
                {
                    var author = await unitOfWork.Repository<Author>().GetByIdAsync(authorId);
                    if (author == null) return BadRequest(new ApiResponse(400));
                    book.Authors.Add(author);
                }
            }
            await unitOfWork.Repository<Book>().AddAsync(book);
            var result = await unitOfWork.CompleteAsync();
            if (result <= 0) return BadRequest(new ApiResponse(400));
            var bookSpec = new BookSpecificationWithProjection(book.Id);
            var createdBook = await unitOfWork.Repository<Book>().GetByIdAsync(bookSpec);
            return Ok(mapper.Map<BookDto>(createdBook));
        }

        [HttpPut]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BookDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<BookDto>> UpdateBooke([FromForm] BookUpdateDto bookUpdateDto)
        {
            var bookSpec = new BookSpecefication(bookUpdateDto.Id);
            var book = await unitOfWork.Repository<Book>().GetByIdAsync(bookSpec);
            if (book == null) return NotFound(new ApiResponse(404));
            if (book.CoverUrl != null)
                fileManager.DeleteFile(book.CoverUrl);
            book.Name = bookUpdateDto.Name;
            book.Price = bookUpdateDto.Price;
            book.Description = bookUpdateDto.Description;
            book.CategoryId = bookUpdateDto.CategoryId;
            book.Language = bookUpdateDto.Language;
            book.PageCount = bookUpdateDto.PageCount;
            book.PublishDate = bookUpdateDto.PublishDate;
            book.Quantity = book.Quantity;
            book.PublisherId = bookUpdateDto.PublisherId;
            book.CoverUrl = await fileManager.UploadFile(bookUpdateDto.CoverImage, "uploads/bookCovers");
            book.Authors.Clear();
            if (bookUpdateDto.AuthorsIds.Any())
            {
                foreach (var authorId in bookUpdateDto.AuthorsIds)
                {
                    var author = await unitOfWork.Repository<Author>().GetByIdAsync(authorId);
                    if (author == null) return BadRequest(new ApiResponse(400));
                    book.Authors.Add(author);
                }
            }
            var result = await unitOfWork.CompleteAsync();
            if (result <= 0) return BadRequest(new ApiResponse(400));
            var bookSpecwithProjection = new BookSpecificationWithProjection(book.Id);
            var updatedBook = await unitOfWork.Repository<Book>().GetByIdAsync(bookSpecwithProjection);
            return Ok(mapper.Map<BookDto>(updatedBook));
        }


        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<BookDto>> GetBookById(int id)
        {
            if (id <= 0)
                return BadRequest(new ApiResponse(400));
            var bookSpec = new BookSpecificationWithProjection(id);
            var book = await unitOfWork.Repository<Book>().GetByIdAsync(bookSpec);
            if (book is null)
                return NotFound(new ApiResponse(404));
            return Ok(mapper.Map<BookDto>(book));
        }

        [HttpGet]
        [Authorize]
        [CacheResponse(300)]
        [ProducesResponseType(typeof(Pagination<BookDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Pagination<BookDto>>> GetAll([FromForm] BookSpecificationParams specParams)
        {
            var bookSpec = new BookSecificationWithParams(specParams);
            var books = await unitOfWork.Repository<Book>().GetAllAsync(bookSpec);
            var bookCount = await unitOfWork.Repository<Book>().GetCountAsync(bookSpec);
            var booksDto = mapper.Map<IReadOnlyList<BookDto>>(books);
            return Ok(new Pagination<BookDto>
            {
                Count = bookCount,
                PageIndex = specParams.PageIndex,
                PageSize = specParams.PageSize,
                Data = booksDto
            });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MessageResponse>> DeleteBook(int id)
        {
            if (id <= 0) return BadRequest(new ApiResponse(400));
            var book = await unitOfWork.Repository<Book>().GetByIdAsync(id);
            if (book == null) return NotFound(new ApiResponse(404));
            unitOfWork.Repository<Book>().DeleteAsync(book);
            var result = await unitOfWork.CompleteAsync();
            if (result <= 0) return BadRequest(new ApiResponse(400));
            return Ok(new MessageResponse { Message = "Book Deleted Successfully" });
        }
    }
}
