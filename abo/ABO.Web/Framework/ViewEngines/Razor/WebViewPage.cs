using ABO.Core.Infrastructure;
using ABO.Services.Localization;
using ABO.Web.Framework.UI;
using System.Web;

namespace ABO.Web.Framework.ViewEngines.Razor
{
    public abstract class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        private IResourceManager _resourceManager;

        public override void InitHelpers()
        {
            base.InitHelpers();

            _resourceManager = EngineContext.Current.Resolve<IResourceManager>();
        }

        /// <summary>
        /// Returns text from resource file.
        /// </summary>
        /// <param name="resourceKey"></param>
        /// <returns></returns>
        public CustomHtmlString T(string resourceKey, params object[] values)
        {
            string text;
            if (values != null)
                text = string.Format(_resourceManager.GetString(resourceKey), values);
            else
                text = _resourceManager.GetString(resourceKey);
            return new CustomHtmlString(text);
        }
    }

    public abstract class WebViewPage : WebViewPage<dynamic>
    {
    }
}