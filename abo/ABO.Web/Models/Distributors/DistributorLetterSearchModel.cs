using ABO.Web.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ABO.Web.Models.Distributors
{
    public class DistributorLetterSearchModel : ViewModelBase
    {
        [ResourceDisplayName("DistributorLetter.Warehouse")]
        public string WarehouseID { get; set; }
        public long? DistributorNumber { get; set; }
        [ResourceDisplayName("Common.StartDate")]
        [UIHint("DatePicker")]
        public DateTime StartDate { get; set; }
        [Display(Name = "EndDate")]
        [UIHint("DatePicker")]
        public DateTime EndDate { get; set; }
    }
}