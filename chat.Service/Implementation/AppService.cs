using chat.Domain.DTOs;
using chat.Domain.Entities;
using chat.Repo;
using chat.Service.Interface;

namespace chat.Service.Implementation
{
    public class AppService : IAppService
    {
        readonly IRepoFactory _repoFactory;
        public AppService(IRepoFactory rf)
        {
            _repoFactory = rf;
        }
        public async Task<App> CreateAppAsync(AppDTO dto, string createdBy)
        {
            var app = AppDTO.mapDtoToApp(dto);
            app.CreatedBy = createdBy;
            app.CreatedDate = DateTimeOffset.UtcNow;

            return (await _repoFactory.App.CreateAsync(app)).Entity;
        }
    }
}
