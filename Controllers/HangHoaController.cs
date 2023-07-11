using Microsoft.AspNetCore.Mvc;
using api.Models;

namespace webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HangHoaController : ControllerBase
    {
        public static List<HangHoaVM> ds_HangHoa = new List<HangHoaVM>();
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(ds_HangHoa);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            try
            {
                // Linq querry
                var hangHoa = ds_HangHoa.SingleOrDefault(hh=>hh.maHangHoa==Guid.Parse(id));
                if(hangHoa==null) return NotFound();
                return Ok(hangHoa);
            }catch
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult Create(HangHoa hangHoa)
        {
            var _hangHoa = new HangHoaVM
            {
                maHangHoa = Guid.NewGuid(),
                tenHangHoa = hangHoa.tenHangHoa,
                donGia = hangHoa.donGia
                // kasasasas
            };
            ds_HangHoa.Add(_hangHoa);
            return Ok(new{
                Success = true, Data = _hangHoa
            });
        }
        [HttpPut("{id}")]
        public IActionResult Edit(string id, HangHoaVM hangHoaEdit)
        {
            try
            {
                var hangHoa = ds_HangHoa.SingleOrDefault(hh=>hh.maHangHoa==Guid.Parse(id));
                if(hangHoa==null) return NotFound();
                // Update
                hangHoa.donGia = hangHoaEdit.donGia;
                hangHoa.tenHangHoa = hangHoaEdit.tenHangHoa;
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Remove(string id)
        {
             try
            {
                var hangHoa = ds_HangHoa.SingleOrDefault(hh=>hh.maHangHoa==Guid.Parse(id));
                if(hangHoa==null) return NotFound();
                // Remove 
                ds_HangHoa.Remove(hangHoa);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}