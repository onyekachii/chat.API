using chat.Repo.Implementation;
using chat.Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.Repo
{
    public interface IRepoFactory
    {
        IAppRepo App { get; }
        IUserRepo User { get; }
        IGroupRepo Group { get; }
        IMessageRepo Message { get; }
        IRefreshTokenRepo RefreshToken {  get; }
        Task SaveAsync();
    }
}
