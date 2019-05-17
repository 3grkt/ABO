using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABO.Core.SearchCriteria
{
    public class ProfileBoxSearchCriteria : SearchCriteriaBase
    {
        public int Id { get; set; }
        public string ProfileBoxName { get; set; }
        public int UserID { get; set; }
        public string WarehouseID { get; set; }
        public int StatusID { get; set; }
        public int ProfileTypeID { get; set; }
    }
}
