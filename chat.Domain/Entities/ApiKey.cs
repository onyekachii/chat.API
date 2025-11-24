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
        public bool Revoked { get; set; } = false;
        public DateTimeOffset? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
        public bool SoftDeleted { get; set; } = false;

    }
}
