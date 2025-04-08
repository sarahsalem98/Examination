using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Examination.PL.Attributes
{
    public class UserTypeAuthorizeAttribute: ActionFilterAttribute
    {
        public string UserType { get; set; }
        public UserTypeAuthorizeAttribute(string userType)
        {
            UserType = userType;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;
            var isAuthenticated = user.Identity?.IsAuthenticated ?? false;
            var userTypeClaim = user.FindFirst("UserType")?.Value;

            if (!isAuthenticated)
            {
               
                context.Result = new RedirectToActionResult("Login", "Account", new {area=""});
                return;
            }

            if (userTypeClaim != UserType)
            {
                
                context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
            }
        }
    }
    
}
