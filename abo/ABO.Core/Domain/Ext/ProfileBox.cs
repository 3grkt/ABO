using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABO.Core.Domain
{
    public class SearchProfileBox : EntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserName { get; set; }
        public string WarehouseName { get; set; }
        public string Status { get; set; }
        public int ADACount { get; set; }
    }
}
