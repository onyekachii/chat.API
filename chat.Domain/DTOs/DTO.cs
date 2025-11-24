
using chat.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chat.Domain.DTOs
{
    
    public record DTO(string CreatedBy, long AppID);

    public record AppDTO([Required][MaxLength(50)] string Name)
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
            Text = dto.Text
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

