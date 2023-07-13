using api.Models;
using api.ViewModels;

namespace api.Services
{
    public interface ICategoryRepository 
    {
        public List<CategoryVM> GetAllCategories();
        CategoryVM GetCategoryById(int categoryId);
        CategoryVM Add(CategoryModel model);
        void Update(CategoryVM cate);
        void Delete(int id);
    }
}