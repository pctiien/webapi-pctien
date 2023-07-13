using api.Data;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly MyDbContext _dbContext;
        public CategoryController(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_dbContext.Categories.ToList());
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var cate = _dbContext.Categories.SingleOrDefault(cat=>cat.cateId == id);
                if(cate==null) return NotFound();
                return Ok(cate);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult CreateNewCategory(CategoryModel model)
        {
            try
            {
                var newCate = new Category
                {
                    cateName = model.CategoryName

                };
                _dbContext.Add(newCate);
                _dbContext.SaveChanges();
                return Ok(newCate);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateCategoryById(int id,CategoryModel model)
        {
            try
            {
                var cate = _dbContext.Categories.SingleOrDefault(cg=>cg.cateId==id);
                if(cate==null) return NotFound();
                cate.cateName = model.CategoryName;
                _dbContext.SaveChanges();
                return Ok(cate);     
            }
            catch
            {
                return BadRequest();
            }
            
        }
    }
}