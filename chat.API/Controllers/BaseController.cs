using chat.Domain.DTOs;
using chat.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace chat.API.Controllers
{
    public class BaseController : ControllerBase, IActionFilter
    {
        protected DTO? UserInfo;
                
        void IActionFilter.OnActionExecuted(ActionExecutedContext context)
        {
           
        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        {

            long.TryParse(User.FindFirst("AppId")?.Value, out var appId);
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException();


            UserInfo = new DTO(
                CreatedBy: username,
                AppID: appId
            );
        }
    }
}
