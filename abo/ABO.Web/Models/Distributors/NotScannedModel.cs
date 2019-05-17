using ABO.Web.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABO.Web.Models.Distributors
{
    public class NotScannedModel : ViewModelBase
    {
        public NotScannedModel()
        {
            //DistributorUpdates = new List<DistributorUpdateModel>();
            Search = new DistributorUpdateSearchModel();
            AllDistributorUpdateTypes = new List<SelectListItem>();
            AllWarehouses = new List<SelectListItem>();
        }

        public DistributorUpdateSearchModel Search { get; set; }
        public IList<SelectListItem> AllDistributorUpdateTypes { get; set; }
        public IList<SelectListItem> AllWarehouses { get; set; }
        public IList<DistributorUpdateModel> DistributorUpdates { get; set; }
        public MvcContrib.Pagination.IPagination<DistributorUpdateModel> Pager { get; set; }

        public class DistributorUpdateSearchModel : ViewModelBase
        {
            [ResourceDisplayName("Distributor.NotScanned.StartDate")]
            [UIHint("DatePicker")]
            public DateTime StartDate { get; set; }
            [ResourceDisplayName("Distributor.NotScanned.EndDate")]
            [UIHint("DatePicker")]
            public DateTime EndDate { get; set; }
            [ResourceDisplayName("Distributor.NotScanned.WarehouseId")]
            public string WarehouseId { get; set; }
            [ResourceDisplayName("Distributor.NotScanned.UpdateType")]
            public string UpdateType { get; set; }
        }

        public class DistributorUpdateModel : ViewModelBase
        {
            public long DistNumber { get; set; }
            public string DistName { get; set; }
            public DateTime? JoinDate { get; set; }
            public DateTime? ExpiryDate { get; set; }
            public string UpdatedType { get; set; }
        }
    }
}