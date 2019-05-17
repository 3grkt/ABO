using ABO.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABO.Services.ImportExport
{
    public interface IExcelParamManager
    {
        ExcelParamCollection GetExcelParams(string templateName);
    }
}
