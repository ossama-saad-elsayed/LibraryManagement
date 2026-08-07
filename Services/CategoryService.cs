using LibraryManagement.DTOS;
using LibraryManagement.Entities;
using LibraryManagement.Services.interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Services
{
    public class CategoryService:ICategoryService
    {

        readonly LibraryManagementDbContext _dbContext;
        public CategoryService(LibraryManagementDbContext dbContext)
        {

            _dbContext = dbContext;
        }

        // 1. Get All Categories
        public async Task<List<CategoryDto>> GetAllCategories()
        {
            var categories = await _dbContext.Categories
                .AsNoTracking()
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                })
                .ToListAsync();

            return categories;
        }

        // 2. Get Category By Id
        public async Task<CategoryDto?> GetCategoryById(int id)
        {
            var category = await _dbContext.Categories.FindAsync(id);

            if (category == null)
            {
                return null;
            }

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }

        // 3. Get Category By Name
        public async Task<CategoryDto?> GetCategoryByName(string name)
        {
            var category = await _dbContext.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Name == name);

            if (category == null)
            {
                return null;
            }

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }

        // 4. Add Category
        public async Task<int> AddCategory(CreateCategoryDto newCategory)
        {
            var category = new Category
            {
                Name = newCategory.Name
            };

            await _dbContext.Categories.AddAsync(category);

            await _dbContext.SaveChangesAsync();

            return category.Id;
        }

        // 5. Update Category
        public async Task<bool> UpdateCategory(CategoryDto updateCategory)
        {
            var category = await _dbContext.Categories.FindAsync(updateCategory.Id);

            if (category == null)
            {
                return false;
            }

            category.Name = updateCategory.Name;
            category.Description = updateCategory.Description;

            await _dbContext.SaveChangesAsync();

            return true;
        }

        // 6. Delete Category
        public async Task<bool> DeleteCategory(int id)
        {
            var category = await _dbContext.Categories.FindAsync(id);

            if (category == null)
            {
                return false;
            }

            _dbContext.Categories.Remove(category);

            await _dbContext.SaveChangesAsync();

            return true;
        }

    }
}
