using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.DTOS;
using LibraryManagement.Services.interfaces;

namespace LibraryManagement.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        // Constructor Injection
        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        // 1. GET: api/author 
        [HttpGet("GetAllAuthors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<AuthorDto>>> GetAllAuthors()
        {
            var authors = await _authorService.GetAllAuthors();
            return Ok(authors);
        }

        // 2. GET: api/author/5 
        [HttpGet("{id}", Name = "GetAuthorById")] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AuthorDto>> GetAuthorById(int id)
        {
            var author = await _authorService.GetAuthorbyId(id);

            if (author == null)
            {
                return NotFound(new { message = $"Author with ID {id} not found." });
            }

            return Ok(author);
        }

        // 3. GET: api/author/search?name=abc 
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AuthorDto>> GetAuthorByName([FromQuery] string name)
        {
            var author = await _authorService.GetAuthorbyName(name);

            if (author == null)
            {
                return NotFound(new { message = $"Author with name '{name}' not found." });
            }

            return Ok(author);
        }

        // 4. POST: api/author 
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAuthor([FromBody] CreateAuthorDto createAuthorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int newAuthorId = await _authorService.AddAuthor(createAuthorDto);

            
            return CreatedAtAction(nameof(GetAuthorById), new { id = newAuthorId }, createAuthorDto);
        }

        // 5. PUT: api/author 
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAuthor([FromBody] AuthorDto authorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool result = await _authorService.UpdateAuthor(authorDto);

            if (!result)
            {
                return NotFound(new { message = $"Author with ID {authorDto.Id} not found." });
            }

            return Ok(new { message = "Author updated successfully." });
        }

        // 6. DELETE: api/author/5 
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            bool result = await _authorService.DeleteAuthor(id);

            if (!result)
            {
                return NotFound(new { message = $"Author with ID {id} not found." });
            }

            return NoContent();
        }
    }
}