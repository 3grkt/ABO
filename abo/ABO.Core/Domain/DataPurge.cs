 
 using System;
using System.Collections.Generic;

namespace ABO.Core.Domain
{

    public partial class DataPurge : EntityBase
    {
        public int Id { get; set; }
        public System.DateTime PurgeDate { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public int UserID { get; set; }
        public Nullable<int> FileCount { get; set; }
        public string PurgeLog { get; set; }
        public virtual User User { get; set; }
    }
}

