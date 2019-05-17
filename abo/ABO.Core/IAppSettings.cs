using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABO.Core
{
    /// <summary>
    /// Contains settings stored in App.Config/Web.Config file.
    /// </summary>
    public interface IAppSettings
    {
        /// <summary>
        /// Gets folder containing profile boxes.
        /// </summary>
        string ProfileBoxFolder { get; }

        /// <summary>
        /// Gets format of AS400 distributor dates (default: yyyyMMdd).
        /// </summary>
        string AS400DistributorDateFormat { get; }

        /// <summary>
        /// Gets date format of profile export file.
        /// </summary>
        string ProfileExportDateFormat { get; }

        /// <summary>
        /// Gets flag to indicate if DistributorUpdate worker is enabled.
        /// </summary>
        bool DistributorUpdateWorkerEnabled { get; }

        /// <summary>
        /// Gets flag to indicate if ProfileScan worker is enabled.
        /// </summary>
        bool ProfileScanWorkerEnabled { get; }

        /// <summary>
        /// Gets flag to indicate if DataPurge worker is enabled.
        /// </summary>
        bool DataPurgeWorkerEnabled { get; }

        /// <summary>
        /// Gets number of years after that expired distributor is marked as deleted.
        /// </summary>
        int ExpiredDistributorDeletionPeriod { get; }

        /// <summary>
        /// Get root path of Profile box
        /// </summary>
        string ProfileBoxRootPath { get; }

        /// <summary>
        /// Gets the replacement of test user.
        /// </summary>
        string TestUserReplacement { get; }

        /// <summary>
        /// Gets list of test users.
        /// </summary>
        IList<string> TestUsers { get; }

        /// <summary>
        /// Gets default cache duration in minutes.
        /// </summary>
        int DefaultCacheDuration { get; }

        /// <summary>
        /// Gets the number of distributor records processed per batch (use in batchjob) - default: 5000.
        /// </summary>
        int DistributorUpdateBatchSize { get; }

        /// <summary>
        /// Gets the delimiter which separates id of distributor and type of profile (use in batchjob).
        /// </summary>
        /// <remarks>E.g. 12345_s.pdf -> '_' is the delimiter.</remarks>
        char[] DistributorDelimiterInProfileFile { get; }

        int UploadAvatarWidth { get; }
        int UploadAvatarHeight { get; }
    }
}
