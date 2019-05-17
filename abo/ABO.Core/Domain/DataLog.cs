 
 using System;
using System.Collections.Generic;

namespace ABO.Core.Domain
{

    public partial class DataLog : EntityBase
    {
        public int Id { get; set; }
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public string PK { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public System.DateTime LogDate { get; set; }
        public string LogUser { get; set; }
        public string LogInfo { get; set; }
    }
}

