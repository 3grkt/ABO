using ABO.Core;
using ABO.Core.SearchCriteria;
using ABO.Services.Logging;
using ABO.Services.Profiles;
using ABO.Services.Security;
using ABO.Services.Users;
using ABO.Services.WareHouse;
using ABO.Web.Extensions;
using ABO.Web.Framework;
using ABO.Web.Framework.Controllers;
using ABO.Web.Models.ProfileBox;
using MvcContrib.UI.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ABO.Core.Domain;
using ABO.Services.Distributors;

using ABO.Web.Models.Profiles;
using System.Threading.Tasks;
using ABO.Services.Localization;
using System.Data;
using ABO.Services.ImportExport;
using System.Collections;
using System.IO;
using System.Net.Mime;
using Novacode;
using ZXing;
using ZXing.Common;
using System.Drawing.Imaging;

namespace ABO.Web.Controllers
{
    [PermissionAuthorize(UserPermission.ManageProfileBox, UserPermission.ViewProfileBoxDetails, UserPermission.CreateProfileBox)]
    public class ProfileBoxController : WebControllerBase
    {
        private const string PROFILEBOX_UPDATECOUNT_LOG = "PROFILE IS MOVED FROM BOX ({0}) TO ({1})";

        private readonly IPermissionService _permissionService;
        private readonly IProfileTypeService _profileTypeService;
        private readonly IWarehouseService _warehouseService;
        private readonly ILogger _logger;
        private readonly IUserService _userService;
        private readonly IProfileBoxService _profileBoxService;
        private readonly IProfileService _profileService;
        private readonly IWorkContext _workContext;
        private readonly IResourceManager _resourceManager;
        private readonly IWebHelper _webHelper;
        private readonly IExportManager _exportManager;
        private readonly IAppSettings _appSettings;

        public ProfileBoxController(IPermissionService permisisonService, IProfileTypeService profileTypeService, ILogger logger,
            IWarehouseService warehouseService, IUserService userService, IProfileBoxService profileBoxService,
            IProfileService profileService, IWorkContext workContext, IResourceManager resourceManager, IWebHelper webHelper,
            IExportManager exportManager, IAppSettings appSettings)
        {
            _permissionService = permisisonService;
            _profileTypeService = profileTypeService;
            _logger = logger;
            _warehouseService = warehouseService;
            _userService = userService;
            _profileBoxService = profileBoxService;
            _profileService = profileService;
            _workContext = workContext;
            _resourceManager = resourceManager;
            _webHelper = webHelper;
            _exportManager = exportManager;
            _appSettings = appSettings;
        }

        public ActionResult Index(ProfileBoxSearchModel search, int page = 1, int pageSize = 5, GridSortOptions sort = null)
        {
            var model = new ProfileBoxsModel();
            //model.ProfileBox = new ProfileBoxInfomationModel
            //{
            //    Name = DateTime.Now.ToString("ddMMyyyy_HH") + "_" + _workContext.User.UserName,
            //    UserName = _workContext.User.UserName
            //};

            Dictionary<string, string> profileTypes = _profileTypeService.GetAllTypes();
            ViewBag.ProfileType = WebUtility.ConvertDictionaryToSelectList(profileTypes, true);

            Dictionary<string, string> warehouses = _warehouseService.GetAllWarehouses();
            ViewBag.Warehouses = WebUtility.ConvertDictionaryToSelectList(warehouses, true);


            Dictionary<string, string> users = _userService.GetAllUsers();
            ViewBag.Users = WebUtility.ConvertDictionaryToSelectList(users, true);



            ViewBag.StatusList = WebUtility.ConvertEnumToSelectList<ABO.Core.ProfileBoxStatus>(true);
            model.SearchModel = search;

            var data = _profileBoxService.Search(page, pageSize, search.ToCriteria<ProfileBoxSearchModel, ProfileBoxSearchCriteria>());

            model.Data = data.Select(x => x.ToModel<ProfileBoxGridModel>()).ToList();

            ViewBag.ShowAddButton = _workContext.User.WarehouseId;

            model.Pager = data.ToMvcPaging(model.Data);

            return View(model);
        }

        public ActionResult GetProfileBoxNewForm(string profileType)
        {
            ViewBag.ProfileBoxRootPath = _appSettings.ProfileBoxRootPath;
            var info = new ProfileBoxInfomationModel
            {
                UserName = _workContext.User.UserName,
                WarehouseId = _workContext.User.WarehouseId,
                WarehouseName = _workContext.User.WarehouseName,

            };
            //info.FolderPath = folderpath + "/" + info.WarehouseName.Trim() + "/" + info.Name;
            Dictionary<string, string> profileTypes = _profileTypeService.GetAllTypes();
            info.ProfileTypeList = WebUtility.ConvertDictionaryToSelectList(profileTypes, false);

            // Set profile box name and save current box count/year
            var warehouseBoxNumber = _profileBoxService.GetNextBoxNumber(_workContext.User.WarehouseId);
            info.Name = GetProfileBoxName(info.ProfileTypeList.Count > 0 ? info.ProfileTypeList[0].Text : "", _workContext.User.WarehouseId, warehouseBoxNumber.BoxNumber, warehouseBoxNumber.Year);
            info.CurrentBoxCount = warehouseBoxNumber.BoxNumber;
            info.CurrentWHYear = warehouseBoxNumber.Year;

            //Dictionary<string, string> warehouses = _warehouseService.GetAllWarehouses();
            //info.WarehousesList = WebUtility.ConvertDictionaryToSelectList(warehouses, false);

            return PartialView("_ProfileBoxAddNew", info);
        }

        [HttpPost]
        public ActionResult Insert(ProfileBoxInfomationModel box)
        {
            var path = box.FolderPath;
            var profile = _profileBoxService.GetByFolderPath(path);
            if (profile != null)
            {
                ErrorNotification(_resourceManager.GetString("Profile.FailedToAddProfileBoxDulicatePath"));
                return Redirect(Request.UrlReferrer.ToString());
            }
            else
            {
                ProfileBox newBox = new ProfileBox
                {
                    Name = box.Name,
                    ADACount = 0,
                    OfficeId = _workContext.User.WarehouseId,
                    ProfileCount = 0,
                    WarehouseId = _workContext.User.WarehouseId,
                    TypeId = box.ProfileType,
                    StatusId = (short)ABO.Core.ProfileBoxStatus.Open,
                    CreatedBy = _workContext.User.UserID,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    ScannedFolder = box.FolderPath
                };

                _profileBoxService.Insert(newBox);
                SuccessNotification(_resourceManager.GetString("Profile.SuccessToAddProfileBox") + " File lưu tại: " + path);
                return Redirect(Request.UrlReferrer.ToString());
            }
        }
        //[AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        [HttpGet]
        public ActionResult Detail(ProfileBoxModel boxdetail, int id, int statusId = 0, int page = 1, int pageSize = 5)
        {
            var model = new ProfileBoxModel();
            var profileBox = _profileBoxService.GetById(id);
            if (profileBox == null)
                return PageNotFound();
            profileBox.WarehouseId = (boxdetail.ProfileBox == null) ? profileBox.WarehouseId : boxdetail.ProfileBox.Warehouse;


            model.ProfileBox = profileBox.ToModel<ProfileBoxDetailModel>();
            model.ProfileBox.StatusList = WebUtility.ConvertEnumToSelectList<ABO.Core.ProfileBoxStatus>(false);
            Dictionary<string, string> warehouses = _warehouseService.GetAllWarehouses();
            model.ProfileBox.WarehouseList = WebUtility.ConvertDictionaryToSelectList(warehouses, false);
            Dictionary<string, string> profileTypes = _profileTypeService.GetAllTypes();
            model.ProfileBox.ProfileTypeList = WebUtility.ConvertDictionaryToSelectList(profileTypes, false);

            Dictionary<string, string> locations = _profileBoxService.GetLocationByWarehouseId(profileBox.WarehouseId);
            model.LocationList = WebUtility.ConvertDictionaryToSelectList(locations, false);

            Dictionary<string, string> openBoxes = _profileBoxService.GetProfileBoxByStatusType((short)ABO.Core.ProfileBoxStatus.Open, profileBox.TypeId, profileBox.Id);
            ViewBag.OpenBoxes = WebUtility.ConvertDictionaryToSelectList(openBoxes, false);

            // Get Profiles
            var profiles = _profileService.SearchProfilesByStatus(id, statusId, page, pageSize, null, null);
            model.Profiles = profiles.Select(x => x.ToModel<ProfileModel>()).ToList();
            model.Pager = profiles.ToMvcPaging(model.Profiles);
            model.StatusList = WebUtility.ConvertEnumToSelectList<ABO.Core.ProfileStatus>(true);

            model.ProfileBox.ProfileCount = model.ProfileBox.ProfileCount;
            return View(model);
        }

        [HttpPost]
        [ActionName("Detail")]
        [FormValueRequired("movedBox")]
        public ActionResult MoveTo(FormCollection form, int boxId)
        {
            var movedList = new List<int>();
            foreach (var key in form.AllKeys.Where(x => x.StartsWith("IsSelected-")))
            {
                if ((bool)form.GetValue(key).ConvertTo(typeof(bool)))
                {
                    var profileId = 0;
                    if (int.TryParse(key.Replace("IsSelected-", ""), out profileId))
                        movedList.Add(profileId);
                }
            }

            if (movedList.Count > 0)
            {
                var profiles = _profileService.GetProfilesByIds(movedList, true);


                foreach (var p in profiles)
                {
                    if (p.StatusId != (short)ABO.Core.ProfileStatus.Valid)
                    {
                        ErrorNotification(_resourceManager.GetString("Profile.MoveToError"));
                        return Redirect(Request.RawUrl);
                    }
                }
                try
                {
                    foreach (var profile in profiles)
                    {
                        _profileService.MoveProfile(profile, boxId);
                    }

                    // Update ADA and Profile count
                    var currentBoxId = int.Parse(form["fromBoxId"]);
                    var boxCountUpdateLog = string.Format(PROFILEBOX_UPDATECOUNT_LOG, currentBoxId, boxId);
                    _profileService.UpdateBoxCount(new int[] { boxId }, _workContext.User.GetLogString(), boxCountUpdateLog);
                    _profileService.UpdateBoxCount(new int[] { currentBoxId }, _workContext.User.GetLogString(), boxCountUpdateLog);

                    SuccessNotification(_resourceManager.GetString("Profile.SuccessToMoveProfile"));
                }
                catch (Exception ex)
                {
                    _logger.WriteLog("Failed to Move Profile.", ex);
                    ErrorNotification(_resourceManager.GetString("Profile.FailedToMoveProfile"));
                }
            }
            return Redirect(Request.RawUrl);
        }

        [HttpPost]
        [ActionName("Index")]
        [FormValueRequired("ExportToExcel")]
        public ActionResult Index(ProfileBoxSearchModel search)
        {
            try
            {
                var profileBoxes = _profileBoxService.Search(1, _webHelper.ExcelSheetMaxRows, search.ToCriteria<ProfileBoxSearchModel, ProfileBoxSearchCriteria>());

                var table = new DataTable();
                table.Columns.Add(_resourceManager.GetString("ProfileBox.Name"));
                table.Columns.Add(_resourceManager.GetString("ProfileBox.CreatedDate"));
                table.Columns.Add(_resourceManager.GetString("ProfileBox.UserID"));
                table.Columns.Add(_resourceManager.GetString("ProfileBox.WareHouse"));
                table.Columns.Add(_resourceManager.GetString("ProfileBox.BoxStatus2"));
                table.Columns.Add(_resourceManager.GetString("ProfileBox.AdaCount"));

                foreach (var box in profileBoxes)
                {
                    table.Rows.Add(new object[]
                {
                    box.Name,
                    box.CreatedDate.ToString("dd/MM/yyyy"),
                    box.User.UserName,
                    box.Warehouse.WarehouseName,
                    box.Status.Name,
                    box.ADACount,
                    
                });
                }

                var excelData = _exportManager.ExportExcelFromDataTable(table, "ProfileBoxes", null);
                return File(excelData, FileContentType.EXCEL, "ProfileBoxes.xlsx");
            }
            catch (Exception ex)
            {
                ErrorNotification(_resourceManager.GetString("Error" + ex.Message));
                return Redirect(Request.RawUrl);
            }
        }

        [HttpPost]
        [ActionName("Index")]
        [FormValueRequired("deleteBox")]
        public ActionResult DeleteProfiles(FormCollection form)
        {
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
                var profileBoxes = _profileBoxService.GetProfileBoxByIds(deletedList);


                foreach (var p in profileBoxes)
                {
                    if (p.StatusId != (short)ABO.Core.ProfileBoxStatus.NeedToDiscard)
                    {
                        ErrorNotification(_resourceManager.GetString("ProfileBox.DiscardError"));
                        return Redirect(Request.RawUrl);
                    }
                }
                try
                {
                    _profileBoxService.DeleteProfileBoxs(deletedList);
                    SuccessNotification(_resourceManager.GetString("Profile.SuccessToDeleteProfileBoxes"));
                }
                catch (Exception ex)
                {
                    _logger.WriteLog("Failed to delete profiles.", ex);
                    ErrorNotification(_resourceManager.GetString("Profile.FailedToDeleteProfileBoxes"));
                }
            }
            return Redirect(Request.RawUrl);
        }

        [HttpPost]
        [ActionName("Detail")]
        [FormValueRequired("ExportToExcel")]
        public ActionResult ExportToExcel(int id, int statusId = 0)
        {
            try
            {
                var profiles = _profileService.SearchProfilesByStatus(id, statusId, 1, _webHelper.ExcelSheetMaxRows, null, null);
                var profileBox = _profileBoxService.GetById(id);

                var excelData = _exportManager.GenerateProfileBoxDetailExcel(profiles, profileBox);
                return File(excelData, FileContentType.EXCEL, "profiles_.xlsx");
            }
            catch (Exception ex)
            {
                ErrorNotification(_resourceManager.GetString("Error" + ex.Message));
                return Redirect(Request.RawUrl);
            }
        }

        [HttpPost]
        [ActionName("Detail")]
        [FormValueRequired("pakaged")]
        public ActionResult Pakaged(int id, ProfileBoxModel boxdetail)
        {
            var userRoles = _workContext.User.Roles;
            var box = _profileBoxService.GetById(id);
            // Check whether status changes in order or not
            if (box.StatusId != (short)ABO.Core.ProfileBoxStatus.Open && box.StatusId != (short)ABO.Core.ProfileBoxStatus.Moved)
            {
                ErrorNotification(_resourceManager.GetString("Profile.FailedToChangeStatusProfileBoxes"));
                return Redirect(Request.RawUrl);
            }
            if (userRoles.Contains(ABO.Core.UserRole.TeamLeader))
            {
                var location = (boxdetail.ProfileBox.Location != null) ? boxdetail.ProfileBox.Location.Value : 0;
                _profileBoxService.UpdateStatus(id, (short)ABO.Core.ProfileBoxStatus.Packed,location, boxdetail.ProfileCount, boxdetail.ProfileBox.Warehouse);
                SuccessNotification(_resourceManager.GetString("Profile.SuccessToPackagedProfileBoxes"));
            }
            else
            {
                ErrorNotification(_resourceManager.GetString("Profile.FailedToPackagedProfileBoxes"));
            }
            return Redirect(Request.RawUrl);
        }

        [HttpPost]
        [ActionName("Detail")]
        [FormValueRequired("moved")]
        public ActionResult Moved(int id, ProfileBoxModel boxdetail)
        {
            var userRoles = _workContext.User.Roles;
            var box = _profileBoxService.GetById(id);
            // Check whether status changes in order or not
            if (box.StatusId != (short)ABO.Core.ProfileBoxStatus.Packed && box.StatusId != (short)ABO.Core.ProfileBoxStatus.Stored)
            {
                ErrorNotification(_resourceManager.GetString("Profile.FailedToChangeStatusProfileBoxes"));
                return Redirect(Request.RawUrl);
            }
            if (userRoles.Contains(ABO.Core.UserRole.TeamLeader))
            {
                var location = (boxdetail.ProfileBox.Location != null) ? boxdetail.ProfileBox.Location.Value : 0;
                _profileBoxService.UpdateStatus(id, (short)ABO.Core.ProfileBoxStatus.Moved, location, null, boxdetail.ProfileBox.Warehouse);
                SuccessNotification(_resourceManager.GetString("Profile.SuccessToMoveProfileBoxes"));
            }
            else
            {
                ErrorNotification(_resourceManager.GetString("Profile.FailedToMoveProfileBoxes"));
            }
            return Redirect(Request.RawUrl);
        }

        [HttpPost]
        [ActionName("Detail")]
        [FormValueRequired("store")]
        public ActionResult Stored(int id, ProfileBoxModel boxdetail)
        {
            //palce to store
            var userRoles = _workContext.User.Roles;
            var box = _profileBoxService.GetById(id);
            // Check whether status changes in order or not
            if (box.StatusId < (short)ABO.Core.ProfileBoxStatus.Moved)
            {
                ErrorNotification(_resourceManager.GetString("Profile.FailedToChangeStatusProfileBoxes"));
                return Redirect(Request.RawUrl);
            }
            if (userRoles.Contains(ABO.Core.UserRole.TeamLeader))
            {
                var location = (boxdetail.ProfileBox.Location != null) ? boxdetail.ProfileBox.Location.Value : 0;
                _profileBoxService.UpdateStatus(id, (short)ABO.Core.ProfileBoxStatus.Stored, location, null, boxdetail.ProfileBox.Warehouse);
                SuccessNotification(_resourceManager.GetString("Profile.SuccessToStoreProfileBoxes"));
            }
            else
            {
                ErrorNotification(_resourceManager.GetString("Profile.FailedToStoreProfileBoxes"));
            }
            return Redirect(Request.RawUrl);
        }

        [HttpPost]
        [ActionName("Detail")]
        [FormValueRequired("discard")]
        public ActionResult Discard(int id, ProfileBoxModel boxdetail)
        {
            var userRoles = _workContext.User.Roles;
            var box = _profileBoxService.GetById(id);
            if (box.StatusId < (short)ABO.Core.ProfileBoxStatus.Stored)
            {
                ErrorNotification(_resourceManager.GetString("Profile.FailedToChangeStatusProfileBoxes"));
                return Redirect(Request.RawUrl);
            }
            if (userRoles.Contains(ABO.Core.UserRole.TeamLeader))
            {
                // check profile
                var profiles = _profileService.SearchProfilesByStatus(id, 0, 1, int.MaxValue, null, null);
                if (profiles.Count != 0)
                {
                    var count = profiles.Count(t => t.StatusId != (short)ProfileStatus.Deleted);
                    if (count > 0)
                    {
                        ErrorNotification(_resourceManager.GetString("Profile.FailedToDiscardProfileBoxes2"));
                        return Redirect(Request.RawUrl);
                    }
                }
                var location = (boxdetail.ProfileBox.Location != null) ? boxdetail.ProfileBox.Location.Value : 0;
                _profileBoxService.UpdateStatus(id, (short)ABO.Core.ProfileBoxStatus.Discarded, location, null, null);
                SuccessNotification(_resourceManager.GetString("Profile.SuccessToDiscardProfileBoxes"));
            }
            else
            {
                ErrorNotification(_resourceManager.GetString("Profile.FailedToDiscardProfileBoxes"));
            }
            return Redirect(Request.RawUrl);
        }

        [HttpPost]
        public string GetProfileBoxName(string profileType, string warehouseId, int boxCount, short year)
        {
            return string.Format("{0}{1}{2}-{3:0000}",
                year % 100,  // year - e.g. 15
                UnicodeCharacterReplacer.ReplaceString(profileType).Replace(" ", ""), // profile type - e.g. SA88
                warehouseId, // warehouse - e.g. 01
                boxCount); // box count - e.g. 0012
        }

        [HttpPost]
        [ActionName("Detail")]
        [FormValueRequired("printLabel")]
        public async Task<ActionResult> PrintLabel(int id)
        {
            Stream stream = CreateLabel(id);
            //TODO: redirect to error page
            if (stream == null)
                return Content("error message", MediaTypeNames.Text.Plain);
            var fileName = "Label.docx";
            return File(stream, "application/zip", fileName);
        }

        private Stream CreateLabel(int id)
        {

            var model = new ProfileBoxModel();
            var profileBox = _profileBoxService.GetById(id);
            string label = profileBox.Name.ToUpper();
            string profileCount = "";
            if (!profileBox.ProfileType.Scanned)
            {
                profileCount = profileBox.ProfileCount.ToString();
            }
            else
            {
                var profiles = _profileService.SearchProfilesByStatus(id, 0, 1, int.MaxValue, null, null);
                profileCount = profiles.Count(x => x.StatusId != (short)ProfileStatus.Deleted).ToString();
            }
            string filePath = Server.MapPath(Url.Content("~/Template/Label.docx"));
            DocX document = DocX.Load(filePath);
            var outputStream = new MemoryStream();
            document.ReplaceText("{Label}", label);
            document.ReplaceText("{count}", profileCount);
            // get placeholder image
            Novacode.Image img = document.Images[0];
            // create barcode encoder
            var barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                Options = new EncodingOptions
                {
                    PureBarcode = false,
                    Height = 100,
                    Width = 300,
                    Margin = 10
                }
            };
            // create barcode image
            var bitmap = barcodeWriter.Write(label);
            // replace place holder image in document with barcode image
            bitmap.Save(img.GetStream(FileMode.Create, FileAccess.Write), ImageFormat.Png);
            
            document.SaveAs(outputStream);
            outputStream.Position = 0;
            document.Dispose();
            return outputStream;
        }
    }
}
