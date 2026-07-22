using LibraryManagement.DTOS;
using LibraryManagement.Entities;
using LibraryManagement.Services.interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
namespace LibraryManagement.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly LibraryManagementDbContext _dbContext;

        public AuthorService(LibraryManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<AuthorDto>> GetAllAuthors()
        {
            var authors = await _dbContext.Authors
                .AsNoTracking() 
                .Select(a => new AuthorDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Biography = a.Biography
                })
                .ToListAsync();

            return authors;
        }

        public async Task<AuthorDto?> GetAuthorbyId(int Id)
        {
            var author = await _dbContext.Authors.FindAsync(Id);

            if (author == null)
            {
                return null;
            }

            return new AuthorDto
            {
                Id = author.Id,
                Name = author.Name,
                Biography = author.Biography
            };
        }

        public async Task<AuthorDto?> GetAuthorbyName(string Name)
        {
            var author = await _dbContext.Authors
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Name == Name);

            if (author == null)
            {
                return null;
            }

            return new AuthorDto
            {
                Id = author.Id,
                Name = author.Name,
                Biography = author.Biography
            };
        }

        public async Task<int> AddAuthor(CreateAuthorDto NewAuthor)
        {
            var author = new Author
            {
                Name = NewAuthor.Name,
                Biography = NewAuthor.Biography
            };

            await _dbContext.Authors.AddAsync(author);
            await _dbContext.SaveChangesAsync(); 

            return author.Id;
        }

        public async Task<bool> UpdateAuthor(AuthorDto UpdateAuthor)
        {
            var author = await _dbContext.Authors.FindAsync(UpdateAuthor.Id);

            if (author == null)
            {
                return false;
            }

            author.Name = UpdateAuthor.Name;
            author.Biography = UpdateAuthor.Biography;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAuthor(int id)
        {
            var author = await _dbContext.Authors.FindAsync(id);

            if (author == null)
            {
                return false;
            }

            _dbContext.Authors.Remove(author);
            await _dbContext.SaveChangesAsync(); 
            return true;
        }
    }

}

