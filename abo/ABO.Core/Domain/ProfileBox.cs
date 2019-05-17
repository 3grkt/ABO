
using System;
using System.Collections.Generic;

namespace ABO.Core.Domain
{

    public partial class ProfileBox : EntityBase
    {
        public ProfileBox()
        {
            this.Profiles = new List<Profile>();
            this.ProfileScans = new List<ProfileScan>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string WarehouseId { get; set; }
        public string OfficeId { get; set; }
        public Nullable<int> LocationId { get; set; }
        public int TypeId { get; set; }
        public short StatusId { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public int ADACount { get; set; }
        public Nullable<int> ProfileCount { get; set; }
        public string ScannedFolder { get; set; }
        public virtual Location Location { get; set; }
        public virtual ICollection<Profile> Profiles { get; set; }
        public virtual ProfileType ProfileType { get; set; }
        public virtual Status Status { get; set; }
        public virtual User User { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual Warehouse Office { get; set; }
        public virtual ICollection<ProfileScan> ProfileScans { get; set; }
    }
}

