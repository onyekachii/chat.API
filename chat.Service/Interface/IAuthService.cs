using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.Service.Interface
{
    public interface IAuthService
    {
        Task<string> GetApiKey(long appId);
        Task<bool> IsApiKeyValid(string apiKeyFromCLient, string apiKey);
    }
}
