using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MvcContrib.Sorting;
using MvcContrib.UI.Grid;

using ABO.Core;
using ABO.Services.Profiles;
using ABO.Services.Security;
using ABO.Web.Framework;
using ABO.Web.Framework.Controllers;
using ABO.Web.Framework.UI;
using ABO.Web.Models.ProfileTypes;
using ABO.Web.Extensions;
using ABO.Core.Domain;
using ABO.Services.Logging;

namespace ABO.Web.Controllers
{
    [PermissionAuthorize(UserPermission.ManageProfileType)]
    public class ProfileTypeController : WebControllerBase
    {
        private readonly IPermissionService _permissionService;
        private readonly IProfileTypeService _profileTypeService;
        private readonly ILogger _logger;

        public ProfileTypeController(IPermissionService permisisonService, IProfileTypeService profileTypeService, ILogger logger)
        {
            _permissionService = permisisonService;
            _profileTypeService = profileTypeService;
            _logger = logger;
        }

        //
        // GET: /ProfileType/

        public ActionResult Index(GridSortOptions sort = null)
        {
            //if (!_permissionService.Authorize(UserPermission.ManageProfileType))
            //    return PageForbidden();

            if (sort == null || string.IsNullOrEmpty(sort.Column))
                sort = new GridSortOptions() { Column = "Name", Direction = SortDirection.Ascending };
            ViewBag.Sort = sort;

            var model = new ProfileTypeIndexModel();
            var profileTypes = _profileTypeService.GetAllTypes(sort.Column, WebUtility.GetSortDir(sort));
            model.ProfileTypes = profileTypes.Select(x => x.ToModel<ProfileTypeModel>()).ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult AjaxEdit(int id)
        {
            var profileType = _profileTypeService.GetById(id);
            if (profileType == null)
                return Content("");

            return PartialView("_CreateOrEdit", profileType.ToModel<ProfileTypeModel>());
        }

        [HttpPost]
        public ActionResult AjaxCreate()
        {
            var model = new ProfileTypeModel();
            return PartialView("_CreateOrEdit", model);
        }

        [HttpPost]
        public ActionResult AjaxDelete(int id)
        {
            var entity = _profileTypeService.GetById(id);
            if (entity == null)
                return AjaxResult(AjaxProcessResult.NotFound);

            if (entity.SystemType)
                return AjaxResult(AjaxProcessResult.NotValid);

            try
            {
                _profileTypeService.Delete(entity);
            }
            catch (Exception ex)
            {
                _logger.WriteLog("Failed to delete ProfileType.", ex);
                return AjaxResult(AjaxProcessResult.Failed);
            }
            return AjaxResult(AjaxProcessResult.Success);
        }

        [HttpPost]
        public ActionResult CreateOrEdit(ProfileTypeModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.Id == 0)
                    {
                        var entity = model.ToEntity<ProfileType>();
                        _profileTypeService.Insert(entity);
                    }
                    else
                    {
                        var entity = _profileTypeService.GetById(model.Id);
                        if (entity == null)
                            return AjaxResult(AjaxProcessResult.NotFound);

                        entity = model.ToEntity(entity);
                        _profileTypeService.Update(entity);
                    }
                }
                catch (Exception ex)
                {
                    _logger.WriteLog("Failed to create or edit ProfileType.", ex, null);
                    return AjaxResult(AjaxProcessResult.Failed);
                }
            }
            return AjaxResult(AjaxProcessResult.Success);
        }
    }
}
