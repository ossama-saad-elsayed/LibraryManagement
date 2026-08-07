using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.DTOS;
using LibraryManagement.Services.interfaces;

namespace LibraryManagement.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // 1. GET: api/category 
        [HttpGet("GetAllCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<CategoryDto>>> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategories();
            return Ok(categories);
        }

        // 2. GET: api/category/5 
        [HttpGet("{id}", Name = "GetCategoryById")] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryDto>> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryById(id);

            if (category == null)
            {
                return NotFound(new { message = $"Category with ID {id} not found." });
            }

            return Ok(category);
        }

        // 3. GET: api/category/search?name=abc 
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryDto>> GetCategoryByName([FromQuery] string name)
        {
            var category = await _categoryService.GetCategoryByName(name);

            if (category == null)
            {
                return NotFound(new { message = $"Category with name '{name}' not found." });
            }

            return Ok(category);
        }

        // 4. POST: api/category 
        [HttpPost("AddCategory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int newCategoryId = await _categoryService.AddCategory(createCategoryDto);

            return CreatedAtAction(nameof(GetCategoryById), new { id = newCategoryId }, createCategoryDto);
        }

        // 5. PUT: api/category 
        [HttpPut("UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool result = await _categoryService.UpdateCategory(categoryDto);

            if (!result)
            {
                return NotFound(new { message = $"Category with ID {categoryDto.Id} not found." });
            }

            return Ok(new { message = "Category updated successfully." });
        }

        // 6. DELETE: api/category/5 
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            bool result = await _categoryService.DeleteCategory(id);

            if (!result)
            {
                return NotFound(new { message = $"Category with ID {id} not found." });
            }

            return NoContent(); 
        }
    }
}