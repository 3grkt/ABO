using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABO.Core
{
    /// <summary>
    /// Represent current working user.
    /// </summary>
    public class WorkingUser
    {
        public WorkingUser()
        {
            Roles = new UserRole[] { };
            Permissions = new UserPermission[] { };
        }

        public int UserID { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }

        public UserRole[] Roles { get; set; }
        public UserPermission[] Permissions { get; set; }

        /// <summary>
        /// Gets string used for logging purpose (including user id and username).
        /// </summary>
        /// <returns></returns>
        public string GetLogString()
        {
            return string.Format("{0} - {1}", UserID, UserName);
        }
    }
}
