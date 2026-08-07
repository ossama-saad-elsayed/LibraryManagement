using LibraryManagement.Entities;
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
                AuthorName = b.Author.Name,
                CategoryName = b.Category.Name,
                PublisherName = b.Publisher.Name,
            }
            ).ToListAsync();

            return books;
        }

        public async Task<BookDto?> GetBookbyId(int Id)
        {
            var book = await _dbContext.Books
        .AsNoTracking()
        .Include(b => b.Author)
        .Include(b => b.Category)
        .Include(b => b.Publisher)
        .FirstOrDefaultAsync(b => b.Id == Id);

            if (book == null)
            {
                return null;
            }

            return new BookDto
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
                AuthorName = book.Author.Name,      
                CategoryName = book.Category.Name,  
                PublisherName = book.Publisher.Name 
            };
        }

        public async Task<BookDto?> GetBookbyTitle(string Title)
        {
            var book = await _dbContext.Books
        .AsNoTracking()
        .Include(b => b.Author)
        .Include(b => b.Category)
        .Include(b => b.Publisher)
        .FirstOrDefaultAsync(b => b.Title == Title);
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
                AuthorName = book.Author.Name,
                CategoryName = book.Category.Name,
                PublisherName = book.Publisher.Name
            };


            return bookDto;
        }

        public async Task<List<BookDto>> GetBooksByCategoryName(string categoryName)
        {
            return await _dbContext.Books
                .AsNoTracking()
                .Where(b => b.Category.Name.Contains(categoryName)) 
                .Select(b => new BookDto
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
                    AuthorName = b.Author.Name,
                    CategoryName = b.Category.Name,
                    PublisherName = b.Publisher.Name
                })
                .ToListAsync();
        }

        public async Task<List<BookDto>> GetBooksByAuthorName(string authorName)
        {
            return await _dbContext.Books
                .AsNoTracking()
                .Where(b => b.Author.Name.Contains(authorName))
                .Select(b => new BookDto
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
                    AuthorName = b.Author.Name,
                    CategoryName = b.Category.Name,
                    PublisherName = b.Publisher.Name
                })
                .ToListAsync();
        }
        public async Task<int> AddBook(CreateBookDto NewBook)
        {
            var book = new Book
            {
                Isbn = NewBook.ISBN,
                Title = NewBook.Title,
                PublicationYear = NewBook.PublicationYear,
                CopiesOwned = NewBook.CopiesOwned,
                AvailableCopies = NewBook.CopiesOwned,
                AuthorId = NewBook.AuthorId,
                CategoryId = NewBook.CategoryId,
                PublisherId = NewBook.PublisherId,
            };

            await _dbContext.AddAsync(book);

            await _dbContext.SaveChangesAsync();

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
            book.AvailableCopies = UpdateBook.AvailableCopies;
            book.AuthorId = UpdateBook.AuthorId;
            book.CategoryId = UpdateBook.CategoryId;
            book.PublisherId = UpdateBook.PublisherId;


            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<int> AddBookByNames(CreateBookByNamesDto dto)
        {
            var author = await _dbContext.Authors.FirstOrDefaultAsync(a => a.Name == dto.AuthorName);
            if (author == null)
            {
                author = new Author { Name = dto.AuthorName };
                await _dbContext.Authors.AddAsync(author);
            }

            var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Name == dto.CategoryName);
            if (category == null)
            {
                category = new Category { Name = dto.CategoryName };
                await _dbContext.Categories.AddAsync(category);
            }

            var publisher = await _dbContext.Publishers.FirstOrDefaultAsync(p => p.Name == dto.PublisherName);
            if (publisher == null)
            {
                publisher = new Publisher { Name = dto.PublisherName };
                await _dbContext.Publishers.AddAsync(publisher);
            }

           
            var book = new Book
            {
                Isbn = dto.ISBN,
                Title = dto.Title,
                PublicationYear = dto.PublicationYear,
                CopiesOwned = dto.CopiesOwned,
                Author = author,
                Category = category,
                Publisher = publisher
            };

            await _dbContext.Books.AddAsync(book);
            await _dbContext.SaveChangesAsync(); 

            return book.Id;
        }

        public async Task<bool> UpdateBookByNames(UpdateBookByNamesDto dto)
        {
            // 1. نجيب الكتاب الحقيقي أولاً
            var book = await _dbContext.Books.FindAsync(dto.Id);
            if (book == null)
            {
                return false;
            }

            // 2. تشيك أو إنشاء الـ Author الجديد
            var author = await _dbContext.Authors.FirstOrDefaultAsync(a => a.Name == dto.AuthorName);
            if (author == null)
            {
                author = new Author { Name = dto.AuthorName };
                await _dbContext.Authors.AddAsync(author);
            }

            // 3. تشيك أو إنشاء الـ Category الجديد
            var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Name == dto.CategoryName);
            if (category == null)
            {
                category = new Category { Name = dto.CategoryName };
                await _dbContext.Categories.AddAsync(category);
            }

            // 4. تشيك أو إنشاء الـ Publisher الجديد
            var publisher = await _dbContext.Publishers.FirstOrDefaultAsync(p => p.Name == dto.PublisherName);
            if (publisher == null)
            {
                publisher = new Publisher { Name = dto.PublisherName };
                await _dbContext.Publishers.AddAsync(publisher);
            }

            // 5. تعديل بيانات الكتاب وربطه بالقيم الجديدة
            book.Isbn = dto.ISBN;
            book.Title = dto.Title;
            book.PublicationYear = dto.PublicationYear;
            book.CopiesOwned = dto.CopiesOwned;
            book.Author = author;
            book.Category = category;
            book.Publisher = publisher;

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
          await   _dbContext.SaveChangesAsync();
            return true;
        }

    }
}