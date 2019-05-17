 
 using System;
using System.Collections.Generic;

namespace ABO.Core.Domain
{

    public partial class ProfileType : EntityBase
    {
        public ProfileType()
        {
            this.DistributorUpdateTypes = new List<DistributorUpdateType>();
            this.Profiles = new List<Profile>();
            this.ProfileBoxes = new List<ProfileBox>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public short StoredYears { get; set; }
        public string Description { get; set; }
        public bool Scanned { get; set; }
        public bool SystemType { get; set; }
        public bool Image { get; set; }
        public virtual ICollection<DistributorUpdateType> DistributorUpdateTypes { get; set; }
        public virtual ICollection<Profile> Profiles { get; set; }
        public virtual ICollection<ProfileBox> ProfileBoxes { get; set; }
    }
}

