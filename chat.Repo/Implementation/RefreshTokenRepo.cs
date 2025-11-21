using chat.Domain.Entities;
using chat.Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.Repo.Implementation
{
    internal class RefreshTokenRepo : RepoBase<RefreshToken>, IRefreshTokenRepo
    {
        public RefreshTokenRepo(ChatContext context) : base(context)
        {
            
        }
    }
}
