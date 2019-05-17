 
 using System;
using System.Collections.Generic;

namespace ABO.Core.Domain
{

    public partial class ProfileScan : EntityBase
    {
        public int Id { get; set; }
        public string WarehouseId { get; set; }
        public int BoxId { get; set; }
        public System.DateTime ScannedDate { get; set; }
        public int FileCount { get; set; }
        public short Result { get; set; }
        public string Description { get; set; }
        public virtual ProfileBox ProfileBox { get; set; }
        public virtual Warehouse Warehouse { get; set; }
    }
}

