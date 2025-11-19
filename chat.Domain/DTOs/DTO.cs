
using chat.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chat.Domain.DTOs
{
    public record GroupDTO([MaxLength(50)] string Name,
        [Required] long AppId, string Description, long CreatedBy,
        long UpdatedBy, long DeletedBy)
    {
        public static Group mapDtoToGroup(GroupDTO dto) => new Group
        {
            Name = dto.Name,
            AppId = dto.AppId,
            Description = dto.Description,
            CreatedBy = dto.CreatedBy,
            UpdatedBy = dto.UpdatedBy,
            DeletedBy = dto.DeletedBy
        };
    };

    public record AppDTO([Required] [MaxLength(50)] string Name,
        long CreatedBy, long UpdatedBy, long DeletedBy)
    {
        public static App mapDtoToApp(AppDTO dto) => new App
        {
            Name = dto.Name,
            CreatedBy = dto.CreatedBy,
            UpdatedBy = dto.UpdatedBy,
            DeletedBy = dto.DeletedBy
        };
    };

    public record UserDTO([Required][MaxLength(50)] string Username, [Required] long AppId,
        long CreatedBy, long UpdatedBy, long DeletedBy)
    {
        public static User mapDtoToUser(UserDTO dto) => new User
        {
            Username = dto.Username,
            AppId = dto.AppId,
            CreatedBy = dto.CreatedBy,
            UpdatedBy = dto.UpdatedBy,
            DeletedBy = dto.DeletedBy
        };
    };

    public record MessageDTO(long? GroupId, [MaxLength(2000)] string Text,
        long CreatedBy, long UpdatedBy, long DeletedBy)
    {
        public static Message mapDtoToMessage(MessageDTO dto) => new Message
        {
            GroupId = dto.GroupId,
            Text = dto.Text,
            CreatedBy = dto.CreatedBy,
            UpdatedBy = dto.UpdatedBy,
            DeletedBy = dto.DeletedBy
        };
    };
}

