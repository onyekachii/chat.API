using chat.Domain.DTOs;
using chat.Domain.Entities;
using chat.Repo;
using chat.Service.Interface;

namespace chat.Service.Implementation
{
    public class AppService : IAppService
    {
        readonly IUnitOfWork UOW;
        public AppService(IUnitOfWork rf)
        {
            UOW = rf;
        }
        public async Task<App> CreateAppAsync(AppDTO dto, string createdBy)
        {
            var app = AppDTO.mapDtoToApp(dto);
            app.CreatedBy = createdBy;
            app.CreatedDate = DateTimeOffset.UtcNow;

            return (await UOW.App.CreateAsync(app)).Entity;
        }
    }
}
