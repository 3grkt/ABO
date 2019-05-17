using ABO.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ABO.Services.ImportExport
{
    public class ExcelParamManager : IExcelParamManager
    {
        private const string EXCEL_PARAMS_CACHE_NAME = "ExcelParams";
        private IWebHelper _webHelper;

        public ExcelParamManager(IWebHelper webHelper)
        {
            this._webHelper = webHelper;
        }

        #region IExcelParamManager Members

        public ExcelParamCollection GetExcelParams(string templateName)
        {
            ExcelParamCollection collection = null;

            var cachedParams = GetCachedExcelParams();
            cachedParams.TryGetValue(templateName, out collection);

            return collection;
        }

        #endregion

        private IDictionary<string, ExcelParamCollection> GetCachedExcelParams()
        {
            var cachedParams = System.Web.HttpContext.Current.Cache[EXCEL_PARAMS_CACHE_NAME] as IDictionary<string, ExcelParamCollection>;
            if (cachedParams == null)
            {
                cachedParams = new Dictionary<string, ExcelParamCollection>();

                XDocument doc = XDocument.Load(_webHelper.ExcelParamConfigFile);
                foreach (var templateElement in doc.Element("ExcelParams").Elements("Template"))
                {
                    var templateName = templateElement.Attribute("name").Value;
                    var collection = new ExcelParamCollection() { TemplateName = templateName };
                    foreach (var param in templateElement.Elements("Param"))
                    {
                        collection[param.Attribute("name").Value] = param.Value; // if there are 2 parmas with the same key, last param is used
                    }
                    
                    cachedParams[templateName] = collection;
                }

                System.Web.HttpContext.Current.Cache.Add(
                    EXCEL_PARAMS_CACHE_NAME,
                    cachedParams,
                    new System.Web.Caching.CacheDependency(_webHelper.ExcelParamConfigFile),
                    System.Web.Caching.Cache.NoAbsoluteExpiration,
                    TimeSpan.FromMinutes(60), // hard-code 60 minutes
                    System.Web.Caching.CacheItemPriority.Normal,
                    null);
            }
                
            return cachedParams;
        }
    }
}
