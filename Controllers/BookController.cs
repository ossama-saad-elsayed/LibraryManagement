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

        [HttpGet("GetALlBooks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<BookDto>>> GetAllBooks()
        {
            var books = await _bookService.GetAllBooks();
            return Ok(books); 
        }

        // 2. GET: api/book/5
        [HttpGet("{id}", Name = "GetBookbyID")] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookDto>> GetBookbyID(int id)
        {
            var book = await _bookService.GetBookbyId(id);

            if (book == null) 
            {
                return NotFound(new { message = $"Book with ID {id} not found." });
            }

            return Ok(book);
        }

        // 3. GET: api/book/title/search?Title=abc
        [HttpGet("title/search", Name = "GetBookbyTitle")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookDto>> GetBookbyTitle([FromQuery] string Title)
        {
            var book = await _bookService.GetBookbyTitle(Title);

            if (book == null) 
            {
                return NotFound(new { message = $"Book with title '{Title}' not found." });
            }

            return Ok(book);
        }

        // GET: api/book/category/action
        [HttpGet("category/{categoryName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<BookDto>>> GetBooksByCategory(string categoryName)
        {
            var books = await _bookService.GetBooksByCategoryName(categoryName);
            return Ok(books);
        }

        //  GET: api/book/author/naguid
        [HttpGet("author/{authorName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<BookDto>>> GetBooksByAuthor(string authorName)
        {
            var books = await _bookService.GetBooksByAuthorName(authorName);
            return Ok(books);
        }

        //  POST: api/book 
        [HttpPost("AddByID")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddBook([FromBody] CreateBookDto createBookDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            int newBookId = await _bookService.AddBook(createBookDto);

            return CreatedAtAction(nameof(GetBookbyID), new { id = newBookId }, createBookDto);
        }

        //  POST: api/book/AddByNames 
        [HttpPost("AddByNames")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddBookByNames([FromBody] CreateBookByNamesDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            int newBookId = await _bookService.AddBookByNames(dto);

            return CreatedAtAction(nameof(GetBookbyID), new { id = newBookId }, dto);
        }

        //  PUT: api/book  IDs
        [HttpPut("UpdateByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBook([FromBody] BookDto bookDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            bool result = await _bookService.UpdateBook(bookDto);
            if (!result)
            {
                return NotFound(new { message = $"Book with ID {bookDto.Id} not found." });
            }

            return Ok(new { message = "Successfully updated." });
        }

        //  PUT: api/book/UpdateByNames 
        [HttpPut("UpdateByNames")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateBookByNames([FromBody] UpdateBookByNamesDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            bool result = await _bookService.UpdateBookByNames(dto);
            if (!result)
            {
                return NotFound(new { message = $"Book with ID {dto.Id} not found." });
            }

            return Ok(new { message = "Successfully updated using names." });
        }

        //  DELETE: api/book/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBook(int id)
        {
            bool result = await _bookService.DeleteBook(id);

            if (!result)
            {
                return NotFound(new { message = $"Book with ID {id} not found." });
            }

            return NoContent();
        }
    }
}
