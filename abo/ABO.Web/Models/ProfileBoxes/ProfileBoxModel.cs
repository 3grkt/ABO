using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABO.Web.Models.ProfileBoxes
{
    public class ProfileBoxModel : ViewModelBase
    {
        public ProfileBoxModel()
        {

        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string WarehouseId { get; set; }
        public Nullable<int> OfficeId { get; set; }
        public Nullable<int> LocationId { get; set; }
        public int TypeId { get; set; }
        public short StatusId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int ADACount { get; set; }
        public string ScannedFolder { get; set; }
    }
}