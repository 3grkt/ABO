using ABO.Core;
using ABO.Core.Domain;
using ABO.Core.DTOs;
using ABO.Services.Distributors;
using ABO.Services.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABO.Jobs.Workers
{
    public class DistributorUpdateWorker : IWorker
    {
        private readonly IDistributorService _distributorService;
        private readonly ILogger _logger;
        private readonly IAppSettings _appSettings;

        public DistributorUpdateWorker(IDistributorService distributorService, ILogger logger, IAppSettings appSettings)
        {
            _distributorService = distributorService;
            _logger = logger;
            _appSettings = appSettings;
        }

        #region IWorker Members

        public void DoWork()
        {
            _logger.Info("======================= Runnning DistributorUpdateWorker =======================");
            try
            {
                // Process in batches to avoid high memory issue
                int totalRecords = _distributorService.CountDistributorLogs();
                int numberOfBatches = (int)Math.Ceiling((double)totalRecords / _appSettings.DistributorUpdateBatchSize);
                _logger.Info(string.Format("Found {0} records - Processing in {1} batch(es)", totalRecords, numberOfBatches));

                for (int i = 0; i < numberOfBatches; i++)
                {
                    _logger.Info(string.Format("### Processing batch {0}", i + 1));
                    var scannableDistributorLogs = _distributorService.GetScannableDistibutorLogs(_appSettings.DistributorUpdateBatchSize);
                    Process(scannableDistributorLogs);
                }

            }
            catch (Exception ex)
            {
                _logger.Error("Failed on running DistributorUpdateWorker", ex);
            }
            _logger.Info("======================= Stopping DistributorUpdateWorker =======================");
        }

        private void Process(IList<DistributorLog> scannableDistributorLogs)
        {
            string errorMessage = string.Empty;
            var copyList = new List<DistributorUpdateDTO>();
            var renewalList = new List<DistributorUpdateDTO>();
            var updateDict = new Dictionary<long, DistributorUpdateDTO>();
            _logger.Info("Start loading distributor logs");
            foreach (var distLog in scannableDistributorLogs)
            {
                _logger.Debug(string.Format(" Record found: {0}\t{1}\t{2}\t{3}", distLog.DISTNO, distLog.UPDATETYPE, distLog.UPDATEDTE, distLog.WAREHOUSE));

                // Not scanned &  Join
                if (DistUpdateType.Joins.Contains(distLog.UPDATETYPE))
                {
                    copyList.Add(new DistributorUpdateDTO(distLog.DISTNO, distLog));
                }
                // Skip & Renewal
                else if (DistUpdateType.Renewals.Contains(distLog.UPDATETYPE))
                {
                    renewalList.Add(new DistributorUpdateDTO(distLog.DISTNO, distLog));
                }
                // Else, update relevant info
                else
                {
                    if (updateDict.ContainsKey(distLog.DISTNO))
                    {
                        updateDict[distLog.DISTNO].Logs.Add(distLog);
                    }
                    else
                    {
                        updateDict[distLog.DISTNO] = new DistributorUpdateDTO(distLog.DISTNO, distLog);
                    }
                }
            }
            _logger.Info("End loading distributor logs");

            _logger.Info("***Start copying distributors from DSS");
            _distributorService.CopyDistributorFromDSS(copyList, out errorMessage);
            if (!string.IsNullOrEmpty(errorMessage))
                _logger.WriteLog("Distributor Copy error message: " + errorMessage, LogLevel.Error);
            _logger.Info("***End copying distributors from DSS");

            _logger.Info("***Start renewing distributor's expiry");
            _distributorService.RenewDistributorExpiry(renewalList, out errorMessage);
            if (!string.IsNullOrEmpty(errorMessage))
                _logger.WriteLog("Distributor Renewal error message: " + errorMessage, LogLevel.Error);
            _logger.Info("***End renewing distributor's expiry");

            _logger.Info("***Start updating distributor info");
            _distributorService.UpdateDistributorInfo(updateDict, out errorMessage);
            if (!string.IsNullOrEmpty(errorMessage))
                _logger.WriteLog("Distributor Update error message: " + errorMessage, LogLevel.Error);
            _logger.Info("***End updating distributor info");

            _logger.Info("***Deleting distributor logs");
            _distributorService.DeleteDistributorLogs(scannableDistributorLogs);
        }

        public bool CanWork
        {
            get { return _appSettings.DistributorUpdateWorkerEnabled; }
        }

        public int Order
        {
            get { return 1; }
        }

        #endregion
    }
}
