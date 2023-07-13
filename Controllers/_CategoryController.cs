using api.Models;
using api.Services;
using api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Controller]
    [Route("api/[Controller]")]
    public class _CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _ICategoryReposity;
        public _CategoryController(ICategoryRepository ICategoryReposity)
        {
            _ICategoryReposity = ICategoryReposity;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var categories = _ICategoryReposity.GetAllCategories();
                return Ok(categories);
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var category = _ICategoryReposity.GetCategoryById(id);
                if(category==null) return NotFound();
                return Ok(category);
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult Create(CategoryModel model) 
        {
            try
            {
                var category = _ICategoryReposity.Add(model);
                return Ok(category);
            }
            catch (System.Exception)
            {
                
                return BadRequest();
            }
        }
        [HttpPut]
        public IActionResult UpdateCategory(CategoryVM modelVM)
        {
            try
            {
                _ICategoryReposity.Update(modelVM);
                return Ok();
            }
            catch (System.Exception)
            {
                
                return BadRequest();
            }

        }
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            try
            {
                _ICategoryReposity.Delete(id);
                return Ok();
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }
    }
}