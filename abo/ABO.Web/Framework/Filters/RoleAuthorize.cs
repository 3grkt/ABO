using ABO.Core;
using ABO.Core.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABO.Web.Framework
{
    /// <summary>
    /// Authorizes user based on role.
    /// </summary>
    public class RoleAuthorize : AuthorizeAttribute
    {
        private readonly UserRole[] _userRoles;

        public RoleAuthorize(params UserRole[] userRoles)
        {
            this._userRoles = userRoles;
        }

        //public override void OnAuthorization(AuthorizationContext filterContext)
        //{
        //    // set roles
        //    HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(
        //        HttpContext.Current.User.Identity,
        //        EngineContext.Current.Resolve<IWorkContext>().User.Roles.Select(x => x.ToString()).ToArray());

        //    // then call base authorization method
        //    base.OnAuthorization(filterContext);
        //}

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (_userRoles == null || _userRoles.Length == 0)
                return true;

            return EngineContext.Current.Resolve<IWorkContext>().User.Roles.Any(r => _userRoles.Contains(r));
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new ForbiddenResult();
        }
    }
}