using LibraryManagement.DTOS;

namespace LibraryManagement.Services.interfaces
{
    public interface IBookService
    {
          Task<List<BookDto>> GetAllBooks();
          Task<BookDto?> GetBookbyId(int Id);
          Task<BookDto?> GetBookbyTitle(string Title);
          Task<int> AddBook(CreateBookDto NewBook);
          Task<bool> UpdateBook(BookDto UpdateBook);
          Task<bool> DeleteBook(int id);

    }
}
