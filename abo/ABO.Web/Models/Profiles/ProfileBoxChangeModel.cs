using ABO.Web.Models.ProfileBoxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABO.Web.Models.Profiles
{
    public class ProfileBoxChangeModel : ViewModelBase
    {
        public ProfileBoxChangeModel()
        {
            AvailableBoxes = new List<SelectListItem>();
        }

        //public ProfileBoxModel CurrentBox { get; set; }
        public int Id { get; set; }
        public int BoxId { get; set; }
        public IList<SelectListItem> AvailableBoxes { get; set; }
    }
}