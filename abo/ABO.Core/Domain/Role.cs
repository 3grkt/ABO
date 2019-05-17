 
 using System;
using System.Collections.Generic;

namespace ABO.Core.Domain
{

    public partial class Role : EntityBase
    {
        public Role()
        {
            this.Users = new List<User>();
            this.Permissions = new List<Permission>();
        }

        public short Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
    }
}

