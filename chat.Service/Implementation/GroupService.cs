using chat.Domain.DTOs;
using chat.Domain.Entities;
using chat.Repo;
using chat.Service.Interface;
using static chat.Domain.DTOs.GroupTypes;

namespace chat.Service.Implementation
{
    public class GroupService : IGroupService
    {
        IRepoFactory _repoFactory;

        public GroupService(IRepoFactory repoFactory)
        {
            _repoFactory = repoFactory;
        }
        public async Task<Group> CreateGroupAsync(GroupPostRequestDTO dto, DTO baseDto)
        {
            var group = GroupPostRequestDTO.mapDtoToGroup(dto);
            group.CreatedDate = DateTimeOffset.UtcNow;
            group.CreatedBy = baseDto.CreatedBy;
            group.AppId = baseDto.AppID;
            return (await _repoFactory.Group.CreateAsync(group)).Entity;
        }
    }
}
