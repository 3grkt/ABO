using ABO.Core;
using ABO.Core.SearchCriteria;
using ABO.Services.Profiles;
using ABO.Web.Extensions;
using ABO.Web.Framework;
using ABO.Web.Framework.Controllers;
using ABO.Web.Models.ProfileScans;
using MvcContrib.Sorting;
using MvcContrib.UI.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABO.Web.Controllers
{
    [PermissionAuthorize(UserPermission.ViewProfileScanResult)]
    public class ProfileScanController : WebControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileScanController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        //
        // GET: /ProfileScan/

        public ActionResult Index(ProfileScanSearchModel search, int page = 1, int pageSize = 10, GridSortOptions sort = null)
        {
            var model = new ProfileScanIndexModel();

            if (search.StartDate == DateTime.MinValue)
                search.StartDate = DateTime.Now.AddDays(-7);
            if (search.EndDate == DateTime.MinValue)
                search.EndDate = DateTime.Now;

            // Set profile box data
            if (sort == null || string.IsNullOrEmpty(sort.Column))
                sort = new GridSortOptions() { Column = "ScannedDate", Direction = SortDirection.Descending };
            ViewBag.Sort = sort;
            
            var boxes = _profileService.SearchProfileScans(
                search.ToCriteria<ProfileScanSearchCriteria>(),
                page,
                pageSize,
                sort.Column,
                WebUtility.GetSortDir(sort));
            model.Scans = boxes.Select(x => x.ToModel<ProfileScanModel>()).ToList();
            model.Pager = boxes.ToMvcPaging(model.Scans);
            model.Search = search;
            model.AvailableWarehouses = GetAllWarehouses();
            model.AvailableResults = GetAllProfileScanResults();

            // If current page > total pages, redirect to last page
            if (model.Pager.TotalPages > 0 && model.Pager.PageNumber > model.Pager.TotalPages)
                return Redirect(Url.Paging(model.Pager.TotalPages, model.Pager.PageSize).ToString());

            return View(model);
        }

    }
}
