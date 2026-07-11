using LibraryManagement.DTOS;

namespace LibraryManagement.Services.interfaces
{
    public interface IAuthorService
    {
       Task<List<AuthorDto>> GetAll();
    }
}
