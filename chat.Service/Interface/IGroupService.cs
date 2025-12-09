using chat.Domain.DTOs;
using chat.Domain.Entities;
using static chat.Domain.DTOs.GroupTypes;

namespace chat.Service.Interface
{
    public interface IGroupService
    {
        Task<Group> CreateGroupAsync(GroupPostRequestDTO dto, DTO baseDTO);
        Task<Group?> GetGroupAsync(long Id, long appId, bool throwExpOnUserNotFound);
        Task<Group?> GetGroupByNameAsync(string name, long appId, bool throwExpOnUserNotFound);
        Task<IList<Group>?> GetAllGroups(DateTime? last, int pageSize, int page, long appID);
    }
}
