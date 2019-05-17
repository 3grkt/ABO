 
 using System;
using System.Collections.Generic;

namespace ABO.Core.Domain
{

    public partial class DistributorUpdate : EntityBase
    {
        public int Id { get; set; }
        public string WarehouseId { get; set; }
        public long DistNumber { get; set; }
        public string UpdatedType { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public short StatusId { get; set; }
        public virtual Distributor Distributor { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual Status Status { get; set; }
    }
}

