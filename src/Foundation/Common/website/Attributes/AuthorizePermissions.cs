using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SYMB2C.Foundation.Common.Enum;

namespace SYMB2C.Foundation.Common.Attributes
{
    /// <summary>
    /// Authorize Permission for Nes accounts.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class AuthorizePermissionsAttribute : FilterAttribute, IAuthorizationFilter
    {
        public string RedirectUrl { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string ViewName { get; set; }

        public string Domains
        {
            get => this._domains ?? string.Empty;
            set
            {
                this._domains = value;
                this._domainsSplit = SplitString(value);
            }
        }

        public string Roles
        {
            get => this._roles ?? string.Empty;
            set
            {
                this._roles = value;
                this._rolesSplit = SplitString(value);
            }
        }

        public object JsonData { get; set; }

        [Required]
        public ActionResultEnum ActionResultEnum { get; set; }

        private static readonly char[] _splitParameter = { ',' };
        private string[] _rolesSplit = new string[0];
        private string _roles;
        private string[] _domainsSplit = new string[0];
        private string _domains;

        protected virtual bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = Sitecore.Context.User;

            if (!user.Identity.IsAuthenticated || (this._rolesSplit.Length > 0 && !this._rolesSplit.Any<string>(user.IsInRole)) || (this._domainsSplit.Any() && !this._domainsSplit.Contains(user.Domain.Name.ToLower())))
            {
                return false;
            }

            return true;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!this.AuthorizeCore(filterContext.HttpContext))
            {
                switch (this.ActionResultEnum)
                {
                    case ActionResultEnum.StatusCode:
                        filterContext.Result = new HttpStatusCodeResult(403);
                        break;
                    case ActionResultEnum.Json:
                        filterContext.Result = new ContentResult { Content = this.JsonData.ToString(), ContentType = "application/json" };
                        break;
                    case ActionResultEnum.Redirection:
                        filterContext.Result = new RedirectResult(this.RedirectUrl);
                        break;
                    case ActionResultEnum.ToAction:
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { this.Controller, this.Action }));
                        break;
                    case ActionResultEnum.View:
                        filterContext.Result = new ViewResult { ViewName = this.ViewName };
                        break;
                    default:
                        filterContext.Result = new HttpStatusCodeResult(403);
                        break;
                }
            }
        }

        internal static string[] SplitString(string original)
        {
            if (string.IsNullOrEmpty(original))
            {
                return new string[0];
            }

            return (from piece in original.Split(_splitParameter)
                let trimmed = piece.Trim()
                where !string.IsNullOrEmpty(trimmed)
                select trimmed).ToArray<string>();
        }
    }
}