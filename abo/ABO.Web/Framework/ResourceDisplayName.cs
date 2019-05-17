using ABO.Core.Infrastructure;
using ABO.Services.Localization;
using ABO.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Web;

namespace ABO.Web.Framework
{
    public class ResourceDisplayName : DisplayNameAttribute
    {
        public string ResourceKey { get; set; }

        private static IResourceManager _resoureManager = EngineContext.Current.Resolve<IResourceManager>();

        public ResourceDisplayName(string resourceKey)
        {
            this.ResourceKey = resourceKey;
        }

        public override string DisplayName
        {
            get
            {
                return _resoureManager.GetString(ResourceKey);
            }
        }
    }
}