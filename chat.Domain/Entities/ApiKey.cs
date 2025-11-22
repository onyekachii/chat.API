using chat.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chat.Domain.Entities
{
    [Index(nameof(Key), IsUnique = true)]
    public class ApiKey :IBaseEntity
    {
        [Key]
        [MaxLength(100)]
        [Required]
        public string Key { get; set; }

        [Required]
        public long AppID { get; set; }
        [ForeignKey(nameof(AppID))]
        public App App { get; set; }
        public bool Revoked { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public long? CreatedBy { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
        public long? DeletedBy { get; set; }
        public bool SoftDeleted { get; set; }

    }
}
