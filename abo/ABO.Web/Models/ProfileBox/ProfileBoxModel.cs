using MvcContrib.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABO.Web.Models.ProfileBox
{
    public class ProfileBoxModel
    {
        public ProfileBoxModel()
        {
            Profiles = new List<ProfileModel>();
        }

        public int BoxId { get; set; }
        public ProfileBoxDetailModel ProfileBox { get; set; }
        public IList<ProfileModel> Profiles { get; set; }
        public IPagination<ProfileModel> Pager { get; set; }
        public int? StatusId { get; set; }
        public int? ProfileCount { get; set; }
        public IList<SelectListItem> StatusList { get; set; }
        public IList<SelectListItem> LocationList { get; set; }

    }
}