using LibraryManagement.DTOS;

namespace LibraryManagement.Services.interfaces
{
    public interface ICategoryService 
    {
        Task<List<CategoryDto>> GetAllCategories();

        Task<CategoryDto?> GetCategoryById(int id);

        Task<CategoryDto?> GetCategoryByName(string name);

        Task<int> AddCategory(CreateCategoryDto newCategory);

        Task<bool> UpdateCategory(CategoryDto updateCategory);

        Task<bool> DeleteCategory(int id);
    }
}
