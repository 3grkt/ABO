 
 using System;
using System.Collections.Generic;

namespace ABO.Core.Domain
{

    public partial class Warehouse : EntityBase
    {
        public Warehouse()
        {
            this.Distributors = new List<Distributor>();
            this.DistributorUpdates = new List<DistributorUpdate>();
            this.Profiles = new List<Profile>();
            this.ProfileBoxes = new List<ProfileBox>();
            this.ProfileBoxes2 = new List<ProfileBox>();
            this.ProfileScans = new List<ProfileScan>();
            this.Users = new List<User>();
        }

        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public int BoxCount { get; set; }
        public short Year { get; set; }
        public virtual ICollection<Distributor> Distributors { get; set; }
        public virtual ICollection<DistributorUpdate> DistributorUpdates { get; set; }
        public virtual ICollection<Profile> Profiles { get; set; }
        public virtual ICollection<ProfileBox> ProfileBoxes { get; set; }
        public virtual ICollection<ProfileBox> ProfileBoxes2 { get; set; }
        public virtual ICollection<ProfileScan> ProfileScans { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<DistributorLetter> DistributorLetter { get; set; }
        public virtual ICollection<DistributorLog> DistributorLogs { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
    }
}

