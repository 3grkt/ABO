using ABO.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABO.Web.Models.Distributors
{
    public class NewLetterModel : ViewModelBase
    {
        public Int64 OldDistNumber { get; set; }
        public string OldDistName { get; set; }
        public Int64 DistNumber { get; set; }
        public string DistName { get; set; }
        public Int64 SponsorNumber { get; set; }
        public string SponsorName { get; set; }
        public Int64 PlatiumNumber { get; set; }
        public string PlatiumName { get; set; }

    }
}