using System.ComponentModel.DataAnnotations;

namespace chat.Domain.Entities;

[Index(nameof(ID), IsUnique = true)]
public class Group<TKeyType> : IBaseEntity<TKeyType>
{
    [Required]
    [Unique]
    public TKeyType ID { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [Required]
    public TKeyType AppId { get; set; }

    public string Description { get; set; }
}