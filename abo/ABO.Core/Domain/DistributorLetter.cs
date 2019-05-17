 
 using System;
using System.Collections.Generic;

namespace ABO.Core.Domain
{

    public partial class DistributorLetter : EntityBase
    {
        public int Id { get; set; }
        public System.DateTime LetterDate { get; set; }
        public long DistNumber { get; set; }
        public string DistName { get; set; }
        public Nullable<long> SponsorId { get; set; }
        public Nullable<long> PlatiniumSponsorId { get; set; }
        public Nullable<long> OldDistNumber { get; set; }
        public string WHId { get; set; }
        public Nullable<int> UserId { get; set; }

        public virtual Distributor Distributor { get; set; }
        public virtual Distributor Distributor1 { get; set; }
        public virtual Distributor Distributor2 { get; set; }
        public virtual Distributor Distributor3 { get; set; }
        public virtual User User { get; set; }
        public virtual Warehouse Warehouse { get; set; }

    }
}

