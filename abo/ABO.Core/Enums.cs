
namespace ABO.Core
{
    /// <summary>
    /// Status of profile.
    /// </summary>
    public enum ProfileStatus : short
    {
        /// <summary>
        /// Hieu luc
        /// </summary>
        Valid = 101,
        /// <summary>
        /// Da xoa
        /// </summary>
        Deleted = 102,
        /// <summary>
        /// Can xoa
        /// </summary>
        NeedToDelete = 103,
    }

    /// <summary>
    /// Status of profile box.
    /// </summary>
    public enum ProfileBoxStatus : short
    {
        /// <summary>
        /// Dang mo
        /// </summary>
        Open = 201,
        /// <summary>
        /// Da dong goi
        /// </summary>
        Packed = 202,
        /// <summary>
        /// Da di chuyen
        /// </summary>
        Moved = 203,
        /// <summary>
        /// Luu kho
        /// </summary>
        Stored = 204,
        /// <summary>
        /// Co the huy
        /// </summary>
        Discardable = 205,
        /// <summary>
        /// Can huy
        /// </summary>
        NeedToDiscard = 206,
        /// <summary>
        /// Da huy
        /// </summary>
        Discarded = 207,
    }

    /// <summary>
    /// Status of distributor log
    /// </summary>
    public enum DistributorLogStatus : short
    {
        /// <summary>
        /// Chua scan
        /// </summary>
        NotScanned = 301,
        /// <summary>
        /// Da scan
        /// </summary>
        Scanned = 302,
        /// <summary>
        /// Khong can scan
        /// </summary>
        Skip = 303,
    }

    /// <summary>
    /// Status of distributorUpdate record.
    /// </summary>
    public enum DistributorUpdateStatus : short
    {
        NotCompleted = 401,
        Completed = 402,
    }

    public enum SystemProfileBoxType : short
    {

    }

    /// <summary>
    /// Result of profile scan.
    /// </summary>
    public enum ProfileScanResult : short
    {
        None = 0,
        OK = 1,
        Error = 2
    }

    /// <summary>
    /// Roles of user. Value is similar to PK of tblRole.
    /// </summary>
    public enum UserRole : short
    {
        Guess = 1,
        Scanner = 2,
        TeamLeader = 3,
        IT = 4,
    }

    /// <summary>
    /// Permission of user. Value is similar to PK of tblPermisison.
    /// </summary>
    public enum UserPermission : short
    {
        /// <summary>
        /// Quan ly loai ho so
        /// </summary>
        ManageProfileType = 1,
        /// <summary>
        /// Quan ly thung ho so
        /// </summary>
        ManageProfileBox = 2,
        /// <summary>
        /// Xem ket qua hang ngay
        /// </summary>
        ViewProfileScanResult = 3,
        /// <summary>
        /// Xem chi tiet thung ho so
        /// </summary>
        ViewProfileBoxDetails = 4,
        /// <summary>
        /// Quan ly ho so NPP
        /// </summary>
        ManageDistributorProfile = 5,
        /// <summary>
        /// Xem thong tin cap nhat NPP
        /// </summary>
        ViewDistributorUpdate = 6,
        /// <summary>
        /// Quan ly user
        /// </summary>
        ManageUser = 7,
        /// <summary>
        /// Xoa du lieu dinh ky
        /// </summary>
        PurgeData = 8,
        /// <summary>
        /// In thu cho NPP
        /// </summary>
        PrintDistributorLetter = 9,
        /// <summary>
        /// Xem ho so NPP
        /// </summary>
        ViewDistributorProfile = 10,
        /// <summary>
        /// Tao thung HS
        /// </summary>
        CreateProfileBox = 11,
    }

    /// <summary>
    /// Level of logging.
    /// </summary>
    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error,
    }
}
