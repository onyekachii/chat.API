using System.ComponentModel.DataAnnotations;
namespace chat.Domain.Entities;

[Index(nameof(ID), IsUnique = true)]
public class AppUser<TKeyType> : IBaseEntity<TKeyType>
{
    [Required]
    public TKeyType ID { get; set; }

    [Required]
    public TKeyType UserId { get; set; }

    [Required]
    public TKeyType AppId { get; set; }
}