
using chat.Domain.Entities;

namespace chat.Repo.Interfaces
{
    public interface IGroupRepo : IRepoBase<Group>
    {
        Task<Group> GetGroupWithUserAsync(long groupId, string userName);
    }
}
