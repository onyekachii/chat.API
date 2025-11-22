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

    public DateTimeOffset? CreatedDate { get; set; }
    public long? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }
    public long? UpdatedBy { get; set; }
    public DateTimeOffset? DeletedDate { get; set; }
    public long? DeletedBy { get; set; }
    public bool SoftDeleted { get; set; } = false;

    public virtual List<Group>? Groups { get; set; }
}