using LibraryManagement.DTOS;
using LibraryManagement.Models;
using LibraryManagement.Services.interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
namespace LibraryManagement.Services
{
    public class AuthorService : IAuthorService
    {
        readonly LibraryManagementDbContext _dbContext;
        public AuthorService(LibraryManagementDbContext dbContext) { 

            _dbContext = dbContext;
        }


        public  async Task< List<AuthorDto>> GetAll()
        {
          var AllAuthors = await  _dbContext.Set<Author>().Select(a=> new AuthorDto 
            { Id = a.Id,
             Name= a.Name , 
                Biography = a.Biography 
            }
            ).ToListAsync();

            return  AllAuthors ;
        }

        //public async Task<AuthorDto> GetAuthor(string Name)
        //{
            
            
        //}

    }
}
