using AutoMapper;
using BookStore.Api.Dtos;
using BookStore.Api.Dtos.Category;
using BookStore.Api.Errors;
using BookStore.Api.Helpers;
using BookStore.Core;
using BookStore.Core.Models;
using BookStore.Core.Specification.CategorySpecification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers
{

    public class categoryController : BaseController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public categoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CategoryDto>> CreateCategory(CategoryCreateDto categoryCreateDto)
        {
            var category = new Category
            {
                Name = categoryCreateDto.Name,
                Description = categoryCreateDto.Description,
            };
            await unitOfWork.Repository<Category>().AddAsync(category);
            var result = await unitOfWork.CompleteAsync();
            if (result <= 0) return BadRequest(new ApiResponse(400));
            var categoryDto = mapper.Map<CategoryDto>(category);
            return Ok(categoryDto);
        }

        [HttpPut]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CategoryDto>> UpdateCategory(CategoryUpdateDto categoryUpdateDto)
        {
            var categorySopec = new CategorySpecefication(categoryUpdateDto.Id);
            var category = await unitOfWork.Repository<Category>().GetByIdAsync(categorySopec);
            if (category == null) return NotFound(new ApiResponse(404));
            category.Name = categoryUpdateDto.Name;
            category.Description = categoryUpdateDto.Description;
            var result = await unitOfWork.CompleteAsync();
            if (result <= 0) return BadRequest(new ApiResponse(400));
            var categoryDto = mapper.Map<CategoryDto>(category);
            return Ok(categoryDto);
        }

        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CategoryDto>> GetCategoryById(int id)
        {
            if (id <= 0) return BadRequest(new ApiResponse(400));
            var categorySpec = new CategorySpecefication(id);
            var category = await unitOfWork.Repository<Category>().GetByIdAsync(categorySpec);
            if (category == null) return NotFound(new ApiResponse(404));
            var categoryDto = mapper.Map<CategoryDto>(category);
            return Ok(categoryDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<MessageResponse>> DeleteCategory(int id)
        {
            if (id <= 0) return BadRequest(new ApiResponse(400));
            var category = await unitOfWork.Repository<Category>().GetByIdAsync(id);
            if (category == null) return NotFound(new ApiResponse(404));
            unitOfWork.Repository<Category>().DeleteAsync(category);
            var result = await unitOfWork.CompleteAsync();
            if (result <= 0) return BadRequest(new ApiResponse(400));
            return Ok(new MessageResponse { Message = "Category Deleted Successfully" });
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IReadOnlyList<CategoryDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<CategoryDto>>> GetAllCategories()
        {
            var categorySpec = new CategorySpecefication();
            var categories = await unitOfWork.Repository<Category>().GetAllAsync(categorySpec);
            var categoryDto = mapper.Map<IReadOnlyList<CategoryDto>>(categories);
            return Ok(categoryDto);
        }
    }
}
