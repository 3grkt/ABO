using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABO.Web.Framework.UI
{
    /// <summary>
    /// Represents result of Ajax process.
    /// </summary>
    public enum AjaxProcessResult
    {
        Success,
        Failed,
        NotFound,
        NotValid
    }
}