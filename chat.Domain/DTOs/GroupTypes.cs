using chat.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace chat.Domain.DTOs
{
    public class GroupTypes
    {
        public record GroupResponseDTO(string Name, string DisplayName, string Description, string? Aggregator, DateTime CreatedDate, string CreatedBy)
        {
            public static GroupResponseDTO mapGroupToDto(Group g) => new GroupResponseDTO(
                Name: g.Name,
                Description: g.Description,
                DisplayName: g.DisplayName ?? string.Empty,
                Aggregator: g.Aggregator,
                CreatedDate: g.CreatedDate.HasValue ? g.CreatedDate.Value : throw new Exception("Created Date not provided"),
                CreatedBy: g.CreatedBy ?? throw new Exception("CreatedBy is null in Group entity")
            );
        };

        public record GroupPostRequestDTO([MaxLength(50)] string Name, string DisplayName, 
            string? Description, [Required] string MethodIdentifier, string? Aggregator)
        {
            public static Group mapDtoToGroup(GroupPostRequestDTO dto) => new Group
            {
                Name = dto.Name,
                Description = dto.Description ?? string.Empty,
                DisplayName = dto.DisplayName,
                Aggregator = dto.Aggregator ?? string.Empty
            };
        };

        public record JoinGroupRequestDTO(string UserName, long GroupID);
    }
}
