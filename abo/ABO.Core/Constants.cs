using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABO.Core
{
    public struct FileExtension
    {
        public const string EXCEL_2003 = ".xls";
        public const string EXCEL = ".xlsx";
        public const string ZIP = ".zip";
        public const string PDF = ".pdf";
    }

    public struct FileContentType
    {
        public const string EXCEL = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public const string WORD = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        public const string ZIP = "application/x-compressed";
        public const string PDF = "application/pdf";
    }

    public struct ExcelExportType
    {
        //public const string ASSET = "Asset";
        //public const string INVENTORY = "Inventory";
        //public const string INVENTORY_CLOSE = "InventoryClose";
        public const string DISTRIBUTOR_UPDATES = "DistributorUpdates";
        public const string PROFILEBOX_DETAIL = "ProfileBoxDetail";
    }

    public struct TableName
    {
        public const string Profile = "tblProfile";
        public const string ProfileBox = "tblProfileBox";
    }

    /// <summary>
    /// Type of distributor update defined in AS400.
    /// </summary>
    public struct DistUpdateType
    {
        /// <summary>
        /// Join
        /// </summary>
        public static readonly string[] Joins = new string[] { "A" };
        /// <summary>
        /// Renewal
        /// </summary>
        public static readonly string[] Renewals = new string[] { "G", "R" };
        /// <summary>
        /// Info update
        /// </summary>
        public static readonly string[] InfoUpdates = new string[] { "K", "M", "N", "P", "X", "Y" };
    }

    /// <summary>
    /// Contains type of help file.
    /// </summary>
    public struct HelpFileType
    {
        public const string DR = "DR";
        public const string Guess = "Guess";
        public const string DRLeader = "DRLeader";
    }

    /// <summary>
    /// Contains name of caches used in system.
    /// </summary>
    public struct CacheName
    {
        public const string DistributorUpdateTypes = "DistributorUpdateTypes";
        public const string Warehouses = "Warehouses";
    }

    public struct WtaImageExtension
    {
        public const string Jpeg = "jpeg";
        public const string Png = "png";
    }

    public enum AboExceptionType
    {
        Info = 0,
        Warning,
        Error
    }
}
