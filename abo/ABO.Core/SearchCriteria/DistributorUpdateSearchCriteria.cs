using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABO.Core.SearchCriteria
{
    public class DistributorUpdateSearchCriteria : SearchCriteriaBase
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long? DistNumber { get; set; }
        public IEnumerable<short> Statuses { get; set; }
        public string WarehouseId { get; set; }
        public string UpdateType { get; set; }
    }
}
