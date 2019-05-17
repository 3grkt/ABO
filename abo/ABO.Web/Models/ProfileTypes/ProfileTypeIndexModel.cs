using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABO.Web.Models.ProfileTypes
{
    public class ProfileTypeIndexModel : ViewModelBase
    {
        public ProfileTypeIndexModel()
        {
            ProfileTypes = new List<ProfileTypeModel>();
        }

        public IList<ProfileTypeModel> ProfileTypes { get; set; }
    }
}