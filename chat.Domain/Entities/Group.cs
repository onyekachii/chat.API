using chat.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chat.Domain.Entities;

public class Group : IBaseEntity
{
    [Required]
    [Key]
    public long ID { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [Required]
    public long AppId { get; set; }
    [ForeignKey(nameof(AppId))]
    public App App { get; set; }

    public string Description { get; set; }

    public string? DisplayName { get; set; }

    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? DeletedDate { get; set; }
    public string? DeletedBy { get; set; }
    public bool SoftDeleted { get; set; } = false;

    public virtual List<User>? Users { get; set; } = new List<User>();
}