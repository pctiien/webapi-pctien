using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/v11/[Controller]")]
    public class ProductController : ControllerBase
    {
        public IHangHoaRepository _IHangHoaRepository;
        public ProductController(IHangHoaRepository IHangHoaRepository)
        {
            _IHangHoaRepository = IHangHoaRepository;
        }
         [HttpGet]
         public IActionResult GetAll()
         {
             try
        {
                 return Ok(_IHangHoaRepository.GetAll());
        }
             catch (System.Exception)
             {
                 return BadRequest();
             }
         }
         [HttpGet("Search")]
         public IActionResult Search(string? search)
         {
             try
             {
                if(string.IsNullOrEmpty(search)) search ="";
                 return Ok(_IHangHoaRepository.Search(search));
             }
         catch 
             {
                 return BadRequest("We can't find the products");
             }
         }
         [HttpGet("Paging")]
         public IActionResult Paging(int page=1)
         {
            try
            {
                var res = _IHangHoaRepository.Paging(page);
                return Ok(res);
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
         }
         [HttpPost]
         public IActionResult CreateNewProduct(ProductModel model)
         {
             try
             {
                 _IHangHoaRepository.CreateNewProduct(model);
                 return Ok();
             }
             catch (System.Exception)
             {
                 return BadRequest();
             }
         }
    }
}