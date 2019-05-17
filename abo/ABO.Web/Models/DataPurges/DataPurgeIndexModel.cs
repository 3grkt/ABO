using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABO.Web.Models.DataPurges
{
    public class DataPurgeIndexModel : ViewModelBase
    {
        public DataPurgeIndexModel()
        {
            Purges = new List<DataPurgeModel>();
            RegisteredModel = new DataPurgeModel();
        }

        public IList<DataPurgeModel> Purges { get; set; }
        public MvcContrib.Pagination.IPagination<DataPurgeModel> Pager { get; set; }
        public DataPurgeModel RegisteredModel { get; set; }
    }
}