using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Data
{
    [Table("User")]
    public class User
    {
        [Key]
        public int Id{set;get;}
        [Required]
        [MaxLength(50)]
        public string UserName{set;get;}
        [Required]
        [MaxLength(250)]
        public string Password{set;get;}
        [EmailAddress]
        public string Name{set;get;}
        public string Email{set;get;}

    }
}