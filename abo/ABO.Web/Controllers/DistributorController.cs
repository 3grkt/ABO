using ABO.Core;
using ABO.Core.Domain;
using ABO.Core.SearchCriteria;
using ABO.Services.Distributors;
using ABO.Services.ImportExport;
using ABO.Services.Localization;
using ABO.Services.Logging;
using ABO.Services.WareHouse;
using ABO.Web.Extensions;
using ABO.Web.Framework;
using ABO.Web.Framework.Controllers;
using ABO.Web.Models.Distributors;
using Ionic.Zip;
using MvcContrib.Sorting;
using MvcContrib.UI.Grid;
using Novacode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ABO.Web.Controllers
{
    [PermissionAuthorize(UserPermission.ViewDistributorUpdate, UserPermission.PrintDistributorLetter)]
    public class DistributorController : WebControllerBase
    {
        #region Fields
        private readonly IDistributorService _distributorService;
        private readonly IWebHelper _webHelper;
        private readonly IExcelParamManager _excelParamManager;
        private readonly IResourceManager _resourceManager;
        private readonly IExportManager _exportManager;
        private readonly ILogger _logger;
        private readonly IWorkContext _workContext;
        private readonly IWarehouseService _warehouseService;
        #endregion

        #region Ctor.
        public DistributorController(
            IDistributorService distributorService,
            IWebHelper webHelper,
            IExcelParamManager excelParamManager,
            IResourceManager resourceManager,
            IExportManager exportManager,
            ILogger logger,
            IWarehouseService warehouseService,
             IWorkContext workContext)
        {
            _distributorService = distributorService;
            _webHelper = webHelper;
            _excelParamManager = excelParamManager;
            _resourceManager = resourceManager;
            _exportManager = exportManager;
            _logger = logger;
            _workContext = workContext;
            _warehouseService = warehouseService;
        }

        #endregion

        #region Actions
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NotScanned(NotScannedModel.DistributorUpdateSearchModel search, int page = 1, int pageSize = 10, GridSortOptions sort = null)
        {
            var model = new NotScannedModel();

            if (search.StartDate == DateTime.MinValue)
                search.StartDate = DateTime.Now.AddDays(-7);
            if (search.EndDate == DateTime.MinValue)
                search.EndDate = DateTime.Now;

            // Search criteria
            model.AllDistributorUpdateTypes = _distributorService.GetAllDistributorUpdateTypeNames().Select(x => new SelectListItem() { Text = x, Value = x }).ToList().InsertEmptyValue();
            model.AllWarehouses = WebUtility.ConvertDictionaryToSelectList(_warehouseService.GetAllWarehouses(), true);
            model.Search = search;

            if (Request.QueryString.HasKeys())
            {
                // Set profile box data
                if (sort == null || string.IsNullOrEmpty(sort.Column))
                    sort = new GridSortOptions() { Column = "DistNumber", Direction = SortDirection.Ascending };
                ViewBag.Sort = sort;

                var boxes = _distributorService.SearchDistributorUpdates(
                    new DistributorUpdateSearchCriteria
                    {
                        StartDate = CommonHelper.GetStartOfDate(search.StartDate),
                        EndDate = CommonHelper.GetEndOfDate(search.EndDate),
                        Statuses = new[] { (short)DistributorUpdateStatus.NotCompleted },
                        WarehouseId = search.WarehouseId,
                        UpdateType = search.UpdateType
                    },
                    page,
                    pageSize,
                    sort.Column,
                    WebUtility.GetSortDir(sort));
                model.DistributorUpdates = boxes.Select(x => x.ToModel<NotScannedModel.DistributorUpdateModel>()).ToList();
                model.Pager = boxes.ToMvcPaging(model.DistributorUpdates);

                // If current page > total pages, redirect to last page
                if (model.Pager.TotalPages > 0 && model.Pager.PageNumber > model.Pager.TotalPages)
                    return Redirect(Url.Paging(model.Pager.TotalPages, model.Pager.PageSize).ToString());
            }
            return View(model);
        }

        [HttpPost]
        [ActionName("NotScanned")]
        public ActionResult ExportNotScannedListToExcel(NotScannedModel.DistributorUpdateSearchModel search)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var distUpdates = _distributorService.SearchDistributorUpdates(
                        new DistributorUpdateSearchCriteria
                        {
                            StartDate = CommonHelper.GetStartOfDate(search.StartDate),
                            EndDate = CommonHelper.GetEndOfDate(search.EndDate),
                            Statuses = new[] { (short)DistributorUpdateStatus.NotCompleted },
                            WarehouseId = search.WarehouseId,
                            UpdateType = search.UpdateType
                        },
                        1,
                        _webHelper.ExcelSheetMaxRows,
                        null,
                        null);
                    var fileName = string.Format(_webHelper.ExcelFileNameFormat, ExcelExportType.DISTRIBUTOR_UPDATES, DateTime.Now, FileExtension.EXCEL);
                    var excelData = _exportManager.GenerateDistributorUpdateExcel(distUpdates, search.StartDate, search.EndDate);
                    return File(excelData, FileContentType.EXCEL, fileName);
                }
                catch (Exception ex)
                {
                    ErrorNotification(_resourceManager.GetString("Distributor.NotScanned.FailedToExportExcel"));
                    _logger.WriteLog("Failed to export not scanned distributors to excel.", ex);
                }
            }
            return Redirect(Request.RawUrl);
        }

        public ActionResult PrintLetter(DistributorLetterSearchModel search, int page = 1, int pageSize = 10, GridSortOptions sort = null)
        {
            search.StartDate = (search.StartDate.Date.ToShortDateString() == "1/1/0001") ? DateTime.Now.AddDays(-7) : search.StartDate;
            search.EndDate = (search.EndDate.Date.ToShortDateString() == "1/1/0001") ? DateTime.Now : search.EndDate;
            Dictionary<string, string> warehouses = _warehouseService.GetAllWarehouses();
            ViewBag.Warehouses = WebUtility.ConvertDictionaryToSelectList(warehouses, true);

            DistributorLetterModel model = new DistributorLetterModel
            {
                Search = search,
                NewLetter = new NewLetterModel()
            };

            var data = _distributorService.SearchDistributorLetter(search.DistributorNumber, search.StartDate, search.EndDate, search.WarehouseID, page, pageSize, sort.Column, "");
            model.Data = data.Select(x => x.ToModel<DistributorGridModel>()).ToList();
            model.Pager = data.ToMvcPaging(model.Data);
            return View(model);
        }


        [HttpPost]
        [ActionName("PrintLetter")]
        public ActionResult ExportLetterToExcel(DistributorLetterSearchModel search)
        {
            try
            {
                var letters = _distributorService.SearchDistributorLetter(search.DistributorNumber, search.StartDate, search.EndDate, search.WarehouseID, 1, _webHelper.ExcelSheetMaxRows, null, null);
                var data = letters.Select(x => x.ToModel<DistributorGridModel>()).ToList();
                var table = new System.Data.DataTable();
                table.Columns.Add(_resourceManager.GetString("NewLetter.Distributor"));
                table.Columns.Add(_resourceManager.GetString("NewLetter.DistributorName"));
                table.Columns.Add(_resourceManager.GetString("NewLetter.Address"));
                table.Columns.Add(_resourceManager.GetString("NewLetter.Telephone"));
                table.Columns.Add(_resourceManager.GetString("NewLetter.OldDistributor"));
                table.Columns.Add(_resourceManager.GetString("NewLetter.OldDistributorName"));
                table.Columns.Add(_resourceManager.GetString("NewLetter.Telephone") + "  ");
                table.Columns.Add(_resourceManager.GetString("NewLetter.Sponsor"));
                table.Columns.Add(_resourceManager.GetString("NewLetter.SponsorName"));

                table.Columns.Add(_resourceManager.GetString("NewLetter.Address") + " ");
                table.Columns.Add(_resourceManager.GetString("NewLetter.Telephone") + " ");
                table.Columns.Add(_resourceManager.GetString("NewLettter.Platium"));
                table.Columns.Add(_resourceManager.GetString("NewLettter.PlatiumName"));
                table.Columns.Add(_resourceManager.GetString("NewLetter.Address") + "   ");
                table.Columns.Add(_resourceManager.GetString("NewLetter.Telephone") + "   ");
                table.Columns.Add(_resourceManager.GetString("DistributorLetter.LetterDate"));
                table.Columns.Add(_resourceManager.GetString("NewLetter.Creator"));
                table.Columns.Add(_resourceManager.GetString("NewLetter.Warehouse"));

                foreach (var letter in data)
                {
                    table.Rows.Add(new object[]
                {
                    letter.DistNumber,
                    letter.DistName,
                    letter.DistAddress,
                    letter.DistTelephone,
                    letter.OldDistNumber,
                    letter.OldDistName,
                    letter.OldDistTelephone,
                    letter.SponsorNum,
                    letter.SponsorName,
                    letter.SponsorAddress,
                    letter.SponsorTelephone,
                    letter.PlatiumNumber,
                    letter.PlatiumName,
                    letter.PlatiumAddress,
                    letter.PlatiumTelephone,
                    letter.LetterDate.ToString("dd/MM/yyyy"),
                    letter.Creator,
                    letter.Warehouse
                });
                }

                var excelData = _exportManager.ExportExcelFromDataTable(table, "Letter", null);
                return File(excelData, FileContentType.EXCEL, "Letter_" + search.DistributorNumber + ".xlsx");
            }
            catch (Exception ex)
            {
                ErrorNotification(_resourceManager.GetString("Error" + ex.Message));
                return Redirect(Request.RawUrl);
            }

        }


        [HttpPost]
        public async Task<ActionResult> VerifyData(List<string> values)
        {
            NewLetterModel model = await GetDistributorsInformation(values);
            if (model != null)
                return Json(model, JsonRequestBehavior.AllowGet);
            else
                return Content("Không tìm thấy nhà phân phối hoặc bảo trợ", MediaTypeNames.Text.Plain);
        }

        [HttpPost]
        public async Task<ActionResult> ExportLetter(NewLetterModel info)
        {
            Stream stream = CreateLetter(info);
            //TODO: redirect to error page
            if (stream == null)
                return Content("error message", MediaTypeNames.Text.Plain);
            var fileName = info.DistNumber + "_ThuTaiGiaNhap.docx";
            return File(stream, "application/zip", fileName);
        }

        #endregion

        #region Utility
        private async Task<NewLetterModel> GetDistributorsInformation(List<string> distNums)
        {
            try
            {
                var oldDistributor = _distributorService.GetDistributorById(long.Parse(distNums[0]));
                var distributor = _distributorService.GetDistributorById(long.Parse(distNums[1]));
                var sponsor = _distributorService.GetDistributorById(long.Parse(distNums[2]));
                var platium = _distributorService.GetDistributorById(long.Parse(distNums[3]));

                if (oldDistributor == null || distributor == null || sponsor == null || platium == null)
                {
                    return null;
                }
                else
                {
                    return new NewLetterModel { OldDistName = oldDistributor.Name, DistName = distributor.Name, SponsorName = sponsor.Name, PlatiumName = platium.Name };
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLog(ex.Message, ex);

                return null;
            }
        }

        private Stream CreateLetter(NewLetterModel info)
        {
            try
            {
                var distributor = _distributorService.GetDistributorById(info.DistNumber);
                var sponsor = _distributorService.GetDistributorById(info.SponsorNumber);
                var platium = _distributorService.GetDistributorById(info.PlatiumNumber);
                var oldDistributor = _distributorService.GetDistributorById(info.OldDistNumber);
                var outputStream = new MemoryStream();


                string filePath = Server.MapPath(Url.Content("~/Template/LetterTemplate.docx"));
                DocX document = DocX.Load(filePath);
                Stream mainStream = new MemoryStream();
                document.SaveAs(mainStream);
                mainStream.Position = 0;
                mainStream = CreateLetterTemplate(mainStream, distributor, sponsor, platium, oldDistributor);

                //TODO: Add contant for file name
                Stream distributorLetter = CreateLetterAddress(mainStream, distributor);
                //zip.AddEntry("Re-apps letter-Nguoi dang ky lai.docx", distributorLetter);


                Stream sponsorLetter = CreateLetterAddress(mainStream, sponsor);
                //zip.AddEntry("Re-apps letter -Tuyen tren cu.docx", sponsorLetter);

                DocX docx = DocX.Load(distributorLetter);
                docx.InsertSectionPageBreak();
                docx.InsertDocument(DocX.Load(sponsorLetter));

                if (info.SponsorNumber != info.PlatiumNumber)
                {
                    Stream platiumLetter = CreateLetterAddress(mainStream, platium);
                    //zip.AddEntry("Re-apps letter -Platinum cu.docx", platiumLetter);
                    docx.InsertSectionPageBreak();
                    docx.InsertDocument(DocX.Load(platiumLetter));

                }
                docx.SaveAs(outputStream);
                _distributorService.AddNewLetter(new DistributorLetter
                {
                    DistName = info.DistName,
                    OldDistNumber = info.OldDistNumber,
                    PlatiniumSponsorId = info.PlatiumNumber,
                    SponsorId = info.SponsorNumber,
                    DistNumber = info.DistNumber,
                    LetterDate = DateTime.Now,
                    UserId = _workContext.User.UserID,
                    WHId = _workContext.User.WarehouseId
                });


                outputStream.Position = 0;
                return outputStream;
            }
            catch (Exception ex)
            {
                //TODO: Redirect to error Page
                return null;
            }
        }

        private Stream CreateLetterTemplate(Stream template, Distributor distributor, Distributor sponsor, Distributor platium, Distributor oldDistributor)
        {
            DocX document = DocX.Load(template);
            document.ReplaceText("{LetterDate}", DateTime.Now.ToString("dd/MM/yyyy"));
            document.ReplaceText("{PlatiumName}", platium.Name);
            document.ReplaceText("{PlatiumADA}", platium.DistNumber.ToString());
            document.ReplaceText("{DistOldADA}", oldDistributor.DistNumber.ToString());
            document.ReplaceText("{DistExpiredDate}", oldDistributor.ExpiryDate == null ? "" : oldDistributor.ExpiryDate.Value.ToString("dd/MM/yyyy"));
            document.ReplaceText("{Sponsor}", sponsor.Name);
            document.ReplaceText("{SponsorAda}", sponsor.DistNumber.ToString());
            document.ReplaceText("{DistName}", distributor.Name.ToString());
            document.ReplaceText("{DistNewAda}", distributor.DistNumber.ToString());

            Stream stream = new MemoryStream();
            document.SaveAs(stream);
            //stream.Position = 0;
            return stream;
        }

        private Stream CreateLetterAddress(Stream template, Distributor distributor)
        {
            DocX document = DocX.Load(template);
            document.ReplaceText("{reciverAda}", distributor.DistNumber.ToString());
            document.ReplaceText("{reciverName}", distributor.Name);
            document.ReplaceText("{reciverAddress1}", distributor.Address1);
            document.ReplaceText("{reciverAddress2}", distributor.Address2);
            document.ReplaceText("{reciverAddress3}", distributor.Address3);
            document.ReplaceText("{reciverAddress4}", distributor.Address4);
            document.ReplaceText("{city}", distributor.City);
            document.ReplaceText("{phoneNumber}", distributor.Telephone);
            
            Stream stream = new MemoryStream();
            document.SaveAs(stream);
            stream.Position = 0;
            return stream;
        }

        #endregion
    }
}
