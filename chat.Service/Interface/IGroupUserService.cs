

using chat.Domain.Entities;

namespace chat.Service.Interface
{
    public interface IGroupUserService
    {
        Task CreateGroupUserAsync (Group g, User u);
    }
}
