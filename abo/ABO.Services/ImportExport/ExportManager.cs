using ABO.Core;
using ABO.Core.Domain;
using Ionic.Zip;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace ABO.Services.ImportExport
{
    public class ExportManager : IExportManager
    {
        private readonly IExcelParamManager _excelParamManager;
        private readonly IWebHelper _webHelper;
        private readonly IAppSettings _appSettings;

        public ExportManager(IExcelParamManager excelParamManager, IWebHelper webHelper, IAppSettings appSettings)
        {
            _excelParamManager = excelParamManager;
            _webHelper = webHelper;
            _appSettings = appSettings;
        }

        #region IExportManager Members

        public byte[] ExportExcelFromDataTable(DataTable tbl, string sheetName = "Data", int[] columnWidths = null)
        {
            byte[] excelData = new byte[] { };

            // Generate excel
            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create the worksheet
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add(sheetName);

                //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                ws.Cells["A1"].LoadFromDataTable(tbl, true);

                // Set column width
                if (columnWidths != null)
                {
                    for (int i = 0; i < columnWidths.Length; i++)
                    {
                        ws.Column(i + 1).Width = columnWidths[i];
                    }
                }

                excelData = pck.GetAsByteArray();
            }
            return excelData;
        }

        public byte[] GenerateDistributorUpdateExcel(IList<DistributorUpdate> distUpdates, DateTime startDate, DateTime endDate)
        {
            byte[] excelData = new byte[] { };
            if (distUpdates.Count > 0)
            {
                using (var excelStream = new MemoryStream())
                {
                    using (var inputStream = new MemoryStream())
                    {
                        // Copy data to memory to avoid locking file for a long time
                        using (var stream = new FileStream(_webHelper.DistributorUpdateExcelTemplate, FileMode.Open))
                        {
                            stream.CopyTo(inputStream);
                        }

                        // Get excel param
                        var excelParams = _excelParamManager.GetExcelParams(ExcelExportType.DISTRIBUTOR_UPDATES);
                        if (excelParams == null)
                            throw new ABOException("Excel params are not found. Please check ExcelParams configuration!");

                        int tableStartRow = Convert.ToInt32(excelParams["TableStartRow"]);
                        string cellExportTime = excelParams["CellExportTime"];
                        string cellRecordCount = excelParams["CellRecordCount"];
                        string cellSearchDate = excelParams["CellSearchDate"];
                        int colDistNumber = Convert.ToInt32(excelParams["ColDistNumber"]);
                        int colDistName = Convert.ToInt32(excelParams["ColDistName"]);
                        int colJoinDate = Convert.ToInt32(excelParams["ColJoinDate"]);
                        int colExpiryDate = Convert.ToInt32(excelParams["ColExpiryDate"]);
                        int colUpdatedType = Convert.ToInt32(excelParams["ColUpdatedType"]);

                        using (var xlPackage = new ExcelPackage(excelStream, inputStream))
                        {
                            int row = tableStartRow;
                            var worksheet = xlPackage.Workbook.Worksheets.FirstOrDefault();

                            // Export time
                            worksheet.Cells[cellExportTime].Value = DateTime.Now.ToString(_webHelper.DateTimeFormat);
                            // Record count
                            worksheet.Cells[cellRecordCount].Value = distUpdates.Count;
                            // Search date
                            worksheet.Cells[cellSearchDate].Value = string.Format("{0} - {1}", _webHelper.GetDateString(startDate), _webHelper.GetDateString(endDate));

                            // Insert new empty row
                            worksheet.InsertRow(row, distUpdates.Count - 1, row);

                            foreach (var distUpdate in distUpdates)
                            {
                                worksheet.Cells[row, colDistNumber].Value = distUpdate.DistNumber;
                                worksheet.Cells[row, colDistName].Value = distUpdate.Distributor.Name;
                                worksheet.Cells[row, colJoinDate].Value = _webHelper.GetDateString(distUpdate.Distributor.JoinDate);
                                worksheet.Cells[row, colExpiryDate].Value = _webHelper.GetDateString(distUpdate.Distributor.ExpiryDate);
                                worksheet.Cells[row, colUpdatedType].Value = distUpdate.UpdatedType;

                                row++;
                            }

                            xlPackage.Save();
                        }
                        excelData = excelStream.ToArray();
                    }
                }
            }
            return excelData;
        }
        public byte[] GenerateProfileBoxDetailExcel(IList<Profile> profiles, ProfileBox boxDetail)
        {
            byte[] excelData = new byte[] { };

            using (var excelStream = new MemoryStream())
            {
                using (var inputStream = new MemoryStream())
                {
                    // Copy data to memory to avoid locking file for a long time
                    using (var stream = new FileStream(_webHelper.ProfileBoxDetailExcelTemplate, FileMode.Open))
                    {
                        stream.CopyTo(inputStream);
                    }

                    // Get excel param
                    var excelParams = _excelParamManager.GetExcelParams(ExcelExportType.PROFILEBOX_DETAIL);
                    if (excelParams == null)
                        throw new ABOException("Excel params are not found. Please check ExcelParams configuration!");

                    int tableStartRow = Convert.ToInt32(excelParams["TableStartRow"]);
                    string cellBoxName = excelParams["BoxName"];
                    string cellOffice = excelParams["Office"];
                    string cellWarehouse = excelParams["Warehouse"];
                    string cellLocation = excelParams["Location"];
                    string cellProfileCount = excelParams["ProfileCount"];
                    int colADA = Convert.ToInt32(excelParams["ADA"]);
                    int colScanedDate = Convert.ToInt32(excelParams["ScanedDate"]);
                    int colProfileStatus = Convert.ToInt32(excelParams["ProfileStatus"]);


                    using (var xlPackage = new ExcelPackage(excelStream, inputStream))
                    {
                        int row = tableStartRow;
                        var worksheet = xlPackage.Workbook.Worksheets.FirstOrDefault();

                        // Box Name
                        worksheet.Cells[cellBoxName].Value = boxDetail.Name;
                        // Office
                        worksheet.Cells[cellOffice].Value = boxDetail.Office.WarehouseName;
                        // Warehouse
                        worksheet.Cells[cellWarehouse].Value = boxDetail.Warehouse.WarehouseName;
                        // Location
                        worksheet.Cells[cellLocation].Value = (boxDetail.Location != null) ? boxDetail.Location.Name : "";
                        // profileCount
                        worksheet.Cells[cellProfileCount].Value = (boxDetail.ProfileCount == null) ? profiles.Count : boxDetail.ProfileCount;

                        // Insert new empty row
                        worksheet.InsertRow(row, profiles.Count - 1, row);

                        foreach (var profile in profiles)
                        {
                            worksheet.Cells[row, colADA].Value = profile.DistNumber;
                            worksheet.Cells[row, colScanedDate].Value = profile.ScannedDate.Value.ToString(_webHelper.DateTimeFormat);
                            worksheet.Cells[row, colProfileStatus].Value = profile.Status.Name;
                            row++;
                        }

                        xlPackage.Save();
                    }
                    excelData = excelStream.ToArray();
                }
            }

            return excelData;
        }

        public byte[] ExportProfilesForDistributor(Distributor dist, IList<Profile> profiles)
        {
            var exportData = new byte[] { };

            var tempProfileFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempProfileFolder);
            //var tempProfileFolder = Path.Combine(tempFolder, dist.DistNumber.ToString());
            //Directory.CreateDirectory(tempProfileFolder);

            // Copy files
            foreach (var profile in profiles)
            {
                var profileFilePath = Path.Combine(_appSettings.ProfileBoxFolder, profile.ProfileBox.Name + "\\" + profile.FileName);
                var newFilePath = Path.Combine(tempProfileFolder,
                    string.Format("{0}_{1}{2}",
                        profile.CreatedDate.ToString(_appSettings.ProfileExportDateFormat),
                        profile.ProfileType.Name.Replace(' ', '_'),
                        Path.GetExtension(profileFilePath)));

                File.Copy(profileFilePath, UnicodeCharacterReplacer.ReplaceString(newFilePath), true);
            }

            var zippedFiles = profiles.Select(x => Path.Combine(_appSettings.ProfileBoxFolder, x.ProfileBox.Name + "\\" + x.FileName));

            // Zip files
            using (var zipFile = new ZipFile(Encoding.UTF8))
            {
                using (var memoryStream = new MemoryStream())
                {
                    zipFile.AddDirectory(tempProfileFolder, string.Empty);
                    //zipFile.AddFiles(zippedFiles, string.Empty);
                    zipFile.Save(memoryStream);
                    exportData = memoryStream.ToArray();
                }
            }

            // Delete temp folder
            try
            {
                Directory.Delete(tempProfileFolder, true);
            }
            catch { }

            return exportData;
        }

        #endregion


        public byte[] ExportProfileBoxes()
        {
            throw new NotImplementedException();
        }
    }
}
