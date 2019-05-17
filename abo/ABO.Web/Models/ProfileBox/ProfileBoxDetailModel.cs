using ABO.Web.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABO.Web.Models.ProfileBox
{
    public class ProfileBoxDetailModel : ViewModelBase
    {
        public int Id { get; set; }
        [ResourceDisplayName("ProfileBox.Name")]
        public string Name { get; set; }
        [ResourceDisplayName("ProfileBox.CreatedDate")]
        public DateTime CreatedDate { get; set; }
        [ResourceDisplayName("ProfileBox.BoxStatus2")]
        public int Status { get; set; }
        public string StatusName { get; set; }
        [ResourceDisplayName("ProfileBox.OfficeName")]
        public string OfficeName { get; set; }
        [ResourceDisplayName("ProfileBox.WareHouse2")]
        public string Warehouse { get; set; }
        public string WarehouseName { get; set; }
        [ResourceDisplayName("ProfileBox.Location")]
        public int? Location { get; set; }
        public string LocationName { get; set; }
        [ResourceDisplayName("ProfileBox.UpdatedDate")]
        public DateTime UpdatedDate { get; set; }
        [ResourceDisplayName("ProfileBox.Creator")]
        public string Creator { get; set; }
        [ResourceDisplayName("ProfileBox.ProfileType")]
        public int ProfileType { get; set; }
        [ResourceDisplayName("ProfileBox.FolderPath")]
        public string FolderPath { get; set; }
        [ResourceDisplayName("ProfileBox.Office")]
        public string OfficeId { get; set; }
        public bool Scanned { get; set; }
        public string ProfileTypeName { get; set; }
        [ResourceDisplayName("ProfileBox.ProfileCount")]
        public int? ProfileCount { get; set; }
        public IEnumerable<SelectListItem> StatusList { get; set; }
        public IList<SelectListItem> WarehouseList { get; set; }
        public IList<SelectListItem> ProfileTypeList { get; set; }
    }
}