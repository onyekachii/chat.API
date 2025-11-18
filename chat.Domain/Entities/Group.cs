using chat.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chat.Domain.Entities;

//[Index(nameof(ID), IsUnique = true)]
public class Group<TKeyType> : IBaseEntity<TKeyType>
{
    [Required]
    public TKeyType ID { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [Required]
    public TKeyType AppId { get; set; }
    [ForeignKey(nameof(AppId))]
    public App<TKeyType> App { get; set; }

    public string Description { get; set; }

    public DateTimeOffset? CreatedDate { get; set; }
    public TKeyType? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }
    public TKeyType? UpdatedBy { get; set; }
    public DateTimeOffset? DeletedDate { get; set; }
    public TKeyType? DeletedBy { get; set; }
    public bool SoftDeleted { get; set; } = false;

    public virtual List<User<TKeyType>>? Users { get; set; }
}