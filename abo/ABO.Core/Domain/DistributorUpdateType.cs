 
 using System;
using System.Collections.Generic;

namespace ABO.Core.Domain
{

    public partial class DistributorUpdateType : EntityBase
    {
        public string Type { get; set; }
        public int ProfileTypeId { get; set; }
        public string Description { get; set; }
        public virtual ProfileType ProfileType { get; set; }
    }
}

