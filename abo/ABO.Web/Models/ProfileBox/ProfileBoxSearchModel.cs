using ABO.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABO.Web.Models.ProfileBox
{
    public class ProfileBoxSearchModel : ViewModelBase
    {
        [ResourceDisplayName("ProfileBox.ProfileBoxName")]
        public string ProfileBoxName { get; set; }
        [ResourceDisplayName("ProfileBox.UserID")]
        public int? UserID { get; set; }
        [ResourceDisplayName("ProfileBox.WareHouse")]
        public string WarehouseID { get; set; }
        [ResourceDisplayName("ProfileBox.BoxStatus")]
        public int? StatusID { get; set; }
        [ResourceDisplayName("ProfileBox.ProfileType")]
        public int? ProfileTypeID { get; set; }
    }
}