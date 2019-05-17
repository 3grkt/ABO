using ABO.Core;
using ABO.Core.Data;
using ABO.Core.Domain;
using ABO.Core.SearchCriteria;
using ABO.Data;
using ABO.Services.Distributors;
using ABO.Services.Localization;
using ABO.Services.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ABO.Services.Profiles
{
    public class ProfileService : ServiceBase, IProfileService
    {
        private const string PROFILE_DUPLICATE_LOG = "DUPLICATED FROM PROFILE WITH ID = {0}";
        private const string PROFILE_SCAN_LOG = "SCANNED BY AUTO JOB";
        private const string PROFILE_NEEDTODELETE_LOG = "MARKED 'NEED TO DELETE' BY AUTO JOB";
        private const string PROFILE_DELETEFROMDIST_LOG = "PROFILE ({0}) DELETED FROM DISTRIBUTOR {1}";
        private const string PROFILEBOX_NEEDTODISCARD_LOG = "MARKED 'NEED TO DISCARD' BY AUTO JOB";
        private const string PROFILEBOX_DISCARDABLE_LOG = "MARKED 'DISCARDABLE' BY AUTO JOB";

        private const string PROFILESCAN_FOLDER_NOTFOUND = "Không tìm thấy folder của thùng đang mở";
        private const string PROFILESCAN_FOLDER_NOPERMISSION = "User không có quyền move file";
        private const string PROFILESCAN_MOVEFILE_ERROR = "Lỗi khi chép file: {0}";
        private const string PROFILESCAN_DISTRIBUTOR_NOTFOUND = "Không tìm thấy thông tin NPP: {0}";
        private const string PROFILESCAN_JOINPROFILE_EXISTED = "NPP đã có đơn gia nhập: {0}";
        private const string PROFILESCAN_ADA_DUPLICATE = "ADA bị trùng: {0}";
        private const string PROFILESCAN_PACKED_FOLDER_NOTEMPTY = "Đóng thùng rồi nhưng folder còn file";
        private const string PROFILESCAN_PACKED_FOLDER_DELETED = "Xóa folder đã đóng gói";
        private const string AUTO_JOB_USER = "AutoJob";

        private const int JOIN_PROFILETYPE = 1; // Loại "Đơn gia nhập"
        private const string PROFILE_PREFIX_TIME_FORMAT = "yyyyMMddHHmmss";

        private readonly IDbContext _dbContext;
        private readonly IRepository<ProfileBox> _profileBoxRepository;
        private readonly IRepository<Profile> _profileRepository;
        private readonly IRepository<ProfileScan> _profileScanRepository;
        private readonly IDataHelper _dataHelper;
        private readonly IAppSettings _appSettings;
        private readonly ILogger _logger;
        private readonly IDistributorService _distributorService;
        //private readonly IResourceManager _resourceManager;

        public ProfileService(
            IDbContext dbContext,
            IRepository<DataLog> dataLogRepository,
            IRepository<ProfileBox> profileBoxRepository,
            IRepository<Profile> profileRepository,
            IRepository<ProfileScan> profileScanRepository,
            IDataHelper dataHelper,
            IAppSettings appSettings,
            ILogger logger,
            IDistributorService distributorService)
            : base(dataLogRepository)
        {
            this._dbContext = dbContext;
            this._profileBoxRepository = profileBoxRepository;
            this._profileRepository = profileRepository;
            this._profileScanRepository = profileScanRepository;
            this._dataHelper = dataHelper;
            this._appSettings = appSettings;
            this._logger = logger;
            this._distributorService = distributorService;
        }

        #region Profile
        public Profile GetProfileById(int id, bool includeProfileBox = false, bool includeProfileType = false)
        {
            var query = _profileRepository.Table;
            if (includeProfileBox)
                query = query.IncludeTable(x => x.ProfileBox);
            if (includeProfileType)
                query = query.IncludeTable(x => x.ProfileType);

            return query.FirstOrDefault(x => x.Id == id);
        }

        public IList<Profile> GetProfilesByIds(IEnumerable<int> ids, bool tracking = false)
        {
            var query = tracking ? _profileRepository.Table : _profileRepository.TableNoTracking;
            query = query.Where(x => ids.Contains(x.Id));
            return query.ToList();
        }

        public void ChangeProfileBox(Profile profile, int newBoxId, string logUser)
        {
            // Duplicate new profile
            var newProfile = profile.Clone() as Profile;
            newProfile.BoxId = newBoxId;
            newProfile.CreatedDate = DateTime.Now;
            _profileRepository.Insert(newProfile);
            CreateLogRecord(TableName.Profile, "Id", newProfile.Id, null, newProfile.Id, logUser, string.Format(PROFILE_DUPLICATE_LOG, profile.Id));

            // Update current profile
            var currentStatusId = profile.StatusId;
            profile.StatusId = (short)ProfileStatus.Deleted;
            _profileRepository.Update(profile);
            CreateLogRecord(TableName.Profile, "StatusId", profile.Id, currentStatusId, profile.StatusId, logUser);
        }

        public void MoveProfile(Profile profile, int newBoxId)
        {
            profile.BoxId = newBoxId;
            _profileRepository.Update(profile);
        }

        public void ChangeADA(Profile profile, Distributor newDist, string logUser)
        {
            // Duplicate new profile
            var newProfile = profile.Clone() as Profile;
            newProfile.DistNumber = newDist.DistNumber;
            newProfile.CreatedDate = DateTime.Now;
            _profileRepository.Insert(newProfile);
            CreateLogRecord(TableName.Profile, "Id", newProfile.Id, null, newProfile.Id, logUser, string.Format(PROFILE_DUPLICATE_LOG, profile.Id));

            // Update current profile
            var currentStatusId = profile.StatusId;
            profile.StatusId = (short)ProfileStatus.Deleted;
            _profileRepository.Update(profile);
            CreateLogRecord(TableName.Profile, "StatusId", profile.Id, currentStatusId, profile.StatusId, logUser);
        }

        public void DeleteProfiles(Distributor distributor, IEnumerable<int> profileIds, string logUser)
        {
            var profiles = this.GetProfilesByIds(profileIds).Where(x => x.StatusId != (short)ProfileStatus.Deleted); // only retrieve not-deleted profiles

            // update profile status
            if (profileIds.Any()) // WARINING: there is a known issue with BulkUpdate so need to check before update
            {
                _profileRepository.BulkUpdate(
                    f => profileIds.Contains(f.Id),
                    u => new Profile() { StatusId = (short)ProfileStatus.Deleted });
            }

            // logging
            foreach (var p in profiles)
            {
                CreateLogRecord(TableName.Profile, "StatusId", p.Id, p.StatusId, (short)ProfileStatus.Deleted, logUser);
            }

            // Update counts on profileBox
            UpdateBoxCount(profiles.Select(x => x.BoxId ?? 0), logUser, string.Format(PROFILE_DELETEFROMDIST_LOG, string.Join(",", profileIds), distributor.DistNumber));
        }

        public IPagedList<Profile> SearchProfilesByDistNumber(long distNumber, int page, int pageSize, string sortCol, string sortDir)
        {
            var query = _profileRepository.Table
                .IncludeTable(x => x.Warehouse)
                .IncludeTable(x => x.ProfileType)
                .IncludeTable(x => x.ProfileBox)
                .IncludeTable(x => x.Status)
                .IncludeTable(x => x.User);

            query = query.Where(x => x.DistNumber == distNumber);

            if (string.IsNullOrEmpty(sortCol))
                query = query.OrderBy(x => x.CreatedDate);
            else
                query = query.SortBy(sortCol + " " + sortDir);

            return new PagedList<Profile>(query, page, pageSize);
        }

        public IList<Profile> GetDistributorProfilesForExport(long distNumber)
        {
            var query = _profileRepository.TableNoTracking
                            .IncludeTable(x => x.ProfileBox)
                            .IncludeTable(x => x.ProfileType);
            query = query.Where(x => x.DistNumber == distNumber);
            return query.ToList();
        }

        public IList<Profile> GetProfilesForDataPurge(DateTime purgeStart, DateTime purgeEnd)
        {
            var query = _profileRepository.Table.IncludeTable(x => x.ProfileBox);
            query = query.Where(x => x.StatusId == (short)ProfileStatus.Deleted && x.CreatedDate >= purgeStart && x.CreatedDate <= purgeEnd);
            return query.ToList();
        }

        #endregion

        #region ProfileBox
        public IList<ProfileBox> GetAvailableProfileBoxes()
        {
            var query = _profileBoxRepository.TableNoTracking;
            query = query.Where(x => x.StatusId == (short)ProfileBoxStatus.Open);
            return query.ToList();
        }

        public IList<ProfileBox> GetProfileBoxesForDataPurge(DateTime purgeStart, DateTime purgeEnd)
        {
            var query = _profileBoxRepository.Table
                            .IncludeTable(x => x.Profiles)
                            .IncludeTable(x => x.ProfileScans);
            query = query.Where(x => x.StatusId == (short)ProfileBoxStatus.Discarded && x.CreatedDate >= purgeStart && x.CreatedDate <= purgeEnd);
            return query.ToList();
        }

        public void UpdateBoxCount(IEnumerable<int> boxIds, string logUser, string logInfo)
        {
            var boxes = _profileBoxRepository.Table
                .IncludeTable(x => x.Profiles)
                .Where(x => boxIds.Contains(x.Id));

            foreach (var box in boxes)
            {
                var oldProfileCount = box.ProfileCount;
                var oldADACount = box.ADACount;

                box.ProfileCount = box.Profiles.Count(x => x.StatusId != (short)ProfileStatus.Deleted);
                box.ADACount = box.Profiles.Where(x => x.StatusId != (short)ProfileStatus.Deleted).Select(x => x.DistNumber).Distinct().Count();
                _profileBoxRepository.Update(box);

                CreateLogRecord(TableName.ProfileBox, "ProfileCount", box.Id, oldProfileCount, box.ProfileCount, logUser, logInfo);
                CreateLogRecord(TableName.ProfileBox, "ADACount", box.Id, oldADACount, box.ADACount, logUser, logInfo);
            }
        }
        #endregion

        #region ProfileScan

        public IPagedList<ProfileScan> SearchProfileScans(ProfileScanSearchCriteria criteria, int page, int pageSize, string sortCol, string sortDir)
        {
            var query = _profileScanRepository.TableNoTracking
                            .IncludeTable(x => x.ProfileBox);

            if (criteria.StartDate.HasValue)
                query = query.Where(x => x.ScannedDate >= criteria.StartDate);
            if (criteria.EndDate.HasValue)
                query = query.Where(x => x.ScannedDate <= criteria.EndDate);
            if (!string.IsNullOrEmpty(criteria.WarehouseId))
                query = query.Where(x => x.WarehouseId == criteria.WarehouseId);
            if (criteria.Result != 0)
                query = query.Where(x => x.Result == criteria.Result);

            if (string.IsNullOrEmpty(sortCol))
                query = query.OrderByDescending(x => x.ScannedDate);
            else
                query = query.SortBy(sortCol + " " + sortDir);

            return new PagedList<ProfileScan>(query, page, pageSize);
        }

        public IList<ProfileScan> GetProfileScansForDataPurge(DateTime purgeStart, DateTime purgeEnd)
        {
            var query = _profileScanRepository.Table;
            query = query.Where(x => x.ScannedDate >= purgeStart && x.ScannedDate <= purgeEnd);
            return query.ToList();
        }
        #endregion

        #region Auto Jobs
        public void ScanAndCopyProfiles()
        {
            Console.WriteLine("Profile Box folder: " + _appSettings.ProfileBoxFolder);
            // Get scannable boxes
            var boxes = GetScannableBoxes();
            foreach (var box in boxes.Where(x => !string.IsNullOrEmpty(x.ScannedFolder))) // only scan box whose ScannedFolder was configured
            {
                if (Directory.Exists(box.ScannedFolder))
                {
                    if (CommonHelper.HasPermissionOnFolder(box.ScannedFolder, System.Security.AccessControl.FileSystemRights.Modify))
                    {
                        _logger.WriteLog(string.Format("Scanning box \"{0}\" - folder: \"{1}\"", box.Name, box.ScannedFolder), LogLevel.Info);
                        var scannedFilePaths = Directory.GetFiles(box.ScannedFolder);
                        if (scannedFilePaths.Length > 0)
                        {
                            _logger.WriteLog(string.Format("Found {1} file(s) in folder: {0}", box.ScannedFolder, scannedFilePaths.Length), LogLevel.Debug);

                            var scanResult = ProfileScanResult.OK;
                            var sbMoveFileError = new StringBuilder();
                            var sbDistNotFound = new StringBuilder();
                            var joinProfileExistedDists = new List<long>();
                            var scannedDistributorUpdateIds = new List<int>();
                            var movedFileList = new List<Tuple<string, string>>();

                            if (box.Profiles == null)
                                box.Profiles = new List<Profile>();

                            foreach (var filePath in scannedFilePaths)
                            {
                                #region Move file and create new profile
                                var fileName = Path.GetFileName(filePath);

                                var distNumber = GetDistNumberFromFileName(fileName, (SystemProfileBoxType)box.TypeId);
                                if (distNumber > 0)
                                {
                                    if (_distributorService.IsDistributorExisted(distNumber))
                                    {
                                        // Check if 'join' profile existed for this distributor
                                        if (IsJoinProfileExisted(distNumber, box.TypeId))
                                        {
                                            joinProfileExistedDists.Add(distNumber);
                                        }
                                        else
                                        {
                                            Profile profile = null;
                                            try
                                            {
                                                // Enqueue file
                                                var copiedFileName = GetCopiedProfileFileName(fileName);
                                                movedFileList.Add(new Tuple<string, string>(filePath, copiedFileName));

                                                // Create new profile
                                                profile = new Profile()
                                                {
                                                    CreatedDate = DateTime.Now,
                                                    DistNumber = distNumber,
                                                    StatusId = (short)ProfileStatus.Valid,
                                                    TypeId = box.TypeId,
                                                    WarehouseId = box.WarehouseId,
                                                    FileName = copiedFileName,
                                                    ScannedDate = DateTime.Now
                                                };
                                                box.Profiles.Add(profile);

                                                // Find and update DistributorUpdate record
                                                var distUpdates = _distributorService.SearchDistributorUpdates(new DistributorUpdateSearchCriteria { DistNumber = distNumber, Statuses = new[] { (short)DistributorUpdateStatus.NotCompleted } }, 1, 100, null, null);
                                                if (distUpdates.Count > 0)
                                                {
                                                    var distUpdateIds = distUpdates.Select(x => x.Id);
                                                    scannedDistributorUpdateIds.AddRange(distUpdateIds);
                                                    _logger.WriteLog(string.Format("Found {0} DistributorUpdate record(s) with id {1} for distributor {2}", distUpdates.Count, string.Join(", ", distUpdateIds), distNumber), LogLevel.Info);
                                                }

                                                _logger.WriteLog("Success to enqueue file: " + fileName, LogLevel.Info);
                                            }
                                            catch (Exception ex)
                                            {
                                                if (profile != null)
                                                    box.Profiles.Remove(profile);

                                                sbMoveFileError.AppendFormat(", {0}", fileName);
                                                scanResult = ProfileScanResult.Error;
                                                _logger.WriteLog(string.Format("Failed to enqueue file: {0}", fileName), ex);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        sbDistNotFound.AppendFormat(", {0}", distNumber);
                                    }
                                }
                                #endregion
                            }

                            // build description string
                            var description =
                                (sbMoveFileError.Length > 0 ? string.Format(PROFILESCAN_MOVEFILE_ERROR, sbMoveFileError.Remove(0, 2)) : string.Empty) +
                                (sbDistNotFound.Length > 0 ? "\r\n" + string.Format(PROFILESCAN_DISTRIBUTOR_NOTFOUND, sbDistNotFound.Remove(0, 2)) : string.Empty) +
                                (joinProfileExistedDists.Any() ? "\r\n" + string.Format(PROFILESCAN_JOINPROFILE_EXISTED, string.Join(", ", joinProfileExistedDists)) : string.Empty);

                            // Log to ProfileScan table
                            var profileScan = new ProfileScan()
                            {
                                WarehouseId = box.WarehouseId,
                                FileCount = scannedFilePaths.Length,
                                Result = (short)scanResult,
                                ScannedDate = DateTime.Now,
                                Description = description
                            };
                            box.ProfileScans.Add(profileScan);
                            box.UpdatedDate = DateTime.Now;

                            // Update database
                            using (var dbTransaction = _dbContext.BeginDbTransaction())
                            {
                                try
                                {
                                    _profileBoxRepository.Update(box);
                                    UpdateProfileAndADACount(box);

                                    // Write log for each profile after Id is generated
                                    foreach (var profile in box.Profiles)
                                    {
                                        CreateLogRecord("tblProfile", "Id", profile.Id, null, profile.Id, AUTO_JOB_USER, PROFILE_SCAN_LOG);
                                    }

                                    // Update DistributorUpdate table
                                    _distributorService.MarkDistributorUpdatesCompleted(scannedDistributorUpdateIds);

                                    dbTransaction.Commit();

                                    // Move files only after all data are saved to DB successfully
                                    string fileMoveMessage;
                                    MoveProfileFiles(movedFileList, Path.Combine(_appSettings.ProfileBoxFolder, box.Name), out fileMoveMessage);
                                    // Update profile description if any
                                    if (!string.IsNullOrEmpty(fileMoveMessage))
                                    {
                                        profileScan.Description = (string.IsNullOrEmpty(profileScan.Description) ? "" : "\r\n") + fileMoveMessage;
                                        _profileScanRepository.Update(profileScan);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    dbTransaction.Rollback();
                                    _logger.WriteLog(string.Format("Failed to update box \"{0}\" in database.", box.Name), ex);
                                }
                            }
                        }
                        else
                        {
                            _logger.WriteLog(string.Format("No profile found in folder: {0}", box.ScannedFolder), LogLevel.Debug);
                        }
                    }
                    else
                    {
                        // Record error
                        _profileScanRepository.Insert(new ProfileScan()
                        {
                            BoxId = box.Id,
                            WarehouseId = box.WarehouseId,
                            FileCount = 0,
                            Result = (short)ProfileScanResult.Error,
                            ScannedDate = DateTime.Now,
                            Description = PROFILESCAN_FOLDER_NOPERMISSION
                        });
                    }
                }
                else
                {
                    // Record error
                    _profileScanRepository.Insert(new ProfileScan()
                    {
                        BoxId = box.Id,
                        WarehouseId = box.WarehouseId,
                        FileCount = 0,
                        Result = (short)ProfileScanResult.Error,
                        ScannedDate = DateTime.Now,
                        Description = PROFILESCAN_FOLDER_NOTFOUND
                    });
                }
            }
        }

        public void ScanAndUpdateProfileStatus()
        {
            var profiles = GetNeedToDeleteProfiles();
            foreach (var profile in profiles)
            {
                var originalStatusId = profile.StatusId;
                profile.StatusId = (short)ProfileStatus.NeedToDelete;

                using (var dbTransaction = _dbContext.BeginDbTransaction())
                {
                    try
                    {
                        _profileRepository.Update(profile);
                        CreateLogRecord("tblProfile", "StatusId", profile.Id, originalStatusId, profile.StatusId, AUTO_JOB_USER, PROFILE_NEEDTODELETE_LOG);

                        dbTransaction.Commit();
                        _logger.WriteLog("Success to mark 'need to delete' for profile with PK=" + profile.Id, LogLevel.Info);
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        _logger.WriteLog("Failed to mark 'need to delete' for profile with Id=" + profile.Id, ex);
                    }
                }
            }
        }

        public void ScanAndUpdateBoxStatus()
        {
            var needToDeleteBoxes = GetNeedToDeleteProfileBoxes();
            UpdateProfileBoxStatusWithLog(needToDeleteBoxes, ProfileBoxStatus.NeedToDiscard, PROFILEBOX_NEEDTODISCARD_LOG, "need to discard");

            var notScannedBoxes = GetNotScannedProfileBoxesOlderThanStoredYears();
            UpdateProfileBoxStatusWithLog(notScannedBoxes, ProfileBoxStatus.NeedToDiscard, PROFILEBOX_NEEDTODISCARD_LOG, "need to discard");

            var discardableBoxes = GetDiscardableProfileBoxes();
            UpdateProfileBoxStatusWithLog(discardableBoxes, ProfileBoxStatus.Discardable, PROFILEBOX_DISCARDABLE_LOG, "discardable");
        }

        public void ScanAndDeletePackedFolders()
        {
            var packedBoxes = GetPackedBoxes();
            foreach (var box in packedBoxes.Where(x => !string.IsNullOrEmpty(x.ScannedFolder))) // only scan box whose ScannedFolder was configured
            {
                if (Directory.Exists(box.ScannedFolder))
                {
                    try
                    {
                        short scanResult;
                        string scanDescription;

                        if (Directory.EnumerateFileSystemEntries(box.ScannedFolder).Any())
                        {
                            scanResult = (short)ProfileScanResult.Error;
                            scanDescription = PROFILESCAN_PACKED_FOLDER_NOTEMPTY;
                        }
                        else
                        {
                            scanResult = (short)ProfileScanResult.OK;
                            scanDescription = PROFILESCAN_PACKED_FOLDER_DELETED;

                            Directory.Delete(box.ScannedFolder); // delete empty folder

                            _logger.WriteLog("Success to delete packed folder - BoxId = " + box.Id);
                        }

                        // Create profileScan record and update db
                        box.ProfileScans.Add(new ProfileScan()
                        {
                            WarehouseId = box.WarehouseId,
                            FileCount = 0,
                            ScannedDate = DateTime.Now,
                            Result = scanResult,
                            Description = scanDescription
                        });
                        _profileBoxRepository.Update(box); // update database
                        _logger.WriteLog("Success to save log - BoxId = " + box.Id);
                    }
                    catch (Exception ex)
                    {
                        _logger.WriteLog("Failed to delete packed folder - BoxId = " + box.Id, ex);
                    }
                }
            }
        }
        #endregion

        #region Utility
        /// <summary>
        /// Gets scannable profile boxes.
        /// </summary>
        /// <returns></returns>
        private List<ProfileBox> GetScannableBoxes()
        {
            var query = _profileBoxRepository.Table;
            query = query.Where(x => x.StatusId == (short)ProfileBoxStatus.Open && x.ProfileType.Scanned);
            return query.ToList();
        }

        /// <summary>
        /// Gets packed profile boxes.
        /// </summary>
        /// <returns></returns>
        private List<ProfileBox> GetPackedBoxes()
        {
            return _profileBoxRepository.Table.Where(x => x.StatusId == (short)ProfileBoxStatus.Packed).ToList();
        }

        /// <summary>
        /// Gets profiles that need to be deleted.
        /// </summary>
        /// <returns></returns>
        private List<Profile> GetNeedToDeleteProfiles()
        {
            return _dataHelper.GetNeedToDeleteProfiles(_appSettings.ExpiredDistributorDeletionPeriod).ToList();
        }

        private void UpdateProfileBoxStatusWithLog(IList<ProfileBox> profileBoxes, ProfileBoxStatus status, string logMessage, string logStatus)
        {
            foreach (var box in profileBoxes)
            {
                var originalStatus = box.StatusId;
                box.StatusId = (short)status;
                box.UpdatedDate = DateTime.Now;
                using (var dbTransaction = _dbContext.BeginDbTransaction())
                {
                    try
                    {
                        _profileBoxRepository.Update(box);
                        CreateLogRecord("tblProfileBox", "StatusId", box.Id, originalStatus, box.StatusId, AUTO_JOB_USER, logMessage);

                        dbTransaction.Commit();
                        _logger.WriteLog("Success to mark '" + logStatus + "' for profile box with Id=" + box.Id, LogLevel.Info);
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        _logger.WriteLog("Failed to mark '" + logStatus + "' for profile box with Id=" + box.Id, ex);
                    }
                }
            }
        }

        private List<ProfileBox> GetNeedToDeleteProfileBoxes()
        {
            var query = _profileBoxRepository.Table;
            query = query.Where(x =>
                (x.StatusId == (short)ProfileBoxStatus.Stored || x.StatusId == (short)ProfileBoxStatus.Packed) &&
                x.Profiles.Count > 0 &&
                !x.Profiles.Any(p => p.StatusId != (short)ProfileStatus.NeedToDelete));
            return query.ToList();
        }

        private List<ProfileBox> GetNotScannedProfileBoxesOlderThanStoredYears()
        {
            return _dataHelper.GetNotScannedProfileBoxesOlderThanStoredYears().ToList();
        }

        private List<ProfileBox> GetDiscardableProfileBoxes()
        {
            var discardableBoxes = new List<ProfileBox>();
            var query = _profileBoxRepository.Table;
            foreach (var box in query.IncludeTable(x => x.Profiles).Where(x => x.StatusId == (short)ProfileBoxStatus.Stored || x.StatusId == (short)ProfileBoxStatus.Packed))
            {
                var totalProfiles = box.Profiles.Count;
                var toDeleteProfiles = box.Profiles.Where(x => x.StatusId == (short)ProfileStatus.NeedToDelete).Count();
                if ((float)toDeleteProfiles / totalProfiles >= 0.5f)
                {
                    discardableBoxes.Add(box);
                }
            }
            return discardableBoxes;
        }

        private long GetDistNumberFromFileName(string fileName, SystemProfileBoxType type)
        {
            int tempNumber;
            string sNumber;

            // If fileName contains a "delimiter" (e.g. _ or -) split, then try parse
            char delimiter = _appSettings.DistributorDelimiterInProfileFile.FirstOrDefault(c => fileName.Contains(c));
            if (delimiter != char.MinValue)
            {
                sNumber = fileName.Substring(0, fileName.IndexOf(delimiter));
                if (int.TryParse(sNumber, out tempNumber))
                    return tempNumber;
            }

            // Remove extension, then parse
            sNumber = Path.GetFileNameWithoutExtension(fileName);
            if (int.TryParse(sNumber, out tempNumber))
                return tempNumber;

            return 0;
        }

        private void MoveProfileFile(string filePath, string destFolder)
        {
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);

            File.Move(filePath, Path.Combine(destFolder, Path.GetFileName(filePath)));
        }

        private void MoveProfileFiles(List<Tuple<string, string>> files, string destFolder, out string logMessage)
        {
            logMessage = string.Empty;
            var duplicatedADAs = new List<string>();

            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);

            var movedFileTuples = new List<Tuple<string, string>>();
            foreach (var item in files)
            {
                var filePath = item.Item1;
                try
                {
                    string destPath = Path.Combine(destFolder, item.Item2);

                    if (!File.Exists(destPath))
                    {
                        File.Move(filePath, destPath);
                        movedFileTuples.Add(new Tuple<string, string>(filePath, destPath));
                    }
                    else
                    {
                        duplicatedADAs.Add(Path.GetFileNameWithoutExtension(filePath));
                    }
                }
                catch (Exception ex)
                {
                    _logger.WriteLog("Failed to move profile files. All moved file(s) will be rolled back.", ex);

                    // Rollback moved files
                    foreach (var tuple in movedFileTuples)
                    {
                        try
                        {
                            File.Move(tuple.Item2, tuple.Item1);
                        }
                        catch { }
                    }
                    break;
                }
            }

            if (duplicatedADAs.Any())
            {
                logMessage = string.Format(PROFILESCAN_ADA_DUPLICATE, string.Join(", ", duplicatedADAs));
            }
        }

        private void UpdateProfileAndADACount(ProfileBox box)
        {
            box.ProfileCount = _profileRepository.TableNoTracking.Count(x => x.BoxId == box.Id && x.StatusId != (short)ProfileStatus.Deleted);
            box.ADACount = _profileRepository.TableNoTracking.Where(x => x.BoxId == box.Id && x.StatusId != (short)ProfileStatus.Deleted)
                                             .Select(x => x.DistNumber).Distinct().Count();
            _profileBoxRepository.Update(box);
        }

        public IPagedList<Profile> SearchProfilesByStatus(int profileBoxId, int statusId, int page, int pageSize, string sortCol, string sortDir)
        {
            var query = _profileRepository.Table
               .IncludeTable(x => x.Status);
            if (statusId != 0)
                query = query.Where(x => x.StatusId == statusId);
            query = query.Where(x => x.BoxId == profileBoxId);

            if (string.IsNullOrEmpty(sortCol))
                query = query.OrderBy(x => x.CreatedDate);
            else
                query = query.SortBy(sortCol + " " + sortDir);

            return new PagedList<Profile>(query, page, pageSize);
        }

        private bool IsJoinProfileExisted(long distNumber, int typeId)
        {
            return typeId == JOIN_PROFILETYPE && _profileRepository.TableNoTracking.Any(x => x.DistNumber == distNumber && x.TypeId == JOIN_PROFILETYPE);
        }

        private string GetCopiedProfileFileName(string profileFile)
        {
            return string.Format("{0}_{1}{2}",
                Path.GetFileNameWithoutExtension(profileFile),
                DateTime.Now.ToString(PROFILE_PREFIX_TIME_FORMAT),
                Path.GetExtension(profileFile));
        }
        #endregion

    }
}
