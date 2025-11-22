using chat.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.Domain.Entities
{
    public class RefreshToken : IBaseEntity
    {
        public int Id { get; set; }
        public string Token { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public DateTimeOffset Expires { get; set; }
        public string CreatedByIp { get; set; } = null!;
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime? Revoked { get; set; }
        public string? RevokedByIp { get; set; }
        public string? ReplacedByToken { get; set; }
        public bool IsActive => !IsRevoked && !IsUsed && Expires > DateTimeOffset.UtcNow;

        public DateTimeOffset? CreatedDate { get; set; }
        public long? CreatedBy { get; set; }
        public DateTimeOffset? UpdatedDate { get; set ; }
        public long? UpdatedBy { get; set ; }
        public DateTimeOffset? DeletedDate { get; set; }
        public long? DeletedBy { get;set; }
        public bool SoftDeleted { get; set; }
    }

}
