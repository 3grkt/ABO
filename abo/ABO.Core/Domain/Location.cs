 
 using System;
using System.Collections.Generic;

namespace ABO.Core.Domain
{

    public partial class Location : EntityBase
    {
        public Location()
        {
            this.ProfileBoxes = new List<ProfileBox>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string WHcode { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual ICollection<ProfileBox> ProfileBoxes { get; set; }
    }
}

