using ABO.Core;
using ABO.Services.Logging;
using ABO.Services.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABO.Jobs.Workers
{
    public class ProfileScanWorker : IWorker
    {
        private readonly IProfileService _profileService;
        private readonly ILogger _logger;
        private readonly IAppSettings _appSettings;

        public ProfileScanWorker(IProfileService profileService, ILogger logger, IAppSettings appSettings)
        {
            this._profileService = profileService;
            this._logger = logger;
            this._appSettings = appSettings;
        }

        #region IWorker Members

        public void DoWork()
        {
            _logger.WriteLog("======================= Running ProfileScanWorker =======================", LogLevel.Info);

            // Copy profiles
            _logger.WriteLog("***Start copying profiles");
            _profileService.ScanAndCopyProfiles();
            _logger.WriteLog("***End copying profiles");

            // Update profile status
            _logger.WriteLog("***Start updating profile status");
            _profileService.ScanAndUpdateProfileStatus();
            _logger.WriteLog("***End updating profile status");

            // Update profile box status
            _logger.WriteLog("***Start updating profile box status");
            _profileService.ScanAndUpdateBoxStatus();
            _logger.WriteLog("***End updating profile box status");

            // Delete packed folders
            _logger.WriteLog("***Start deleting packed folder");
            _profileService.ScanAndDeletePackedFolders();
            _logger.WriteLog("***End deleting packed folder");

            _logger.WriteLog("======================= Stopping ProfileScanWorker =======================", LogLevel.Info);
        }

        public bool CanWork
        {
            get { return _appSettings.ProfileScanWorkerEnabled; }
        }

        public int Order
        {
            get { return 2; }
        }

        #endregion
    }
}
