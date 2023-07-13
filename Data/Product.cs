using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Data
{
    [Table("Product")]
    public class Product
    {
        public Product()
        {
            DonHangChiTiet = new List<DonHangChiTiet>();
        }
        [Key]
        public Guid Id {set;get;}
        [Required]
        [MaxLength(100)]
        public string ProductName {set;get;}
        public string Summary{set;get;}
        [Range(0,double.MaxValue)]
        public double Price{set;get;}
        [Range(0,100)]
        public double Discount{set;get;}
        public int? cateId{set;get;}
        [ForeignKey("cateId")]
        public Category Category{set;get;}
        public ICollection<DonHangChiTiet> DonHangChiTiet{set;get;}
    }
}