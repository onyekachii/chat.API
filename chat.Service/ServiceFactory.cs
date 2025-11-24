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
        readonly IGroupService _groupService;
        public ServiceFactory(IAuthService authService, IUserService userService, IRepoFactory repoFactory,
            IAppService appService, IGroupService groupService)
        {
            _authService = authService;
            _userService = userService;
            _repoFactory = repoFactory;
            _appService = appService;
            _groupService = groupService;
        }
        public IAuthService AuthService => _authService;

        public IUserService UserService => _userService;

        public IRepoFactory db => _repoFactory;

        public IAppService AppService => _appService;

        public IGroupService GroupService => _groupService;
    }
}
