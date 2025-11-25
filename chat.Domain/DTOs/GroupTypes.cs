using chat.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace chat.Domain.DTOs
{
    public class GroupTypes
    {
        public record GroupResponseDTO(string Name, string Description, DateTimeOffset CreatedDate, string CreatedBy)
        {
            public static GroupResponseDTO mapGroupToDto(Group g) => new GroupResponseDTO(
                Name: g.Name,
                Description: g.Description,
                CreatedDate: g.CreatedDate.HasValue ? g.CreatedDate.Value : throw new Exception("Created Date not provided"),
                CreatedBy: g.CreatedBy ?? throw new Exception("CreatedBy is null in Group entity")
            );
        };

        public record GroupPostRequestDTO([MaxLength(50)] string Name, string Description)
        {
            public static Group mapDtoToGroup(GroupPostRequestDTO dto) => new Group
            {
                Name = dto.Name,
                Description = dto.Description
            };
        };

        public record JoinGroupRequestDTO(string UserName, long GroupID);
    }
}
