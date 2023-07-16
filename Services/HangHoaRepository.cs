using api.Data;
using api.Models;
using api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class HangHoaRepository : IHangHoaRepository
    {
        public int page_Size =5;
        public int max_Size {set;get;}
        public MyDbContext _Context{set;get;}
        
        public HangHoaRepository(MyDbContext context)
        {
            _Context = context;
            max_Size = (int)Math.Ceiling(_Context.Products.Count()*1.0/page_Size);
        }

        public List<ProductVM> Search(string search)
        {
            if(search==null) search="";
            var searchList = _Context.Products.Where(p=>p.ProductName.Contains(search));
            var result = searchList.Select(pro=>
            new ProductVM{
                ProductName = pro.ProductName,
                Id = pro.Id,
                Price = pro.Price,
                CategoryName = pro.Category.cateName
            });
            return result.ToList();
        }
        
        public void CreateNewProduct(ProductModel model)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                ProductName = model.ProductName,
                cateId = model.CateId,
                Price = model.Price,
                Discount = model.Discount,
                Summary = model.Summary
            };
            _Context.Add(product);
            _Context.SaveChanges();
        }

        public List<ProductVM> GetAll()
        {
            var list = _Context.Products.Select(p=>
            new ProductVM {
                Id = p.Id,
                ProductName = p.ProductName,
                Price = p.Price,
                CategoryName = p.Category.cateName
            });
            return list.ToList();
        }
        public List<ProductVM> Paging(int page)
        {
            // if(page<1) page=1;
            // if(page>max_Size) page = max_Size;
            // var res = _Context.Products.Skip((page-1)*page_Size)
            //         .Take(page_Size)
            //         .Select(p=>new ProductVM{
            //             Id = p.Id,
            //             ProductName = p.ProductName,
            //             Price = p.Price,
            //             CategoryName = p.Category.cateName
            //             }).ToList();
            // return res;
            var proList = PaginatedList<Product>.Create(_Context.Products.Include(p=>p.Category).AsQueryable(),page,page_Size);
            var res = proList.Select(p=>new ProductVM{
                Id = p.Id ,
                ProductName = p.ProductName,
                Price = p.Price,
                CategoryName = p.Category?.cateName
            }).ToList();
            return res;
        }
    }
}