 
 using System;
using System.Collections.Generic;

namespace ABO.Core.Domain
{

    public partial class User : EntityBase
    {
        public User()
        {
            this.DataPurges = new List<DataPurge>();
            this.Profiles = new List<Profile>();
            this.ProfileBoxes = new List<ProfileBox>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public short RoleId { get; set; }
        public string WarehouseId { get; set; }
        public virtual ICollection<DataPurge> DataPurges { get; set; }
        public virtual ICollection<Profile> Profiles { get; set; }
        public virtual ICollection<ProfileBox> ProfileBoxes { get; set; }
        public virtual Role Role { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual ICollection<DistributorLetter> DistributorLetter { get; set; }

    }
}

