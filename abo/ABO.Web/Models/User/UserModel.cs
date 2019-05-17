
using ABO.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABO.Web.Models.User
{
    public class UserModel:ViewModelBase
    {
        public int UserId { get; set; }
        [ResourceDisplayName("User.Name")]
        public string UserName { get; set; }
        [ResourceDisplayName("User.FullName")]
        public string FullName { get; set; }
        [ResourceDisplayName("User.Warehouse")]
        public string WarehouseId { get; set; }
        [ResourceDisplayName("User.Role")]
        public int RoleId { get; set; }
        
        public IList<SelectListItem> WarehouseList { get; set; }
        public IList<SelectListItem> RoleList { get; set; }

        public bool IsEditMode { get; set; }
    }
}