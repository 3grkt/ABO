using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABO.Web.Models.Distributors
{
    public class DistributorGridModel:ViewModelBase
    {
        public long DistNumber { get; set; }
        public string DistName { get; set; }
        public long OldDistNumber { get; set; }
        public string OldDistName { get; set; }
        public string DistAddress { get; set; }
        public string OldDistAddress { get; set; }
        public long SponsorNum { get; set; }
        public string SponsorName { get; set; }
        public string SponsorAddress { get; set; }
        public long PlatiumNumber { get; set; }
        public string PlatiumName { get; set; }
        public string PlatiumAddress { get; set; }
        public DateTime LetterDate { get; set; }
        public String Warehouse { get; set; }
        public String Creator { get; set; }
        public string DistTelephone { get; set; }
        public string OldDistTelephone { get; set; }
        public string SponsorTelephone { get; set; }
        public string PlatiumTelephone { get; set; }
    }
}