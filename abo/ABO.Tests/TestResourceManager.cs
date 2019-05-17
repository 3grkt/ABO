using ABO.Services.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABO.Tests
{
    public class TestResourceManager : IResourceManager
    {
        #region IResourceManager Members

        public string GetString(string resourceKey)
        {
            return resourceKey;
        }

        #endregion
    }

}
