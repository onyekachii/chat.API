using chat.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.Domain.Entities
{
    public class RefreshToken : IBaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public long AppID { get; set; }
        public App App { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public string UserName { get; set; }
        [ForeignKey(nameof(UserName))]
        public User User { get; set; }
        public DateTime Expires { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime? RevokedDate { get; set; }
        public string? ReplacedByToken { get; set; }
        public bool IsActive => !IsRevoked && !IsUsed && Expires > DateTime.UtcNow;

        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set ; }
        public string? UpdatedBy { get; set ; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get;set; }
        public bool SoftDeleted { get; set; }
    }

}
