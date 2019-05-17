using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABO.Core
{
    /// <summary>
    /// Contains parameter used in an excel template file.
    /// </summary>
    public class ExcelParamCollection : Dictionary<string, string>
    {
        public string TemplateName { get; set; }
        
        public ExcelParamCollection()
            : base()
        {
        }
    }
}
