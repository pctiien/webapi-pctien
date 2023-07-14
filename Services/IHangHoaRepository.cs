using api.Models;
using api.ViewModels;

namespace api.Services
{
    public interface IHangHoaRepository
    {
        List<ProductVM> Search(string search);
        void CreateNewProduct(ProductModel model);
        List<ProductVM> GetAll();
        List<ProductVM> Paging(int page);
    }
}