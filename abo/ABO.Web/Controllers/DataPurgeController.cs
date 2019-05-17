using ABO.Core;
using ABO.Core.Domain;
using ABO.Services.DataPurges;
using ABO.Services.Localization;
using ABO.Services.Logging;
using ABO.Web.Extensions;
using ABO.Web.Framework;
using ABO.Web.Framework.Controllers;
using ABO.Web.Models.DataPurges;
using MvcContrib.Sorting;
using MvcContrib.UI.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABO.Web.Controllers
{
    [PermissionAuthorize(UserPermission.PurgeData)]
    public class DataPurgeController : WebControllerBase
    {
        private readonly IDataPurgeService _dataPurgeService;
        private readonly IWorkContext _workContext;
        private readonly IResourceManager _resourceManager;
        private readonly ILogger _logger;

        public DataPurgeController(IDataPurgeService dataPurgeService, IWorkContext workContext, IResourceManager resourceManager, ILogger logger)
        {
            _dataPurgeService = dataPurgeService;
            _workContext = workContext;
            _resourceManager = resourceManager;
            _logger = logger;
        }

        public ActionResult Index(int page = 1, int pageSize = 10, GridSortOptions sort = null)
        {
            var model = new DataPurgeIndexModel();

            if (model.RegisteredModel.PurgeDate == DateTime.MinValue)
                model.RegisteredModel.PurgeDate = DateTime.Today;
            if (model.RegisteredModel.StartDate == DateTime.MinValue)
                model.RegisteredModel.StartDate = DateTime.Today;
            if (model.RegisteredModel.EndDate == DateTime.MinValue)
                model.RegisteredModel.EndDate = DateTime.Today;


            // Set profile box data
            if (sort == null || string.IsNullOrEmpty(sort.Column))
                sort = new GridSortOptions() { Column = "PurgeDate", Direction = SortDirection.Ascending };
            ViewBag.Sort = sort;

            var boxes = _dataPurgeService.SearchDataPurges(
                page,
                pageSize,
                sort.Column,
                WebUtility.GetSortDir(sort));
            model.Purges = boxes.Select(x => x.ToModel<DataPurgeModel>()).ToList();
            model.Pager = boxes.ToMvcPaging(model.Purges);

            // If current page > total pages, redirect to last page
            if (model.Pager.TotalPages > 0 && model.Pager.PageNumber > model.Pager.TotalPages)
                return Redirect(Url.Paging(model.Pager.TotalPages, model.Pager.PageSize).ToString());

            return View(model);
        }

        [HttpPost]
        [ActionName("Index")]
        [FormValueRequired("register")]
        public ActionResult Register([Bind(Prefix = "RegisteredModel")] DataPurgeModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.ToEntity<DataPurge>();

                entity.UserID = _workContext.User.UserID;

                try
                {
                    _dataPurgeService.Insert(entity);
                    SuccessNotification(_resourceManager.GetString("DataPurge.SuccessToRegister"));
                }
                catch (Exception ex)
                {
                    ErrorNotification(_resourceManager.GetString("DataPurge.FailedToRegister"), ex);
                }
            }
            return Redirect(Request.RawUrl);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            bool success = true;
            string message = string.Empty;

            if (ModelState.IsValid)
            {
                DataPurge entity = _dataPurgeService.GetDataPurgeById(id);
                if (entity != null)
                {
                    try
                    {
                        _dataPurgeService.Delete(entity);
                        //SuccessNotification(_resourceManager.GetString("DataPurge.SuccessToDeletePurge"));

                    }
                    catch (Exception ex)
                    {
                        //ErrorNotification(_resourceManager.GetString("DataPurge.FailedToDeletePurge"), ex);
                        success = false;
                        message = _resourceManager.GetString("DataPurge.FailedToDeletePurge");
                        _logger.WriteLog("Failed to delete data purge with id=" + id, ex);
                    }
                }
            }
            return Json(new { success, message });
        }

    }
}
