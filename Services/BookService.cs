using LibraryManagement.Models;
using LibraryManagement.DTOS;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Services.interfaces;
using System.Runtime.CompilerServices;

namespace LibraryManagement.Services
{
    public class BookService : IBookService
    {
        readonly LibraryManagementDbContext _dbContext;
        public BookService(LibraryManagementDbContext dbContext)
        {

            _dbContext = dbContext;
        }

        public async Task<List<BookDto>> GetAllBooks()
        {
            var books = await _dbContext.Books.AsNoTracking().Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                ISBN = b.Isbn,
                PublicationYear = b.PublicationYear ?? 0,
                CopiesOwned = b.CopiesOwned,
                AvailableCopies = b.AvailableCopies,
                AuthorId = b.AuthorId,
                CategoryId = b.CategoryId,
                PublisherId = b.PublisherId,
                AuthorName = "O",
                CategoryName = "O",
                PublisherName = "O"
            }
            ).ToListAsync();

            return books;
        }

        public async Task<BookDto?> GetBookbyId(int Id)
        {
            var book = await _dbContext.Books.FindAsync(Id);
            if (book == null)
            {
                return null;
            }

            BookDto bookDto = new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                ISBN = book.Isbn,
                PublicationYear = book.PublicationYear ?? 0,
                CopiesOwned = book.CopiesOwned,
                AvailableCopies = book.AvailableCopies,
                AuthorId = book.AuthorId,
                CategoryId = book.CategoryId,
                PublisherId = book.PublisherId,
                AuthorName = "O",
                CategoryName = "O",
                PublisherName = "O"
            };


            return bookDto;
        }

        public async Task<BookDto?> GetBookbyTitle(string Title)
        {
            var book = await _dbContext.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Title == Title);
            if (book == null)
            {
                return null;
            }

            BookDto bookDto = new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                ISBN = book.Isbn,
                PublicationYear = book.PublicationYear ?? 0,
                CopiesOwned = book.CopiesOwned,
                AvailableCopies = book.AvailableCopies,
                AuthorId = book.AuthorId,
                CategoryId = book.CategoryId,
                PublisherId = book.PublisherId,
                AuthorName = "O",
                CategoryName = "O",
                PublisherName = "O"
            };


            return bookDto;
        }

        public async Task<int> AddBook(CreateBookDto NewBook)
        {
            var book = new Book
            {
                Isbn = NewBook.ISBN,
                Title = NewBook.Title,
                PublicationYear = NewBook.PublicationYear,
                CopiesOwned = NewBook.CopiesOwned,
                AuthorId = NewBook.AuthorId,
                CategoryId = NewBook.CategoryId,
                PublisherId = NewBook.PublisherId,
            };

            await _dbContext.AddAsync(book);

            _dbContext.SaveChanges();

            return book.Id;
        }

        public async Task<bool> UpdateBook(BookDto UpdateBook)
        {
            var book = await _dbContext.Books.FindAsync(UpdateBook.Id);

            if (book == null)
            {
                return false;
            }

            book.Isbn = UpdateBook.ISBN;
            book.Title = UpdateBook.Title;
            book.PublicationYear = UpdateBook.PublicationYear;
            book.CopiesOwned = UpdateBook.CopiesOwned;
            book.AuthorId = UpdateBook.AuthorId;
            book.CategoryId = UpdateBook.CategoryId;
            book.PublisherId = UpdateBook.PublisherId;


            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteBook(int id)
        {
            var book = await _dbContext.Books.FindAsync(id);

            if (book == null)
            {
                return false;
            }

            _dbContext.Remove(book);

            return true;
        }

    }
}