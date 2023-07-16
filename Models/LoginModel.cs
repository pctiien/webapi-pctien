using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class LoginModel
    {
        [Required]
        [MaxLength(50)]
        public string UserName{set;get;}
        [Required]
        [MaxLength(250)]
        public string Password{set;get;}
    }
}