using ABO.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABO.Web.Models.User
{
    public class UserSearchModel:ViewModelBase
    {
        [ResourceDisplayName("User.Warehouse")]
        public string WarehouseId { get; set; }
        [ResourceDisplayName("User.Name")]
        public int? UserId { get; set; }
        public IList<SelectListItem> WarehouseList { get; set; }
        public IList<SelectListItem> Users { get; set; }
    }
}