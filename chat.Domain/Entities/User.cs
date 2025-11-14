using System.ComponentModel.DataAnnotations;
namespace chat.Domain.Entities;

[Index(nameof(ID), IsUnique = true)]
public class User<TKeyType> : IBaseEntity<TKeyType>
{
    [Required]
    public TKeyType ID { get; set; }

    [Required]
    [MaxLength(50)]
    public string Username { get; set; }
}