using ABO.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABO.Web.Models.Profiles
{
    public class ProfileADAChangeModel : ViewModelBase
    {
        public int Id { get; set; }
        [ResourceDisplayName("Profile.CurrentADA")]
        public long DistNumber { get; set; }
        [ResourceDisplayName("Profile.NewADA")]
        public long NewDistNumber { get; set; }
    }
}