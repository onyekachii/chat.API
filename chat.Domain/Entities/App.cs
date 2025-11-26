
using chat.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace chat.Domain.Entities;

[Index(nameof(ID), IsUnique = true)]
[Index(nameof(Name), IsUnique = true)]
public class App : IBaseEntity
{
    [Required]
    [Key]
    public long ID { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    public int JwtAccessExpiryMinutes { get; set; }
    public DateTimeOffset? CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTimeOffset? DeletedDate { get; set; }
    public string? DeletedBy { get; set; }
    public bool SoftDeleted { get; set; } = false;

    public virtual List<User>? AppUsers { get; set; }
    public virtual List<Group>? Groups { get; set; }
}