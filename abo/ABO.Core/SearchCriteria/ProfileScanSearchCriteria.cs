using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABO.Core.SearchCriteria
{
    public class ProfileScanSearchCriteria : SearchCriteriaBase
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string WarehouseId { get; set; }
        public short Result { get; set; }
    }
}
