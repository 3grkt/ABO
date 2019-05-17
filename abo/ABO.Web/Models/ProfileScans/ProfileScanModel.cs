using ABO.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABO.Web.Models.ProfileScans
{
    public class ProfileScanModel : ViewModelBase
    {
        public int Id { get; set; }
        public string WarehouseId { get; set; }
        public int BoxId { get; set; }
        public string BoxName { get; set; }
        public string Folder { get; set; }
        public DateTime ScannedDate { get; set; }
        public int FileCount { get; set; }
        public ProfileScanResult Result { get; set; }
        public string Description { get; set; }
    }
}