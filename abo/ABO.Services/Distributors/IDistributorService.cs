using ABO.Core.Domain;
using DSS = ABO.Core.Domain.DSS;
using System.Collections.Generic;
using ABO.Core.DTOs;
using System;
using ABO.Core;
using ABO.Core.SearchCriteria;

namespace ABO.Services.Distributors
{
    public interface IDistributorService
    {
        Core.Domain.Distributor GetDistributorById(long distNumber);
        /// <summary>
        /// Gets distributors updated in AS400.
        /// </summary>
        /// <returns></returns>
        IList<DistributorLog> GetScannableDistibutorLogs(int numberOfRecords);
        /// <summary>
        /// Counts the number of distributor logs.
        /// </summary>
        /// <returns></returns>
        int CountDistributorLogs();
        /// <summary>
        /// Deletes all records from DistributorLog table.
        /// </summary>
        void DeleteDistributorLogs();

        /// <summary>
        /// Deletes given logs from DistributorLog table.
        /// </summary>
        /// <param name="logs"></param>
        void DeleteDistributorLogs(IEnumerable<DistributorLog> logs);

        void CopyDistributorFromDSS(IEnumerable<DistributorUpdateDTO> distNumbers, out string errorMessage);

        void RenewDistributorExpiry(IEnumerable<DistributorUpdateDTO> distNumbers, out string errorMessage);

        void UpdateDistributorInfo(IDictionary<long, DistributorUpdateDTO> updateDict, out string errorMessage);

        void AddNewLetter(DistributorLetter letter);
        IPagedList<DistributorLetter> SearchDistributorLetter(long? distributorNumber, DateTime startDate, DateTime endDate,string warehouse, int page, int pageSize, string sortCol, string sortDir);


        IList<DSS.Distributor> GetDSSDistributors(IEnumerable<long> distNumbers);

        IPagedList<DistributorUpdate> SearchDistributorUpdates(DistributorUpdateSearchCriteria criteria, int page, int pageSize, string sortCol, string sortDir);

        void MarkDistributorUpdatesCompleted(IEnumerable<int> distUpdateIds);

        bool IsDistributorExisted(long distNumber);

        IList<Distributor> GetDistributorsForDataPurge(DateTime purgeStart, DateTime purgeEnd);

        IList<string> GetAllDistributorUpdateTypeNames();
    }
}
