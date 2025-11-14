using System.ComponentModel.DataAnnotations;

namespace chat.Domain.Entities;

[Index(nameof(ID), IsUnique = true)]
[Index(nameof(Name), IsUnique = true)]
public class App<TKeyType> : IBaseEntity<TKeyType>
{
    [Required]
    public TKeyType ID { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
}