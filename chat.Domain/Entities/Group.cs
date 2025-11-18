using chat.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chat.Domain.Entities;

[Index(nameof(ID), IsUnique = true)]
public class Group : IBaseEntity
{
    [Required]
    public long ID { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [Required]
    public Guid AppId { get; set; }
    [ForeignKey(nameof(AppId))]
    public App App { get; set; }

    public string Description { get; set; }

    public DateTimeOffset? CreatedDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTimeOffset? DeletedDate { get; set; }
    public Guid? DeletedBy { get; set; }
    public bool SoftDeleted { get; set; } = false;

    public virtual List<User>? Users { get; set; }
}