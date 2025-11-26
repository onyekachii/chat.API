using chat.Domain.DTOs;
using chat.Domain.Entities;
using chat.Repo;
using chat.Service.Interface;
using Microsoft.EntityFrameworkCore;

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

        public async Task<App?> GetAppAsync(long id, bool throwExpOnUserNotFound)
        {
            var app = await UOW.App.FindByCondition(app => app.ID == id && !app.SoftDeleted).SingleOrDefaultAsync();
           return app is null && throwExpOnUserNotFound ? throw new UnauthorizedAccessException("App does not exists") : app;

        }
    }
}
