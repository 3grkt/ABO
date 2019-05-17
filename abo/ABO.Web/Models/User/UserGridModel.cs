using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABO.Web.Models.User
{
    public class UserGridModel : ViewModelBase
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public string WarehouseName { get; set; }
        public string Role { get; set; }
    }
}