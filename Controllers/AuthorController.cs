using LibraryManagement.DTOS;
using LibraryManagement.Services;
using LibraryManagement.Services.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;


        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;

        }

        [HttpGet("GetAllAuthors",Name = "GetAllAuthors")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task < ActionResult<IEnumerable<AuthorDto>> >GetAllAuthors()
        {
            var allAuthors = await _authorService.GetAll();

            return Ok(allAuthors);
        }


    }
}
