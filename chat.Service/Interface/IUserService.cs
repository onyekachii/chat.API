using chat.Domain.DTOs;
using chat.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.Service.Interface
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(UserDTO dto, long appId);

        Task<User?> GetUserAsync(string username, long appId);
    }
}
