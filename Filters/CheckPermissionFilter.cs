using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using MyFirstWebAPI.Models;

namespace MyFirstWebAPI.Filters
{
    public class CheckPermissionFilter : IAsyncAuthorizationFilter
    {
        private readonly ProductsContext _context;
        private readonly Permission _requiredPermission;

        public CheckPermissionFilter(ProductsContext context, Permission requiredPermission)
        {
            _context = context;
            _requiredPermission = requiredPermission;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var usernameClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (usernameClaim == null)
            {
                context.Result = new ObjectResult("Invalid token or you are not logged in.")
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
                return;
            }

            string username = usernameClaim.Value;

            // we dont have a field in the userpermission for the username, se we wanna ge the id of that returned user name and check for that id in the userpermission table
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                context.Result = new ObjectResult("Invalid token or you are not logged in.")
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
                return;
            }

            var hasPermission = await _context.Users
            .Include(u => u.UserPermissions) 
            .Where(u => u.Username == username)
            .SelectMany(u => u.UserPermissions)
            .AnyAsync(p => p.Permission == _requiredPermission);

            
            if (!hasPermission)
            {
                context.Result = new ObjectResult($"You do not have the {_requiredPermission} permission required for this action.")
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }
        }
    }
}

    