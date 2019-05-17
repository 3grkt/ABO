 
 using System;
using System.Collections.Generic;

namespace ABO.Core.Domain
{

    public partial class Permission : EntityBase
    {
        public Permission()
        {
            this.Roles = new List<Role>();
        }

        public short Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}

