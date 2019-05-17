using ABO.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABO.Web.Models.ProfileTypes
{
    public class ProfileTypeModel : ViewModelBase
    {
        public int Id { get; set; }
        [ResourceDisplayName("ProfileType.Name")]
        public string Name { get; set; }
        [ResourceDisplayName("ProfileType.StoredYears")]
        public short StoredYears { get; set; }
        [ResourceDisplayName("ProfileType.Description")]
        public string Description { get; set; }
        [ResourceDisplayName("ProfileType.Scanned")]
        public bool Scanned { get; set; }
        public bool SystemType { get; set; }
        [ResourceDisplayName("ProfileType.Image")]
        public bool Image { get; set; }
    }
}