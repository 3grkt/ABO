using ABO.Core;
using ABO.Core.Domain;
using ABO.Services.ImportExport;
using ABO.Services.Localization;
using ABO.Services.Users;
using ABO.Services.WareHouse;
using ABO.Web.Extensions;
using ABO.Web.Framework;
using ABO.Web.Framework.Controllers;
using ABO.Web.Models.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ABO.Web.Controllers
{
    [PermissionAuthorize(UserPermission.ManageUser)]
    public class UserController : WebControllerBase
    {
        private readonly IUserService _userService;
        private readonly IWorkContext _workContext;
        private readonly IResourceManager _resourceManager;
        private readonly IWebHelper _webHelper;
        private readonly IExportManager _exportManager;
        private readonly IAppSettings _appSettings;
        private readonly IWarehouseService _warehouseService;

        public UserController(IUserService userService, IWorkContext workContext, IResourceManager resourceManager, IWebHelper webHelper,
            IExportManager exportManager, IAppSettings appSettings, IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
            _userService = userService;
            _workContext = workContext;
            _resourceManager = resourceManager;
            _webHelper = webHelper;
            _exportManager = exportManager;
            _appSettings = appSettings;
        }

        public ActionResult Index(UserSearchModel search, int pageIndex = 1, int pageSize = 10)
        {
            var model = new UsersModel();

            Dictionary<string, string> warehouses = _warehouseService.GetAllWarehouses();
            ViewBag.Warehouses = WebUtility.ConvertDictionaryToSelectList(warehouses, true);
            Dictionary<string, string> users = _userService.GetAllUsers();
            ViewBag.Users = WebUtility.ConvertDictionaryToSelectList(users, true);
            model.Search = search;

            var data = _userService.GetUsers(pageIndex, pageSize, search.WarehouseId, (search.UserId == null) ? 0 : search.UserId.Value);
            model.Data = data.Select(x => x.ToModel<UserGridModel>()).ToList();
            model.Pager = data.ToMvcPaging(model.Data);
            return View(model);
        }
        
        public async Task<ActionResult> GetUserNewForm()
        {
            var userInfo = new UserModel { IsEditMode = false };

            Dictionary<string, string> roles = _userService.GetRoles();
            userInfo.RoleList = WebUtility.ConvertDictionaryToSelectList(roles, false);

            Dictionary<string, string> warehouses = _warehouseService.GetAllWarehouses();
            userInfo.WarehouseList = WebUtility.ConvertDictionaryToSelectList(warehouses, false);

            return PartialView("_NewEditForm", userInfo);
        }

        public async Task<ActionResult> GetUserEditForm(int id)
        {
            ViewBag.ProfileBoxRootPath = _appSettings.ProfileBoxRootPath;
            var user = _userService.GetUserById(id);

            var userInfo = new UserModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                WarehouseId = user.WarehouseId,
                RoleId = user.RoleId,
                IsEditMode = true
            };

            Dictionary<string, string> roles = _userService.GetRoles();
            userInfo.RoleList = WebUtility.ConvertDictionaryToSelectList(roles, false);

            Dictionary<string, string> warehouses = _warehouseService.GetAllWarehouses();
            userInfo.WarehouseList = WebUtility.ConvertDictionaryToSelectList(warehouses, false);

            return PartialView("_NewEditForm", userInfo);
        }

        [HttpPost]
        public ActionResult InsertEdit(UserModel user)
        {
            if (user.IsEditMode)
            {
                var entity = _userService.GetUserById(user.UserId);
                if (entity != null)
                {
                    entity.UserName = user.UserName;
                    entity.RoleId = (short)user.RoleId;
                    entity.WarehouseId = user.WarehouseId;
                    entity.FullName = user.FullName;
                    _userService.UpdateUser(entity);

                    SuccessNotification(_resourceManager.GetString("User.SuccessToEditUser"));
                    return Redirect(Request.UrlReferrer.ToString());
                }
                else
                {
                    ErrorNotification(_resourceManager.GetString("User.ErrorToEditUser"));
                    return Redirect(Request.UrlReferrer.ToString());
                }

            }
            else
            {
                var entity = _userService.GetUserByUsername(user.UserName);
                if (entity != null)
                {
                    ErrorNotification(_resourceManager.GetString("User.ErrorToAddExistedUser"));
                    return Redirect(Request.UrlReferrer.ToString());
                }
                else
                {
                    User u = new User
                    {
                        UserName = user.UserName,
                        RoleId = (short)user.RoleId,
                        WarehouseId = user.WarehouseId,
                        FullName = user.FullName
                    };
                    _userService.InsertUser(u);

                    SuccessNotification(_resourceManager.GetString("User.SuccessToAddUser"));
                    return Redirect(Request.UrlReferrer.ToString());
                }
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var entity = _userService.GetUserById(id);
            if (entity != null)
                _userService.DeleteUser(entity);
            else
            {
                ErrorNotification(_resourceManager.GetString("User.ErrorToDeleteUser"));
                return Redirect(Request.UrlReferrer.ToString());
            }
            SuccessNotification(_resourceManager.GetString("User.SuccessToDeleteUser") + id);
            return Redirect(Request.UrlReferrer.ToString());
        }


        [HttpPost]
        [ActionName("Index")]
        [FormValueRequired("ExportToExcel")]
        public ActionResult Index(UserSearchModel search)
        {
            try
            {
                var users = _userService.GetUsers(1, _webHelper.ExcelSheetMaxRows, search.WarehouseId, (search.UserId == null) ? 0 : search.UserId.Value);
                var table = new DataTable();
                table.Columns.Add(_resourceManager.GetString("User.Name"));
                table.Columns.Add(_resourceManager.GetString("User.FullName"));
                table.Columns.Add(_resourceManager.GetString("User.Warehouse"));
                table.Columns.Add(_resourceManager.GetString("User.Role"));

                foreach (var user in users)
                {
                    table.Rows.Add(new object[]
                {
                    user.UserName,
                    user.FullName,
                    user.Warehouse.WarehouseName,
                    user.Role.Name,
                });
                }

                var excelData = _exportManager.ExportExcelFromDataTable(table, "Users", null);
                return File(excelData, FileContentType.EXCEL, "Users.xlsx");
            }
            catch (Exception ex)
            {
                ErrorNotification(_resourceManager.GetString("Error" + ex.Message));
                return Redirect(Request.RawUrl);
            }
        }

    }
}
