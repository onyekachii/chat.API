using chat.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace chat.Domain.Entities;

[Index(nameof(ID), IsUnique = true)]
public class User : IBaseEntity
{
    [Required]
    public long ID { get; set; }

    [Required]
    [MaxLength(50)]
    public string Username { get; set; }

    [Required]
    public Guid AppId { get; set; }
    public App App { get; set; }

    public DateTimeOffset? CreatedDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTimeOffset? DeletedDate { get; set; }
    public Guid? DeletedBy { get; set; }
    public bool SoftDeleted { get; set; } = false;

    public virtual List<Group>? Groups { get; set; }
}