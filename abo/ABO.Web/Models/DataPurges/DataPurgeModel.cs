using ABO.Web.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ABO.Web.Models.DataPurges
{
    public class DataPurgeModel : ViewModelBase
    {
        public int Id { get; set; }
        [ResourceDisplayName("DataPurge.Field.PurgeDate")]
        [UIHint("DatePicker")]
        public DateTime PurgeDate { get; set; }
        [ResourceDisplayName("DataPurge.Field.StartDate")]
        [UIHint("DatePicker")]
        public DateTime StartDate { get; set; }
        [ResourceDisplayName("DataPurge.Field.EndDate")]
        [UIHint("DatePicker")]
        public DateTime EndDate { get; set; }
        public string UserName { get; set; }
        public int? FileCount { get; set; }
        public string PurgeLog { get; set; }
    }
}