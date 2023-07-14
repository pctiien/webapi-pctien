using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class CategoryModel 
    {
        [NotMapped]
        [Required]
        [MaxLength(50)]
        public string CategoryName {set;get;} 
    }
}