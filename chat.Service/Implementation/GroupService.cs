using chat.Domain.DTOs;
using chat.Domain.Entities;
using chat.Repo;
using chat.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static chat.Domain.DTOs.GroupTypes;

namespace chat.Service.Implementation
{
    public class GroupService : IGroupService
    {
        IUnitOfWork UOW;

        public GroupService(IUnitOfWork repoFactory)
        {
            UOW = repoFactory;
        }
        public async Task<Group> CreateGroupAsync(GroupPostRequestDTO dto, DTO baseDto)
        {
            var group = GroupPostRequestDTO.mapDtoToGroup(dto);
            group.CreatedDate = DateTime.UtcNow;
            group.DisplayName = dto.DisplayName;
            group.CreatedBy = baseDto.CreatedBy;
            group.AppId = baseDto.AppID;
            return (await UOW.Group.CreateAsync(group)).Entity;
        }

        public async Task<IList<Group>?> GetAllGroups(DateTime? last, int pageSize, int page, long appID)
        {
            var groups = await UOW.Group.FindByCondition(g => !g.SoftDeleted && (last == null || g.CreatedDate > last) && appID == g.AppId)
                .OrderBy(g => g.CreatedDate).Skip(pageSize * (page - 1)).Take(pageSize).ToListAsync();

            return groups;
        }

        public async Task<Group?> GetGroupAsync(long Id, long appId, bool throwExpOnUserNotFound)
        {
            var group = UOW.Group.FindByCondition(g => g.ID == Id && g.AppId == appId && !g.SoftDeleted).FirstOrDefault();
            return group is null && throwExpOnUserNotFound ? throw new Exception("Group does not exists") : group;
        }

        public async Task<Group?> GetGroupByNameAsync(string name, long appId, bool throwExpOnUserNotFound)
        {
            var group = UOW.Group.FindByCondition(g => g.Name == name && g.AppId == appId && !g.SoftDeleted).FirstOrDefault();
            return group is null && throwExpOnUserNotFound ? throw new Exception("Group does not exists") : group;
        }
    }
}
