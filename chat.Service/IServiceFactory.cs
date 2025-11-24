using chat.Repo;
using chat.Service.Interface;

namespace chat.Service
{
    public interface IServiceFactory
    {
        IAuthService AuthService { get; }
        IUserService UserService { get; }
        IAppService AppService { get; }
        IGroupService GroupService { get; }
        IRepoFactory db { get; }

    }
}
