using chat.Domain.DTOs;
using chat.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.Service.Interface
{
    public interface IAppService
    {
        Task<App> CreateAppAsync(AppDTO dto);
    }
}
