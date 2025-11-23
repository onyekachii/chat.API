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
            await _repoFactory.User.CreateAsync(user);
            return user;
        }

        public async Task<User?> GetUserAsync(string username, long appId)
        {
            return await _repoFactory.User.FindByCondition(u => string.Equals(u.Username, username) &&
                u.AppId == appId && !u.SoftDeleted).SingleOrDefaultAsync() ?? throw new UnauthorizedAccessException("User does not exists");
        }
    }
}
