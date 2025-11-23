using chat.Repo;
using chat.Service.Interface;

namespace chat.Service
{
    public class ServiceFactory : IServiceFactory
    {
        readonly IAuthService _authService;
        readonly IUserService _userService;
        readonly IRepoFactory _repoFactory;
        public ServiceFactory(IAuthService authService, IUserService userService, IRepoFactory repoFactory)
        {
            _authService = authService;
            _userService = userService;
            _repoFactory = repoFactory;
        }
        public IAuthService AuthService => _authService;

        public IUserService UserService => _userService;

        public IRepoFactory db => _repoFactory;
    }
}
