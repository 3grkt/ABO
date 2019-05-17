using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ABO.Core.Domain.WTA;

namespace ABO.Web.Models.ProfileImageMerging
{
    public class ProspectAvatarModel: ViewModelBase
    {
        public long DistNumber { get; set; }
        public KeyValuePair<string, ProspectAvatar> FirstPersonImage { get; set; }
        public KeyValuePair<string, ProspectAvatar> SecondPersonImage { get; set; }
    }
}