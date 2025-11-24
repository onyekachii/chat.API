using chat.Domain.DTOs;
using chat.Domain.Entities;

namespace chat.Service.Interface
{
    public interface IAppService
    {
        Task<App> CreateAppAsync(AppDTO dto, string createdBy);
    }
}
