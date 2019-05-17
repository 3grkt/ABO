using ABO.Core.Infrastructure;
using ABO.Services.Logging;
//using AOB.Services.Offices;
using ABO.Web.Framework.UI;
using ABO.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using AOB.Services.Locations;
//using AOB.Services.Departments;
using ABO.Core.Domain;
using ABO.Services.Localization;
using ABO.Web.Helpers;
using ABO.Core;
using ABO.Services.WareHouse;

namespace ABO.Web.Framework.Controllers
{
    [HandleError]
    public abstract class WebControllerBase : Controller
    {
        #region Actions

        public ActionResult PageError()
        {
            return new ErrorResult();
        }

        public ActionResult PageNotFound(string message = null)
        {
            return new NotFoundResult(message);
        }

        public ActionResult PageForbidden()
        {
            return new ForbiddenResult();
        }

        public JsonResult AjaxResult(AjaxProcessResult result)
        {
            return Json(new { result = result.ToString() });
        }
        #endregion

        #region Common Data

        //protected IList<SelectListItem> GetAllOffices()
        //{
        //    return EngineContext.Current.Resolve<IOfficeService>().GetAllOffices().Select(o =>
        //        new SelectListItem()
        //        {
        //            Text = o.OFFName,
        //            Value = o.OfficeID.ToString()
        //        })
        //        .ToList()
        //        .InsertEmptyValue(); //insert empty value to list
        //}

        //protected IList<SelectListItem> GetAllLocations()
        //{
        //    return EngineContext.Current.Resolve<ILocationService>().GetAllLocations().Select(l =>
        //        new SelectListItem()
        //        {
        //            Text = l.LocationDescription,
        //            Value = l.LocationID.ToString()
        //        })
        //        .ToList()
        //        .InsertEmptyValue();
        //}

        //protected IList<SelectListItem> GetAllDepartments()
        //{
        //    return EngineContext.Current.Resolve<IDepartmentService>().GetAllDepartments().Select(d =>
        //        new SelectListItem()
        //        {
        //            Text = d.DeptName,
        //            Value = d.DeptID.ToString()
        //        })
        //        .ToList()
        //        .InsertEmptyValue();
        //}

        protected IEnumerable<SelectListItem> GetAllWarehouses()
        {
            return EngineContext.Current.Resolve<IWarehouseService>().GetAllWarehouses().Select(d =>
                new SelectListItem()
                {
                    Text = d.Value,
                    Value = d.Key
                })
                .ToList()
                .InsertEmptyValue();
        }

        protected IEnumerable<SelectListItem> GetAllProfileScanResults()
        {
            return new SelectListItem[]
            {
                new SelectListItem(){ Text =EngineContext.Current.Resolve<IResourceManager>().GetString("Common.All"), Value = ProfileScanResult.None.ToString("D")},
                new SelectListItem(){Text = ProfileScanResult.OK.ToString().ToUpper(), Value = ProfileScanResult.OK.ToString("D")},
                new SelectListItem(){Text = ProfileScanResult.Error.ToString().ToUpper(), Value = ProfileScanResult.Error.ToString("D")}
            };
        }

        #endregion

        #region Utility

        // /// <summary>
        // /// Generates excel file from asset list.
        // /// </summary>
        // /// <param name="_resourceManager"></param>
        // /// <param name="assets"></param>
        // /// <param name="fileName"></param>
        //protected FileResult GenerateAssetExcel(IResourceManager _resourceManager, IList<SearchedAsset> assets, string fileName)
        // {
        //     // Create datatable
        //     var tbl = new DataTable();
        //     tbl.Columns.Add(_resourceManager.GetString("Asset.AssetCode"));
        //     tbl.Columns.Add(_resourceManager.GetString("Asset.Description"));
        //     tbl.Columns.Add(_resourceManager.GetString("Asset.Manufacturer"));
        //     tbl.Columns.Add(_resourceManager.GetString("Asset.SerialNumber"));
        //     tbl.Columns.Add(_resourceManager.GetString("Asset.Value"));
        //     tbl.Columns.Add(_resourceManager.GetString("Asset.Location"));
        //     tbl.Columns.Add(_resourceManager.GetString("Asset.Status"));

        //     foreach (var ast in assets)
        //     {
        //         tbl.Rows.Add(new object[]
        //         {
        //             ast.AssetCode,
        //             ast.Description,
        //             ast.Manufacturer,
        //             ast.SerialNumber,
        //             ast.AssetValue,
        //             ast.LocationName,
        //             ast.StatusName
        //         });
        //     }

        //     // Set column widths in excel
        //     var columnWidths = new int[]
        //     {
        //         10, // AssetCode
        //         30, // Description
        //         25, // Manufacturer
        //         15, // SerialNumber
        //         15, // AssetValue
        //         20, // Location
        //         20, // Status
        //     };

        //     return File(GenerateExcelFromDataTable(tbl, fileName, "Asset", columnWidths),
        //                 FileContentType.EXCEL,
        //                 fileName);
        // }


        /// <summary>
        /// Log exception
        /// </summary>
        /// <param name="exc">Exception</param>
        protected void LogException(Exception exc)
        {
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            var logger = EngineContext.Current.Resolve<ILogger>();

            var customer = workContext.User;
            logger.WriteLog(exc.Message, exc, customer);
        }
        /// <summary>
        /// Display success notification
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void SuccessNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotificationType.Success, message, persistForTheNextRequest);
        }

        /// <summary>
        /// Display warning notification
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void WarningNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotificationType.Warning, message, persistForTheNextRequest);
        }

        /// <summary>
        /// Display error notification
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        /// <param name="logException">A value indicating whether exception should be logged</param>
        protected virtual void ErrorNotification(string message, Exception exception = null, bool persistForTheNextRequest = true)
        {
            if (exception != null)
                LogException(exception);
            AddNotification(NotificationType.Error, message, persistForTheNextRequest);
        }
        /// <summary>
        /// Display notification
        /// </summary>
        /// <param name="type">Notification type</param>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void AddNotification(NotificationType type, string message, bool persistForTheNextRequest)
        {
            string dataKey = string.Format("notifications.{0}", type);
            if (persistForTheNextRequest)
            {
                if (TempData[dataKey] == null)
                    TempData[dataKey] = new List<string>();
                ((List<string>)TempData[dataKey]).Add(message);
            }
            else
            {
                if (ViewData[dataKey] == null)
                    ViewData[dataKey] = new List<string>();
                ((List<string>)ViewData[dataKey]).Add(message);
            }
        }

        protected virtual int CorrectPageIndex(int page, int pageSize, int totalItems)
        {
            if (totalItems > 0)
            {
                var lastPage = (int)Math.Ceiling((float)totalItems / pageSize);
                if (page > lastPage)
                    page = lastPage;
            }
            return page;
        }

        protected virtual T LoadSessionData<T>(string sessionKey)
            where T : class
        {
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            object obj;
            if (workContext.SessionData.TryGetValue(sessionKey, out obj))
                return obj as T;
            return null;
        }

        protected virtual void SaveSessionData(string sessionKey, object data)
        {
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            workContext.SessionData[sessionKey] = data;
        }
        #endregion
    }
}