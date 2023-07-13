using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Data
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int cateId{set;get;}
        [Required]
        [MaxLength(50)]
        public string cateName{set;get;}


    }
}