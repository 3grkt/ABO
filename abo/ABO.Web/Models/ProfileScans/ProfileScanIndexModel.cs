using ABO.Core;
using ABO.Web.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABO.Web.Models.ProfileScans
{
    public class ProfileScanIndexModel : ViewModelBase
    {
        public ProfileScanIndexModel()
        {
            Search = new ProfileScanSearchModel();
            Scans = new List<ProfileScanModel>();
        }

        public ProfileScanSearchModel Search { get; set; }
        public IList<ProfileScanModel> Scans { get; set; }
        public MvcContrib.Pagination.IPagination<ProfileScanModel> Pager { get; set; }

        public IEnumerable<SelectListItem> AvailableWarehouses { get; set; }
        public IEnumerable<SelectListItem> AvailableResults { get; set; }
    }

    public class ProfileScanSearchModel : ViewModelBase
    {
        [UIHint("DatePicker")]
        public DateTime StartDate { get; set; }
        [UIHint("DatePicker")]
        public DateTime EndDate { get; set; }
        [ResourceDisplayName("ProfileScan.Search.WarehouseId")]
        public string WarehouseId { get; set; }
        [ResourceDisplayName("ProfileScan.Search.Result")]
        public ProfileScanResult Result { get; set; }
    }
}