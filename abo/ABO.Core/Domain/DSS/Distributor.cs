using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABO.Core.Domain.DSS
{
    /// <summary>
    /// Represents distributor record in DSS database.
    /// </summary>
    public class Distributor : EntityBase
    {
        public Int64 DistributorNo { get; set; }
        public String Name1 { get; set; }
        public String Sex1 { get; set; }
        public String Name2 { get; set; }
        public String Sex2 { get; set; }
        public String Address1 { get; set; }
        public String Address2 { get; set; }
        public String Address3 { get; set; }
        public String Address4 { get; set; }
        public String City { get; set; }
        public String Mobile { get; set; }
        public String HPHONE { get; set; }
        public String BPHONE { get; set; }
        public String Status { get; set; }
        public String Class { get; set; }
        public String Award { get; set; }
        public Int32 JDATE { get; set; }
        public Int32 EJDATE { get; set; }
        public Int32 SPONS { get; set; }
        public Int32 DD { get; set; }
        public Int32 DIAMOND { get; set; }
        public Int32 BDATE1 { get; set; }
        public String BPLACE1 { get; set; }
        public Int32 BDATE2 { get; set; }
        public String BPLACE2 { get; set; }
        public String ID1 { get; set; }
        public Int32 IDDATE1 { get; set; }
        public String IDPLACE1 { get; set; }
        public String ID2 { get; set; }
        public Int32 IDDATE2 { get; set; }
        public String IDPLACE2 { get; set; }
    }
}
