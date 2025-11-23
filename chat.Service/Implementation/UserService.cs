using chat.Domain.DTOs;
using chat.Domain.Entities;
using chat.Repo;
using chat.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.Service.Implementation
{
    public class UserService : IUserService
    {
        IRepoFactory _repoFactory;
        public UserService(IRepoFactory rf)
        {
            _repoFactory = rf;
        }

        public async Task<User> CreateUserAsync(UserDTO dto, long appId)
        {
            var user = UserDTO.mapDtoToUser(dto, appId);
            user.CreatedDate = DateTimeOffset.UtcNow;
            return (await _repoFactory.User.CreateAsync(user)).Entity;
        }

        public async Task<User?> GetUserAsync(string username, long appId, bool throwExpOnUserNotFound)
        {
            var user = await _repoFactory.User.FindByCondition(u => string.Equals(u.Username, username) &&
                u.AppId == appId && !u.SoftDeleted).SingleOrDefaultAsync();
            return user is null && throwExpOnUserNotFound ? throw new UnauthorizedAccessException("User does not exists") : user;
        }
    }
}
