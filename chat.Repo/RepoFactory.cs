using chat.Repo.Implementation;
using chat.Repo.Interfaces;

namespace chat.Repo
{
    public class RepoFactory : IRepoFactory
    {
        private ChatContext _chatContext;
        private IAppRepo _appRepo;
        private IUserRepo _userRepo;
        private IGroupRepo _groupRepo;
        private IMessageRepo _messageRepo;
        public RepoFactory(ChatContext chatContext)
        {
            _chatContext = chatContext;
        }

        public IAppRepo App {
            get
            {
                return _appRepo ??= new AppRepo(_chatContext);                
            }
        }

        public IUserRepo User {
            get
            {
                return _userRepo ??= new UserRepo(_chatContext);
            }
        }

        public IGroupRepo Group {
            get
            {
                return _groupRepo ??= new GroupRepo(_chatContext);
            }
        }

        public IMessageRepo Message
        {
            get
            {
                return _messageRepo ??= new MessageRepo(_chatContext);
            }
        }

        public Task SaveAsync() => _chatContext.SaveChangesAsync();
    }
}
