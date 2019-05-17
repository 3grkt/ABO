using System;
using System.Collections.Generic;
using System.Globalization;

namespace ABO.Core
{
    public interface IWorkContext
    {
        /// <summary>
        /// Gets or sets working culture.
        /// </summary>
        CultureInfo WorkingCulture { get; set; }

        /// <summary>
        /// Gets or sets current working user.
        /// </summary>
        WorkingUser User { get; set; }

        /// <summary>
        /// Gets data stored in session.
        /// </summary>
        Dictionary<string, object> SessionData { get; }
    }
}
