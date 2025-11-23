using chat.Repo;
using chat.Service.Interface;

namespace chat.Service
{
    public class ServiceFactory : IServiceFactory
    {
        readonly IAuthService _authService;
        readonly IUserService _userService;
        readonly IRepoFactory _repoFactory;
        readonly IAppService _appService;
        public ServiceFactory(IAuthService authService, IUserService userService, IRepoFactory repoFactory,
            IAppService appService)
        {
            _authService = authService;
            _userService = userService;
            _repoFactory = repoFactory;
            _appService = appService;
        }
        public IAuthService AuthService => _authService;

        public IUserService UserService => _userService;

        public IRepoFactory db => _repoFactory;

        public IAppService AppService => _appService;
    }
}
