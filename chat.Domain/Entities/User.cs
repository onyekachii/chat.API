using chat.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace chat.Domain.Entities;

[Index(nameof(Username), IsUnique = true)]
public class User : IBaseEntity
{
    [Key]
    [Required]
    [MaxLength(50)]
    public string Username { get; set; }

    public string DisplayName { get; set; }

    [Required]
    public long AppId { get; set; }
    public App App { get; set; }

    public DateTime? CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? DeletedDate { get; set; }
    public string? DeletedBy { get; set; }
    public bool SoftDeleted { get; set; } = false;

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
}