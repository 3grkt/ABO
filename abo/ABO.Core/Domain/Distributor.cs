 
 using System;
using System.Collections.Generic;

namespace ABO.Core.Domain
{

    public partial class Distributor : EntityBase
    {
        public Distributor()
        {
            this.DistributorLetters = new List<DistributorLetter>();
            this.DistributorLetters1 = new List<DistributorLetter>();
            this.DistributorLetters2 = new List<DistributorLetter>();
            this.DistributorLetters3 = new List<DistributorLetter>();
            this.DistributorLogs = new List<DistributorLog>();
            this.DistributorUpdates = new List<DistributorUpdate>();
            this.Profiles = new List<Profile>();
        }

        public long DistNumber { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Telephone { get; set; }
        public string City { get; set; }
        public Nullable<System.DateTime> JoinDate { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public string WarehouseId { get; set; }
        public virtual ICollection<DistributorLetter> DistributorLetters { get; set; }
        public virtual ICollection<DistributorLetter> DistributorLetters1 { get; set; }
        public virtual ICollection<DistributorLetter> DistributorLetters2 { get; set; }
        public virtual ICollection<DistributorLetter> DistributorLetters3 { get; set; }
        public virtual ICollection<DistributorLog> DistributorLogs { get; set; }
        public virtual ICollection<DistributorUpdate> DistributorUpdates { get; set; }
        public virtual ICollection<Profile> Profiles { get; set; }
        public virtual Warehouse Warehouse { get; set; }
    }
}

