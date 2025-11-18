using chat.Domain.Entities;
using chat.Repo.Interfaces;

namespace chat.Repo.Implementation
{
    internal class GroupRepo : RepoBase<Group>, IGroupRepo
    {
        public GroupRepo(ChatContext context) : base(context)
        {
        }
    }
}
