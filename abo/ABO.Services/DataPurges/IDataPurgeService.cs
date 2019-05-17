using ABO.Core;
using ABO.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABO.Services.DataPurges
{
    public interface IDataPurgeService
    {
        /// <summary>
        /// Searches data purges.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortCol"></param>
        /// <param name="sortDir"></param>
        /// <returns></returns>
        IPagedList<DataPurge> SearchDataPurges(int page, int pageSize, string sortCol, string sortDir);
        /// <summary>
        /// Inserts data purge record.
        /// </summary>
        /// <param name="entity"></param>
        void Insert(DataPurge entity);
        /// <summary>
        /// Gets today data purges.
        /// </summary>
        /// <returns></returns>
        IList<DataPurge> GetTodayDataPurge();
        /// <summary>
        /// Purges profiles.
        /// </summary>
        /// <param name="purgedProfiles"></param>
        /// <param name="purgeCount"></param>
        void PurgeProfiles(IList<Profile> purgedProfiles, out int purgeCount);
        /// <summary>
        /// Purges profile boxes.
        /// </summary>
        /// <param name="purgedBoxes"></param>
        void PurgeProfileBoxes(IList<ProfileBox> purgedBoxes);
        /// <summary>
        /// Purges profile scans.
        /// </summary>
        /// <param name="purgedProfileScans"></param>
        void PurgeProfileScans(IList<ProfileScan> purgedProfileScans);
        /// <summary>
        /// Purges distributors.
        /// </summary>
        /// <param name="purgedDistributors"></param>
        void PurgeDistributors(IList<Distributor> purgedDistributors);
        /// <summary>
        /// Reset box count per warehouse to 1.
        /// </summary>
        void ResetWarehouseBoxCount();
        /// <summary>
        /// Gets data purge by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DataPurge GetDataPurgeById(int id);
        /// <summary>
        /// Deletes data purge.
        /// </summary>
        /// <param name="entity"></param>
        void Delete(DataPurge entity);
        /// <summary>
        /// Updates data purge.
        /// </summary>
        /// <param name="entity"></param>
        void Update(DataPurge entity);
    }
}
