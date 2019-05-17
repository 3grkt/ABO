using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABO.Services.Localization
{
    public interface IResourceManager
    {
        string GetString(string resourceKey);
    }
}
