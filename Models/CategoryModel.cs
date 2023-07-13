using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class CategoryModel 
    {
        [Required]
        [MaxLength(50)]
        public string CategoryName {set;get;} 
    }
}