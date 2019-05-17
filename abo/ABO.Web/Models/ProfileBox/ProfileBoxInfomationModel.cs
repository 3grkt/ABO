using ABO.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABO.Web.Models.ProfileBox
{
    public class ProfileBoxInfomationModel : ViewModelBase
    {
        [ResourceDisplayName("ProfileBox.Name")]
        public string Name { get; set; }
        [ResourceDisplayName("ProfileBox.UserID")]
        public string UserName { get; set; }
        [ResourceDisplayName("Profile.ProfileBox.WarehouseName")]
        public string WarehouseId { get; set; }
        [ResourceDisplayName("ProfileBox.ProfileType")]
        public int ProfileType { get; set; }
        [ResourceDisplayName("ProfileBox.FolderPath")]
        public string FolderPath { get; set; }
        public string WarehouseName { get; set; }
        public IList<SelectListItem> ProfileTypeList { get; set; }
        //public IList<SelectListItem> WarehousesList { get; set; }
        public int CurrentBoxCount { get; set; }
        public int CurrentWHYear { get; set; }
    }
}