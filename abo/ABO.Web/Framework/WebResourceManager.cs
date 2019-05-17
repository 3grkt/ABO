using ABO.Core;
using ABO.Core.Infrastructure;
using ABO.Services.Localization;
using System.Resources;

namespace ABO.Web.Framework
{
    public class WebResourceManager : IResourceManager
    {
        private static ResourceManager _resourceManager;
        public static ResourceManager ResourceManager
        {
            get
            {
                if (_resourceManager == null)
                {
                    _resourceManager = new ResourceManager(typeof(ABO.Web.Properties.Resources));
                }
                return _resourceManager;
            }
        }

        public string GetString(string resourceKey)
        {
            //var key = resourceKey.Replace(".", "_"); // Replace dot
            //return ResourceManager.GetString(key, WorkContext.Current.WorkingCulture);
            var value = ResourceManager.GetString(resourceKey, EngineContext.Current.Resolve<IWorkContext>().WorkingCulture);
            return string.IsNullOrEmpty(value) ? resourceKey : value;
        }
    }
}