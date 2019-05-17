
using MvcContrib.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABO.Web.Models.Distributors
{
    public class DistributorLetterModel : ViewModelBase
    {
        public DistributorLetterModel()
        {
            Data = new List<DistributorGridModel>();
        }
        public DistributorLetterSearchModel Search { get; set; }
        public NewLetterModel NewLetter { get; set; }
        public IList<DistributorGridModel> Data { get; set; }
        public IPagination<DistributorGridModel> Pager { get; set; }
    }
}