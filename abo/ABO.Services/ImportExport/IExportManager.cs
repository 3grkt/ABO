using ABO.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace ABO.Services.ImportExport
{
    public interface IExportManager
    {
        byte[] ExportExcelFromDataTable(DataTable tbl, string sheetName = "Data", int[] columnWidths = null);
        byte[] GenerateDistributorUpdateExcel(IList<DistributorUpdate> distUpdates, DateTime startDate, DateTime endDate);
        byte[] ExportProfilesForDistributor(Distributor dist, IList<Profile> profiles);
        byte[] ExportProfileBoxes();
        byte[] GenerateProfileBoxDetailExcel(IList<Profile> profiles, ProfileBox boxDetail);
    }
}
