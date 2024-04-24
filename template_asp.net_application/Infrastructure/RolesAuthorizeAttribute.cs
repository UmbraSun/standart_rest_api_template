using Microsoft.AspNetCore.Mvc.Filters;
using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace template_asp.net_application.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    internal sealed class RolesAuthorizeAttribute : Attribute, IAsyncActionFilter
    {
        private string _headerName;
        private List<string> _roles;

        public RolesAuthorizeAttribute(params RoleType[] roles)
        {
            _headerName = JwtBuilder.Authorization();
            _roles = roles.Select(x => x.ToString()).ToList();
        }


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var (token, isBearer) = IsCorrect(context);
            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            if (isBearer)
            {
                var (isCorrectBearer, result) = IsCorrectBearer(configuration, token);
                 if(!isCorrectBearer)
                 {
                     context.Result = result;
                     return;
                 }

                await next();
            }

            context.Result = new UnauthorizedResult();
        }

        private (string Token, bool IsBearer) IsCorrect(ActionExecutingContext context)
        {
            if (context.ActionDescriptor is not ControllerActionDescriptor actionDescriptor)
                return (null, false);

            var isHasAuthorizeHeader = context.HttpContext.Request.Headers.TryGetValue(_headerName, out var headerKeys);

            if (!actionDescriptor.MethodInfo.IsDefined(typeof(AuthorizeAttribute), true) && !isHasAuthorizeHeader)
                return (null, false);

            var keys = headerKeys.FirstOrDefault()?.Split(" ");
            var last = keys?.Last();
            var isBearer = keys?.Length == 2 && keys.FirstOrDefault()?.ToLower() == JwtBuilder.Bearer() && !string.IsNullOrWhiteSpace(last);

            return (last, isBearer);
        }

        private (bool IsCorrect, IActionResult Result) IsCorrectBearer(IConfiguration configuration, string accessToken)
        {
            var token = new JsonWebTokenHandler().ValidateToken(accessToken, JwtBuilder.Parameters(configuration));
            var rolesNames = new HashSet<string>(token?.ClaimsIdentity?.Claims
                .Where(x => x.Type == ClaimTypes.Role)
                .Select(x => x.Value) ?? new List<string>());

            IActionResult result = rolesNames.Any() ? new ForbidResult() : new UnauthorizedResult();
            return (_roles.Any(x => rolesNames.Contains(x)), result);
        }
    }
}
