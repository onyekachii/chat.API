using chat.Domain.DTOs;
using chat.Domain.Entities;
using chat.Repo;
using chat.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace chat.Service.Implementation
{
    public class UserService : IUserService
    {
        IUnitOfWork UOW;
        public UserService(IUnitOfWork rf)
        {
            UOW = rf;
        }

        public async Task<User> CreateUserAsync(UserDTO dto, long appId)
        {
            var user = UserDTO.mapDtoToUser(dto, appId);
            user.CreatedDate = DateTimeOffset.UtcNow;
            return (await UOW.User.CreateAsync(user)).Entity;
        }

        public async Task<User?> GetUserAsync(string username, long appId, bool throwExpOnUserNotFound)
        {
            var user = await UOW.User.FindByCondition(u => string.Equals(u.Username, username) &&
                u.AppId == appId && !u.SoftDeleted).SingleOrDefaultAsync();
            return user is null && throwExpOnUserNotFound ? throw new UnauthorizedAccessException("User does not exists") : user;
        }
    }
}
