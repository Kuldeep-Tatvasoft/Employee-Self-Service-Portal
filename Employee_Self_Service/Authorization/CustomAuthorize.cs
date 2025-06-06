using System.Security.Claims;
using Employee_Self_Service_BAL.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Employee_Self_Service.Authorization;

public class CustomAuthorize : Attribute, IAuthorizationFilter
    {
        private readonly string[] _roles;
        
        public CustomAuthorize(params string[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {

            
            var jwtService = context.HttpContext.RequestServices.GetService(typeof(IJwtService)) as IJwtService;

            
            context.HttpContext.Request.Cookies.TryGetValue("token" ,out String? token);

            
            var principal = jwtService?.ValidateToken(token ?? "");


            if (principal == null)
            {
                context.Result = new RedirectToActionResult("Login", "Index", null);
                
                context.HttpContext.Response.Cookies.Delete("token");
                return;
            }

            context.HttpContext.User = principal;

            if (_roles.Length > 0)
            {   
                var userRole = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "";
                
                if (!_roles.Contains(userRole))
                {

                    context.Result = new RedirectToActionResult("LoginRedirect", "Login", new { message = " Access Denied: You do not have permission of this Page." });
                }
            }
        }
    }