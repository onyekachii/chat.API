using chat.Domain.Entities;
using chat.Repo.Interfaces;

namespace chat.Repo.Implementation
{
    internal class UserRepo : RepoBase<User>, IUserRepo
    {
        public UserRepo(ChatContext context) : base(context)
        {
        }
    
    }
}
