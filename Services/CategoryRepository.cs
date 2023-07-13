using api.Data;
using api.Models;
using api.ViewModels;

namespace api.Services
{
    public class CategoryRepository : ICategoryRepository
    {
        public MyDbContext _context{set;get;}
        public CategoryRepository(MyDbContext context)
        {
            _context = context;
        }
        public CategoryVM Add(CategoryModel model)
        {
            var category = new Category
            {
                cateName = model.CategoryName
            };
            _context.Categories.Add(category);
            _context.SaveChanges();
            return new CategoryVM{
                cateId = category.cateId,
                cateName = category.cateName
            };
        }

        public void Delete(int id)
        {
            var category = _context.Categories.Single(cg=>cg.cateId == id);
            if(category==null)  return ;
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }

        public List<CategoryVM> GetAllCategories()
        {
            var categories = _context.Categories.Select(
                cate=>new CategoryVM{
                    cateId = cate.cateId,
                    cateName = cate.cateName
                }
            );
            return categories.ToList();
        }

        public CategoryVM GetCategoryById(int categoryId)
        {
            var cate = _context.Categories.Single(cg=>cg.cateId==categoryId);
            if(cate!=null){
                var cateVM = new CategoryVM
                {
                    cateId = cate.cateId,
                    cateName =cate.cateName
                };
                return cateVM;
            }
            return null;
        }

        public void Update(CategoryVM cate)
        {
            var category = _context.Categories.Single(cg=>cg.cateId == cate.cateId);
            if(category == null) return;
            category.cateName = cate.cateName;
            _context.SaveChanges();
        }
    }
}