using ABO.Core;
using ABO.Core.Infrastructure;
using ABO.Services.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABO.Web.Framework
{
    /// <summary>
    /// Authorizes user based on permission.
    /// </summary>
    public class PermissionAuthorize : AuthorizeAttribute
    {
        private readonly UserPermission[] _permissions;

        public PermissionAuthorize(params UserPermission[] permissions)
        {
            _permissions = permissions;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (_permissions == null || _permissions.Length == 0)
                return true;

            var permissionService = EngineContext.Current.Resolve<IPermissionService>();
            return permissionService.Authorize(_permissions);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new ForbiddenResult();
        }
    }
}