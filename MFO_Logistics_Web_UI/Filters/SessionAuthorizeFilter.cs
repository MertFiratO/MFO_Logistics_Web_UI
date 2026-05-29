using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MFO_Logistics_Web_UI.Filters
{
    public class SessionAuthorizeFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.RouteData.Values["controller"]?.ToString();
 
            if (controller == "Auth")
                return;

            var token = context.HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
