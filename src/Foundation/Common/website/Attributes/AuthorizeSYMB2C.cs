using System.Web.Mvc;

namespace SYMB2C.Foundation.Common.Attributes
{
    public class AuthorizeNesAttribute : AuthorizeAttribute
    {
        public string SignInUrl { get; set; }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (string.IsNullOrWhiteSpace(SignInUrl))
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
            else
            {
                filterContext.Result = new RedirectResult(SignInUrl);
            }
        }
    }
}