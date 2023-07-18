using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Data
{
    [Table("RefreshToken")]
    public class RefreshToken
    {
        [Key]
        public Guid Id{set;get;}
        public int UserId{set;get;}
        [ForeignKey(nameof(UserId))]
        public User User{set;get;}
        public string Token{set;get;}
        public string JwtId{set;get;}
        public bool IsUsed{set;get;}
        public bool IsRevoked{set;get;}
        public DateTime IssuedAt{set;get;}
        public DateTime ExpiredAt{set;get;}

    }
}