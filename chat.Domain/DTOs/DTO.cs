
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

    public record AppDTO([Required][MaxLength(50)] string Name,
        long CreatedBy, long UpdatedBy, long DeletedBy)
    {
        public static App mapDtoToApp(AppDTO dto) => new App
        {
            Name = dto.Name
        };
    };
    
    public record UserDTO([Required][MaxLength(50)] string Username, string DisplayName,
        long CreatedBy, long UpdatedBy, long DeletedBy)
    {
        public static User mapDtoToUser(UserDTO dto, long AppID) => new User
        {
            Username = dto.Username,
            DisplayName = dto.DisplayName,
            AppId = AppID            
        };
        public static UserDTO mapUserToDto(User u, long AppID) => new UserDTO(
            Username: u.Username,
            DisplayName: u.DisplayName, 0,0,0   
        );
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

    public record ApiKeyDTO([Required][MaxLength(100)] string Key, [Required] long AppId)
    {
        public static ApiKey mapDtoToApiKey(ApiKeyDTO dto) => new ApiKey
        {
            Key = dto.Key,
            AppID = dto.AppId
        };
    };

    public record RefreshTokenDTO(string Token, string Username )
    {        
        public static RefreshTokenDTO mapRefreshTokenToDto(RefreshToken m) => new RefreshTokenDTO
        (
            Token: m.Token,
            Username: m.UserName
        );
    };

    public record RefreshRequestDTO(string Token, string Username, byte Role);

    public record ExternalAuthRequestDTO(string username, byte role);
}

