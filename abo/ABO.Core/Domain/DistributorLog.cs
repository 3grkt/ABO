 
 using System;
using System.Collections.Generic;

namespace ABO.Core.Domain
{

    public partial class DistributorLog : EntityBase
    {
        public long Id { get; set; }
        public string WAREHOUSE { get; set; }
        public int DISTNO { get; set; }
        public string UPDATETYPE { get; set; }
        public int UPDATEDTE { get; set; }
    }
}

