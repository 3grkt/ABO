using MvcContrib.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABO.Web.Models.ProfileBox
{
    public class ProfileBoxsModel:  ViewModelBase
    {
        public ProfileBoxsModel()
        {
            Data = new List<ProfileBoxGridModel>();
        }

        public ProfileBoxSearchModel SearchModel { get; set; }
        public IList<ProfileBoxGridModel> Data { get; set; }
        public IPagination<ProfileBoxGridModel> Pager { get; set; }
        //public ProfileBoxInfomationModel ProfileBox { get; set; }

    }
}