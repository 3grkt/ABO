using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcContrib.UI.Grid;

using ABO.Core;
using ABO.Core.Domain;
using ABO.Services.Distributors;
using ABO.Services.Profiles;
using ABO.Web.Extensions;
using ABO.Web.Framework;
using ABO.Web.Framework.Controllers;
using ABO.Web.Models.Profiles;
using MvcContrib.Sorting;
using ABO.Services.Localization;
using ABO.Services.Logging;
using ABO.Services.ImportExport;
using System.IO;

namespace ABO.Web.Controllers
{
    [PermissionAuthorize(UserPermission.ManageDistributorProfile, UserPermission.ViewDistributorProfile)]
    public class ProfileController : WebControllerBase
    {
        #region Fields
        private readonly IProfileService _profileService;
        private readonly IDistributorService _distributorService;
        private readonly IResourceManager _resourceManager;
        private readonly IExportManager _exportManager;
        private readonly ILogger _logger;
        private readonly IWorkContext _workContext;
        private readonly IAppSettings _appSettings;
        #endregion

        #region Ctor.
        public ProfileController(
            IProfileService profileService,
            IDistributorService distributorService,
            IResourceManager resourceManager,
            IExportManager exportManager,
            ILogger logger,
            IWorkContext workContext,
            IAppSettings appSettings)
        {
            _profileService = profileService;
            _distributorService = distributorService;
            _resourceManager = resourceManager;
            _exportManager = exportManager;
            _logger = logger;
            _workContext = workContext;
            _appSettings = appSettings;
        }
        #endregion

        #region Actions
        public ActionResult Index(long? distNumber = null, int page = 1, int pageSize = 10, GridSortOptions sort = null)
        {
            var model = new ProfileIndexModel();

            if (distNumber.HasValue)
            {
                // Set distributor data
                var distributor = _distributorService.GetDistributorById(distNumber.Value);
                if (distributor != null)
                    model.Distributor = distributor.ToModel<ProfileIndexModel.DistributorModel>();

                // Set profile box data
                if (sort == null || string.IsNullOrEmpty(sort.Column))
                    sort = new GridSortOptions() { Column = "CreatedDate", Direction = SortDirection.Descending };
                ViewBag.Sort = sort;

                var boxes = _profileService.SearchProfilesByDistNumber(
                    distNumber.Value,
                    page,
                    pageSize,
                    sort.Column,
                    WebUtility.GetSortDir(sort));
                model.ProfileBoxes = boxes.Select(x => x.ToModel<ProfileIndexModel.ProfileModel>()).ToList();
                model.Pager = boxes.ToMvcPaging(model.ProfileBoxes);

                // If current page > total pages, redirect to last page
                if (model.Pager.TotalPages > 0 && model.Pager.PageNumber > model.Pager.TotalPages)
                    return Redirect(Url.Paging(model.Pager.TotalPages, model.Pager.PageSize).ToString());
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult DownloadProfileFile(int downloadProfileId)
        {
            var profile = _profileService.GetProfileById(downloadProfileId, includeProfileBox: true, includeProfileType: true);
            if (profile == null)
                return PageNotFound();

            var profileFilePath = Path.Combine(_appSettings.ProfileBoxFolder, profile.ProfileBox.Name + "\\" + profile.FileName);
            if (!System.IO.File.Exists(profileFilePath))
                return PageNotFound(_resourceManager.GetString("Profile.DownloadedFileNotFound"));

            var fileName = string.Format("{0}_{1}{2}",
                Path.GetFileNameWithoutExtension(profile.FileName),
                UnicodeCharacterReplacer.ReplaceString(profile.ProfileType.Name),
                Path.GetExtension(profile.FileName));
            return File(profileFilePath, FileContentType.PDF, fileName);
        }

        [HttpPost]
        public ActionResult LoadProfileBoxChange(int profileId)
        {
            var profile = _profileService.GetProfileById(profileId);
            if (profile == null)
                return Content("");

            var model = profile.ToModel<ProfileBoxChangeModel>();
            model.AvailableBoxes = _profileService.GetAvailableProfileBoxes()
                .Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
                .ToList();

            return PartialView("_ChangeProfileBox", model);
        }

        [HttpPost]
        [ActionName("Index")]
        [FormValueRequired("saveProfileBoxChange")]
        public ActionResult SaveProfileBoxChange(ProfileBoxChangeModel model)
        {
            if (ModelState.IsValid)
            {
                var profile = _profileService.GetProfileById(model.Id);
                if (profile == null)
                    return PageNotFound();

                if (profile.BoxId != model.BoxId)
                {
                    try
                    {
                        _profileService.ChangeProfileBox(profile, model.BoxId, _workContext.User.GetLogString());
                        SuccessNotification(_resourceManager.GetString("Profile.SuccessToChangeProfileBox"));
                    }
                    catch (Exception ex)
                    {
                        _logger.WriteLog("Failed to change profile box.", ex);
                        ErrorNotification(_resourceManager.GetString("Profile.FailedToChangeProfileBox"));
                    }
                }
            }
            return Redirect(Request.RawUrl);
        }

        [HttpPost]
        public ActionResult LoadADAChange(int profileId)
        {
            var profile = _profileService.GetProfileById(profileId);
            if (profile == null)
                return Content("");

            var model = profile.ToModel<ProfileADAChangeModel>();
            return PartialView("_ChangeADA", model);
        }

        [HttpPost]
        [ActionName("Index")]
        [FormValueRequired("saveADAChange")]
        public ActionResult SaveADAChange(ProfileADAChangeModel model)
        {
            if (ModelState.IsValid)
            {
                var profile = _profileService.GetProfileById(model.Id);
                if (profile == null)
                    return PageNotFound();

                var newDist = _distributorService.GetDistributorById(model.NewDistNumber);
                if (newDist != null)
                {
                    try
                    {
                        _profileService.ChangeADA(profile, newDist, _workContext.User.GetLogString());
                        SuccessNotification(_resourceManager.GetString("Profile.SuccessToChangeADA"));
                    }
                    catch (Exception ex)
                    {
                        _logger.WriteLog("Failed to change ADA.", ex);
                        ErrorNotification(_resourceManager.GetString("Profile.FailedToChangeADA"));
                    }
                }
                else
                {
                    ErrorNotification(_resourceManager.GetString("Profile.ChangeADA.NewADANotFound"));
                }


            }
            return Redirect(Request.RawUrl);
        }

        [HttpPost]
        [ActionName("Index")]
        [FormValueRequired("deleteProfiles")]
        public ActionResult DeleteProfiles(long distNumber, FormCollection form)
        {
            var distributor = _distributorService.GetDistributorById(distNumber);
            if (distributor == null)
                return PageNotFound();

            var deletedList = new List<int>();
            foreach (var key in form.AllKeys.Where(x => x.StartsWith("IsSelected-")))
            {
                if ((bool)form.GetValue(key).ConvertTo(typeof(bool)))
                {
                    var profileId = 0;
                    if (int.TryParse(key.Replace("IsSelected-", ""), out profileId))
                        deletedList.Add(profileId);
                }
            }

            if (deletedList.Count > 0)
            {
                try
                {
                    _profileService.DeleteProfiles(distributor, deletedList, _workContext.User.GetLogString());
                    SuccessNotification(_resourceManager.GetString("Profile.SuccessToDeleteProfiles"));
                }
                catch (Exception ex)
                {
                    _logger.WriteLog("Failed to delete profiles.", ex);
                    ErrorNotification(_resourceManager.GetString("Profile.FailedToDeleteProfiles"));
                }
            }
            return Redirect(Request.RawUrl);
        }

        [HttpPost]
        [ActionName("Index")]
        [FormValueRequired("exportToPdf")]
        public ActionResult ExportToPdf(int distNumber)
        {
            var dist = _distributorService.GetDistributorById(distNumber);
            if (dist == null)
                return PageNotFound();

            try
            {
                var profiles = _profileService.GetDistributorProfilesForExport(dist.DistNumber);
                var exportedData = _exportManager.ExportProfilesForDistributor(dist, profiles);
                var fileName = dist.DistNumber + FileExtension.ZIP;
                return File(exportedData, FileContentType.ZIP, fileName);
            }
            catch (Exception ex)
            {
                ErrorNotification(_resourceManager.GetString("Profile.FailedToExportPDFs"));
                _logger.WriteLog("Failed to export distributor's profiles to PDFs.", ex);
            }

            return Redirect(Request.RawUrl);
        }
        #endregion
    }
}
