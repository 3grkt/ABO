using ABO.Core;
using ABO.Core.Infrastructure;
using ABO.Services.Users;
using ABO.Web.Extensions;
using ABO.Web.Framework;
using ABO.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ABO.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        #region Events
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Initialize engine context
            EngineContext.CreateEngineInstance = () => new WebEngine(); // engine object used in Web app
            EngineContext.GetDependencyRegistrars = () => new IDependencyRegistrar[]{ // dependency registrar
                new CoreDependencyRegistrar(),
                new DependencyRegistrar()
            };
            EngineContext.Initialize();

            // Object Mapping
            AutoMapperConfiguration.ConfigMapping();

            // FluentValidation
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            FluentValidation.Mvc.FluentValidationModelValidatorProvider.Configure(x => x.ValidatorFactory = new ModelValidatorFactory());

            // Custom Datetime format binding
            var binder = new DateTimeModelBinderExtention();
            ModelBinders.Binders.Add(typeof(DateTime), binder);
            ModelBinders.Binders.Add(typeof(DateTime?), binder);
        }

        //protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        //{
        //    Debug.WriteLine("User: " + HttpContext.Current.User.Identity.Name + "; Request: " + HttpContext.Current.Request.Path);
        //    SetAuthenticationInfo();
        //}

        protected void Application_Error(object sender, EventArgs e)
        {
            if (HttpContext.Current.IsDebuggingEnabled) // for debug
                return;

            var error = Server.GetLastError();
            if (error != null)
            {
                var httpException = error as HttpException;
                if (httpException != null && httpException.GetHttpCode() == (int)System.Net.HttpStatusCode.NotFound)
                    return;

                Server.ClearError();
                //Response.Redirect(UrlHelper.GenerateUrl("Default", "Error", "Error", null, null, HttpContext.Current.Request.RequestContext, true));
                EngineContext.Current.Resolve<ABO.Services.Logging.ILogger>().WriteLog("Application Error: " + error.Message, error, null);
                Response.RedirectToRoute(new { controller = "Error", action = "Index" });
            }
        }
        #endregion

        #region Utilities

        //private void SetAuthenticationInfo()
        //{
        //    var context = HttpContext.Current;
        //    if (context != null)
        //    {
        //        var userService = EngineContext.Current.Resolve<IUserService>();
        //        var userName = GetUserName(context.User.Identity.Name);

        //        if (HttpContext.Current.IsDebuggingEnabled) // for debug
        //        {
        //            // Username only contains 8 characters
        //            if (userName.Length > 8)
        //                userName = userName.Substring(0, 8);
        //        }

        //        //TODO: hard-code for testing
        //        //if (userName == "Tri Nguyen" || userName == "tnguyen258") userName = "VNM00000";

        //        //TODO: hard-code for Forms authentication
        //        //userName = "VNM00000";
        //        //System.Web.Security.FormsAuthentication.SetAuthCookie(userName, false);

        //        var user = userService.GetUserByUsername(userName);
        //        if (user != null)
        //        {
        //            // Overwrite current principle
        //            var roles = new string[] { user.Role.Name };
        //            var principal = new GenericPrincipal(context.User.Identity, roles);
        //            context.User = principal;

        //            // Set work context
        //            var workContext = EngineContext.Current.Resolve<IWorkContext>();
        //            workContext.User.UserID = user.Id;
        //            workContext.User.UserName = user.UserName;
        //            workContext.User.FullName = user.FullName;
        //        }
        //    }
        //}

        ///// <summary>
        ///// Gets username by removing domain if exists.
        ///// </summary>
        ///// <param name="fullUserName"></param>
        ///// <returns></returns>
        //private string GetUserName(string fullUserName)
        //{
        //    var backSlashIndex = fullUserName.IndexOf("\\");
        //    if (backSlashIndex >= 0 && backSlashIndex < fullUserName.Length - 1)
        //        return fullUserName.Substring(backSlashIndex + 1);
        //    return fullUserName;
        //}

        #endregion
    }
}