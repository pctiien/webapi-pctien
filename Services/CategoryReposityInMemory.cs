using api.Models;
using api.ViewModels;

namespace api.Services
{
    public class CategoryRepositoryInMemory : ICategoryRepository
    {
        public static List<CategoryVM> Categories = new List<CategoryVM>
        {
            new CategoryVM{cateId =1,cateName ="Tivi"},
            new CategoryVM{cateId =2,cateName ="Tu lanh"},
            new CategoryVM{cateId =3,cateName ="May giat"},
            new CategoryVM{cateId =4,cateName ="Ban phim"},
            new CategoryVM{cateId =5,cateName ="May tinh"}
        };
        public CategoryVM Add(CategoryModel model)
        {
            var category = new CategoryVM
            {
             cateId = Categories.Max(cate=>cate.cateId),
             cateName = model.CategoryName   
            };
            return category;
        }

        public void Delete(int id)
        {
            var cate = Categories.Single(cg=>cg.cateId == id);
            Categories.Remove(cate);
        }

        public List<CategoryVM> GetAllCategories()
        {
            return Categories;
        }

        public CategoryVM GetCategoryById(int categoryId)
        {
            var cate = Categories.Single(cg=>cg.cateId == categoryId);
            return cate;
        }

        public void Update(CategoryVM cate)
        {
            var category = Categories.Single(cg=>cg.cateId == cate.cateId);
            category.cateName = cate.cateName;
        }
    }
}