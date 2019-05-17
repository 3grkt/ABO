 
 using System;
using System.Collections.Generic;

namespace ABO.Core.Domain
{

    public partial class Status : EntityBase
    {
        public Status()
        {
            this.Profiles = new List<Profile>();
            this.ProfileBoxes = new List<ProfileBox>();
            this.DistributorLogs = new List<DistributorLog>();
            this.DistributorUpdates = new List<DistributorUpdate>();
        }

        public short Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string StatusType { get; set; }
        public virtual ICollection<Profile> Profiles { get; set; }
        public virtual ICollection<ProfileBox> ProfileBoxes { get; set; }
        public virtual ICollection<DistributorLog> DistributorLogs { get; set; }
        public virtual ICollection<DistributorUpdate> DistributorUpdates { get; set; }
    }
}

