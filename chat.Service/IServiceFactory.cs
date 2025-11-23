using chat.Repo;
using chat.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.Service
{
    public interface IServiceFactory
    {
        IAuthService AuthService { get; }
        IUserService UserService { get; }
        IRepoFactory db { get; }

    }
}
