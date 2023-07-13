using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.ViewModels
{
    public class CategoryVM
    {
        public int cateId{set;get;}
        public string cateName{set;get;}
    }
}