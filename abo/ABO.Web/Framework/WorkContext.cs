using ABO.Core;
using ABO.Core.Infrastructure;
using ABO.Services.Users;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ABO.Web.Framework
{
    public class WorkContext : ABO.Core.IWorkContext
    {
        #region Ctor.
        public WorkContext()
        {
            WorkingCulture = new CultureInfo("en-US"); // default culture
            //User = new WorkingUser();
        }
        #endregion

        #region Properties
        public CultureInfo WorkingCulture { get; set; }

        public WorkingUser User
        {
            get
            {
                var obj = HttpContext.Current.Session["WorkContext.User"] as WorkingUser;
                if (obj == null)
                {
                    obj = GetWorkingUser();
                    HttpContext.Current.Session["WorkContext.User"] = obj;
                }
                return obj;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                HttpContext.Current.Session["WorkContext.User"] = value;
            }
        }

        public Dictionary<string, object> SessionData
        {
            get
            {
                var obj = HttpContext.Current.Session["WorkContext.SessionData"] as Dictionary<string, object>;
                if (obj == null)
                {
                    obj = new Dictionary<string, object>();
                    HttpContext.Current.Session["WorkContext.SessionData"] = obj;
                }
                return obj;
            }
        }
        #endregion

        #region Utilities
        /// <summary>
        /// Gets username by removing domain if exists.
        /// </summary>
        /// <param name="fullUserName"></param>
        /// <returns></returns>
        private string GetUserName(string fullUserName)
        {
            var userName = fullUserName;
            var backSlashIndex = userName.IndexOf("\\");
            if (backSlashIndex >= 0 && backSlashIndex < userName.Length - 1)
                return userName.Substring(backSlashIndex + 1);
            return userName;
        }

        private WorkingUser GetWorkingUser()
        {
            var workingUser = new WorkingUser();
            var userName = GetUserName(HttpContext.Current.User.Identity.Name);

            if (HttpContext.Current.IsDebuggingEnabled) // for debug
            {
                var appSettings = EngineContext.Current.Resolve<IAppSettings>();
                // For test users, use VNM00000 by default
                if (appSettings.TestUsers.Contains(userName, StringComparer.OrdinalIgnoreCase))
                    userName = appSettings.TestUserReplacement;
            }

            var user = EngineContext.Current.Resolve<IUserService>().GetUserByUsername(userName);
            if (user != null)
            {
                workingUser.UserID = user.Id;
                workingUser.FullName = user.FullName;
                workingUser.UserName = user.UserName;
                workingUser.Roles = new UserRole[] { (UserRole)user.RoleId };
                workingUser.Permissions = user.Role.Permissions.Select(x => (UserPermission)x.Id).ToArray();
                workingUser.WarehouseName = (user.Warehouse != null) ? user.Warehouse.WarehouseName : null;
                workingUser.WarehouseId = (user.Warehouse != null) ? user.Warehouse.WarehouseId : null;
            }

            return workingUser;
        }
        #endregion
    }

    ///// <summary>
    ///// Represent current working user.
    ///// </summary>
    //public class WorkingUser
    //{
    //    public WorkingUser()
    //    {
    //        Roles = new UserRole[] { };
    //    }

    //    public int UserID { get; set; }
    //    public string UserName { get; set; }
    //    public string FullName { get; set; }
    //    public UserRole[] Roles { get; set; }
    //}
}