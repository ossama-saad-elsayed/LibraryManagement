using LibraryManagement.DTOS;

namespace LibraryManagement.Services.interfaces
{
    public interface IAuthorService
    {
        Task<List<AuthorDto>> GetAllAuthors();

        Task<AuthorDto?> GetAuthorbyId(int Id);

        Task<AuthorDto?> GetAuthorbyName(string Name);

        Task<int> AddAuthor(CreateAuthorDto NewAuthor);

        Task<bool> UpdateAuthor(AuthorDto UpdateAuthor);

        Task<bool> DeleteAuthor(int id);
    }
}
