using chat.Domain.Entities;
using chat.Repo;
using chat.Service.Interface;

namespace chat.Service.Implementation
{
    public class GroupUserService : IGroupUserService
    {
        IUnitOfWork UOW;

        public GroupUserService(IUnitOfWork repoFactory)
        {
            UOW = repoFactory;
        }

        public async Task CreateGroupUserAsync(Group g, User u) 
        {
            var groupUser = await UOW.Group.GetGroupWithUserAsync(g.ID, u.Username);
            if(groupUser is null)
                throw new Exception("Group does not exist");
            if (groupUser.Users!.Any())            
                throw new Exception("User already exists in the group");
            
            g!.Users!.Add(u!);
            UOW.Group.Update(g);
        }      
    }
}
