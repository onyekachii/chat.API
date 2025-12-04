using chat.Domain.Entities;
using chat.Repo.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace chat.Repo.Implementation
{
    internal class GroupRepo : RepoBase<Group>, IGroupRepo
    {
        readonly ChatContext _context;
        public GroupRepo(ChatContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Group?> GetGroupWithUserAsync(long groupId, string userName)
        {
            var groupUser = await _context.Groups.Where(g => g.ID == groupId)
                .Select(g => new
                {
                    Group = g,
                    User = g.Users.SingleOrDefault(u => u.Username == userName)
                }).SingleOrDefaultAsync();

            groupUser?.Group?.Users?.Add(groupUser?.User);

            return groupUser?.Group;
        }               
    }
}
