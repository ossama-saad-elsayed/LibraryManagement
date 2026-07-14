using LibraryManagement.DTOS;
using LibraryManagement.Services.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {   
        readonly IBookService _bookService; 
          
      public  BookController (IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("GetAllBooks",Name = "GetAllBooks")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<BookDto>> GetAllBooks()
        {
            var books = await _bookService.GetAllBooks();

            return Ok(books);
        }

        [HttpGet("GetBookbyID", Name = "GetBookbyID")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<BookDto>> GetBookbyID(int id)
        {
            var book = await _bookService.GetBookbyId(id);

            return Ok(book);
        }
        [HttpGet("GetBookbyTitle", Name = "GetBookbyTitle")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<BookDto>> GetBookbyTitle(string Title)
        {
            var book = await _bookService.GetBookbyTitle( Title);

            return Ok(book);
        }


        [HttpPost("AddBook", Name = "AddBook")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddBook([FromBody] CreateBookDto createBookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int newBookId = await _bookService.AddBook(createBookDto);

            
            return CreatedAtAction(nameof(GetBookbyID), new { id = newBookId }, createBookDto);
        }

        [HttpPut("UpdateBook", Name = "UpdateBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async  Task<IActionResult> UpdateBook (BookDto bookDto)
        {
           bool result  =await _bookService.UpdateBook(bookDto);
            if (!result)
            {
                return NotFound(new { message = $" not found book with number {bookDto.Id} " });
            }

            return Ok(new { message = " successfully  updated" });
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBook(int id)
        {
            bool result = await _bookService.DeleteBook(id);

            if (!result)
            {
                return NotFound(new { message = $" not found with : {id} " });
            }

            return NoContent();
        }
    }
}
