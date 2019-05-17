using ABO.Core;
using ABO.Services.DataPurges;
using ABO.Services.Distributors;
using ABO.Services.Logging;
using ABO.Services.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABO.Jobs.Workers
{
    public class DataPurgeWorker : IWorker
    {
        private readonly IDataPurgeService _dataPurgeService;
        private readonly IProfileService _profileService;
        private readonly IDistributorService _distributorService;
        private readonly ILogger _logger;
        private readonly IAppSettings _appSettings;

        public DataPurgeWorker(
            IDataPurgeService dataPurgeService,
            IProfileService profileService,
            IDistributorService distributorService,
            ILogger logger,
            IAppSettings appSettings)
        {
            _dataPurgeService = dataPurgeService;
            _profileService = profileService;
            _distributorService = distributorService;
            _logger = logger;
            _appSettings = appSettings;
        }

        #region IWorker Members

        public void DoWork()
        {
            _logger.WriteLog("======================= Running DataPurgeWorker =======================", LogLevel.Info);

            #region Reset warehouse box count

            _dataPurgeService.ResetWarehouseBoxCount(); // always run

            #endregion

            #region Purge data
            var todayDataPurges = _dataPurgeService.GetTodayDataPurge();

            if (todayDataPurges.Count == 0)
            {
                _logger.WriteLog("No data purge found today.");
            }
            else
            {
                foreach (var dataPurge in todayDataPurges)
                {
                    _logger.WriteLog("***Start purging profiles");
                    var purgedProfiles = _profileService.GetProfilesForDataPurge(dataPurge.StartDate, dataPurge.EndDate.EndOfDate());
                    int profilePurgeCount;
                    _dataPurgeService.PurgeProfiles(purgedProfiles, out profilePurgeCount);
                    _logger.WriteLog("***End purging profiles");

                    _logger.WriteLog("***Start purging profile scans");
                    var purgedProfileScans = _profileService.GetProfileScansForDataPurge(dataPurge.StartDate, dataPurge.EndDate.EndOfDate());
                    _dataPurgeService.PurgeProfileScans(purgedProfileScans);
                    _logger.WriteLog("***End purging profile scans");

                    _logger.WriteLog("***Start purging profile boxes");
                    var purgedBoxes = _profileService.GetProfileBoxesForDataPurge(dataPurge.StartDate, dataPurge.EndDate.EndOfDate());
                    _dataPurgeService.PurgeProfileBoxes(purgedBoxes);
                    _logger.WriteLog("***End purging profile boxes");

                    _logger.WriteLog("***Start purging distributors");
                    var purgedDistributors = _distributorService.GetDistributorsForDataPurge(dataPurge.StartDate, dataPurge.EndDate.EndOfDate());
                    _dataPurgeService.PurgeDistributors(purgedDistributors);
                    _logger.WriteLog("***End purging distributors");

                    // Update file count
                    dataPurge.FileCount = profilePurgeCount;
                    _dataPurgeService.Update(dataPurge);
                }
            } 
            #endregion

            _logger.WriteLog("======================= Stopping DataPurgeWorker =======================", LogLevel.Info);
        }

        public bool CanWork
        {
            get { return _appSettings.DataPurgeWorkerEnabled; }
        }

        public int Order
        {
            get { return 3; }
        }

        #endregion
    }
}
