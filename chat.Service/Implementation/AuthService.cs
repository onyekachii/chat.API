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
    internal class AuthService : IAuthService
    {
        private RepoFactory _db;
        public AuthService(RepoFactory db)
        {
            _db = db;
        }
        public async Task<string> GetApiKey(long appId)
        {
            var key = await _db.ApiKey.FindByCondition(a => a.AppID == appId 
            && a.SoftDeleted == false && !a.Revoked).SingleOrDefaultAsync();
            if (key == null) 
                throw new Exception( "Api key not found for the given App ID." );
            return key.Key;
        }
        public async Task<bool> IsApiKeyValid(string apiKeyFromCLient, string apiKey)
            => string.Equals(apiKeyFromCLient, apiKey);
        
    }
}
