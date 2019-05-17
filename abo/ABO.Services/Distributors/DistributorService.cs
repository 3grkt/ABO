using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using ABO.Core;
using ABO.Core.Data;
using ABO.Core.Domain;
using DSS = ABO.Core.Domain.DSS;
using ABO.Core.DTOs;
using ABO.Core.Infrastructure;
using ABO.Core.SearchCriteria;
using ABO.Data;
using ABO.Services.Caching;
using ABO.Services.Logging;

namespace ABO.Services.Distributors
{
    public class DistributorService : IDistributorService
    {
        private readonly IRepository<Distributor> _distributorRepository;
        private readonly IRepository<DistributorLog> _distributorLogRepository;
        private readonly IRepository<DistributorUpdateType> _distributorUpdateTypeRepository;
        private readonly IRepository<DistributorUpdate> _distributorUpdateRepository;
        private readonly IRepository<DistributorLetter> _distributorLetterRepository;

        private readonly IDbContext _dbContext;
        private readonly IDbContext _dssDataContext;
        private readonly IDataProvider _dataProvider;
        private readonly IAppSettings _appSettings;
        private readonly ILogger _logger;
        private readonly ICacheService _cacheService;

        public DistributorService(
            IDbContext dbContext,
            IRepository<Distributor> distributorRepository,
            IRepository<DistributorLog> distributorLogRepository,
            IRepository<DistributorUpdateType> distributorUpdateTypeRepository,
            IRepository<DistributorUpdate> distributorUpdateRepository,
            IRepository<DistributorLetter> distributorLetterRepository,
            IDataProvider dataProvider,
            IAppSettings appSettings,
            ILogger logger,
            ICacheService cacheService)
        {
            _dbContext = dbContext;
            _distributorRepository = distributorRepository;
            _distributorLogRepository = distributorLogRepository;
            _distributorUpdateTypeRepository = distributorUpdateTypeRepository;
            _distributorUpdateRepository = distributorUpdateRepository;
            _distributorLetterRepository = distributorLetterRepository;
            _dataProvider = dataProvider;
            _appSettings = appSettings;
            _logger = logger;
            _cacheService = cacheService;
            _dssDataContext = EngineContext.Current.Resolve<IDbContext>("DSSDataContext");
        }

        #region IDistributorService Members

        public Distributor GetDistributorById(long id)
        {
            var query = _distributorRepository.Table.IncludeTable(x => x.Warehouse);
            return query.FirstOrDefault(x => x.DistNumber == id);
        }

        public IList<DistributorLog> GetScannableDistibutorLogs(int numberOfRecords)
        {
            return _dbContext.ExecuteSqlList<DistributorLog>(
                string.Format("SELECT TOP {0} ROW_NUMBER() OVER (ORDER BY UPDATEDTE) AS ID, DISTNO, UPDATETYPE, UPDATEDTE, WAREHOUSE FROM tblDistributorLog", numberOfRecords));
        }

        public int CountDistributorLogs()
        {
            return _distributorLogRepository.TableNoTracking.Count();
        }

        public void DeleteDistributorLogs()
        {
            _dbContext.ExecuteSql("DELETE tblDistributorLog");
        }

        public void DeleteDistributorLogs(IEnumerable<DistributorLog> logs)
        {
            foreach (var log in logs)
            {
                _dbContext.ExecuteSql(
                    "DELETE tblDistributorLog WHERE DISTNO=@DISTNO AND UPDATETYPE=@UPDATETYPE AND UPDATEDTE=@UPDATEDTE AND WAREHOUSE=@WAREHOUSE",
                    _dataProvider.CreateParameter("DISTNO", log.DISTNO, DbType.Int32),
                    _dataProvider.CreateParameter("UPDATETYPE", log.UPDATETYPE, DbType.AnsiString),
                    _dataProvider.CreateParameter("UPDATEDTE", log.UPDATEDTE, DbType.Int32),
                    _dataProvider.CreateParameter("WAREHOUSE", log.WAREHOUSE, DbType.AnsiString));
            }
        }

        public void CopyDistributorFromDSS(IEnumerable<DistributorUpdateDTO> distUpdates, out string errorMessage)
        {
            var dssDists = GetDSSDistributors(distUpdates.Select(x => x.DistNumber));
            StringBuilder sbMessage = new StringBuilder();
            foreach (var dssDist in dssDists)
            {
                if (_distributorRepository.TableNoTracking.Any(x => x.DistNumber == dssDist.DistributorNo))
                {
                    _logger.WriteLog(string.Format("Distributor {0} already existed in ABO", dssDist.DistributorNo), LogLevel.Warning);
                }
                else
                {
                    var dist = ConvertDSSDistributor(dssDist);
                    var distUpdate = distUpdates.FirstOrDefault(x => x.DistNumber == dist.DistNumber);
                    dist.WarehouseId = GetWarehouseId(distUpdate.Logs[0].WAREHOUSE);
                    using (var dbTransaction = _dbContext.BeginDbTransaction())
                    {
                        try
                        {
                            dist.DistributorUpdates.Add(new DistributorUpdate()
                            {
                                DistNumber = dist.DistNumber,
                                UpdatedDate = DateTime.Now,
                                UpdatedType = GetDistributorUpdateTypeDesc(distUpdate.Logs[0].UPDATETYPE),
                                WarehouseId = dist.WarehouseId,
                                StatusId = (short)DistributorUpdateStatus.NotCompleted,
                            });
                            _distributorRepository.Insert(dist);

                            dbTransaction.Commit();
                            _logger.WriteLog("Success to copy distributor " + dist.DistNumber, LogLevel.Info);
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            sbMessage.AppendFormat("\r\nFailed to insert distributor {0}. Error: {1}; ", dist.DistNumber, CommonHelper.GetFullExceptionDetails(ex));
                        }
                    }
                }
            }
            errorMessage = sbMessage.ToString();
        }

        public void RenewDistributorExpiry(IEnumerable<DistributorUpdateDTO> distUpdates, out string errorMessage)
        {
            var distNumbers = distUpdates.Select(x => x.DistNumber);
            var dssDists = GetDSSDistributors(distNumbers);
            var localDists = _distributorRepository.Table.Where(x => distNumbers.Contains(x.DistNumber)).ToDictionary(x => x.DistNumber);
            var sbMessage = new StringBuilder();
            foreach (var dssDist in dssDists)
            {
                if (localDists.ContainsKey(dssDist.DistributorNo))
                {
                    var dist = localDists[dssDist.DistributorNo];
                    dist.ExpiryDate = GetDistributorDateValue(dssDist.EJDATE.ToString());

                    var distUpdate = distUpdates.FirstOrDefault(x => x.DistNumber == dist.DistNumber);

                    using (var dbTransaction = _dbContext.BeginDbTransaction())
                    {
                        try
                        {
                            dist.DistributorUpdates.Add(new DistributorUpdate()
                            {
                                DistNumber = dist.DistNumber,
                                UpdatedDate = DateTime.Now,
                                UpdatedType = GetDistributorUpdateTypeDesc(distUpdate.Logs[0].UPDATETYPE),
                                WarehouseId = dist.WarehouseId,
                                StatusId = (short)DistributorUpdateStatus.Completed,
                            });
                            _distributorRepository.Update(dist);

                            dbTransaction.Commit();
                            _logger.WriteLog("Success to renew expiry date of distributor " + dist.DistNumber, LogLevel.Info);
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            sbMessage.AppendFormat("\r\nFailed to renew expiry date of distributor {0}. Error: {1}; ", dist.DistNumber, CommonHelper.GetFullExceptionDetails(ex));
                        }
                    }
                }
                else
                {
                    sbMessage.AppendFormat("Distributor {0} is not found in ABO; ", dssDist.DistributorNo);
                }
            }

            errorMessage = sbMessage.ToString();
        }

        public void UpdateDistributorInfo(IDictionary<long, DistributorUpdateDTO> updateDict, out string errorMessage)
        {
            var distNumbers = updateDict.Keys.ToList();
            var dssDists = GetDSSDistributors(distNumbers);
            var localDists = _distributorRepository.Table.Where(x => distNumbers.Contains(x.DistNumber)).ToDictionary(x => x.DistNumber);
            var sbMessage = new StringBuilder();
            foreach (var dssDist in dssDists)
            {
                if (localDists.ContainsKey(dssDist.DistributorNo))
                {
                    var dist = localDists[dssDist.DistributorNo];
                    dist = ConvertDSSDistributor(dssDist, dist);
                    using (var dbTransaction = _dbContext.BeginDbTransaction())
                    {
                        try
                        {
                            dist.DistributorUpdates.Add(new DistributorUpdate()
                            {
                                DistNumber = dist.DistNumber,
                                UpdatedDate = DateTime.Now,
                                UpdatedType = GetDistributorUpdateTypeDesc(updateDict[dssDist.DistributorNo].Logs.Select(x => x.UPDATETYPE).ToArray()),
                                WarehouseId = dist.WarehouseId,
                                StatusId = (short)DistributorUpdateStatus.NotCompleted,
                            });
                            _distributorRepository.Update(dist);

                            dbTransaction.Commit();
                            _logger.WriteLog("Success to update info of distributor " + dist.DistNumber, LogLevel.Info);
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            sbMessage.AppendFormat("\r\nFailed to update info of distributor {0}. Error: {1}; ", dist.DistNumber, CommonHelper.GetFullExceptionDetails(ex));
                        }
                    }
                }
                else
                {
                    sbMessage.AppendFormat("Distributor {0} is not found in ABO; ", dssDist.DistributorNo);
                }
            }

            errorMessage = sbMessage.ToString();
        }

        public IList<DSS.Distributor> GetDSSDistributors(IEnumerable<long> distNumbers)
        {
            if (distNumbers.Count() == 0)
            {
                return new List<DSS.Distributor>();
            }

            string distNumbersParam = string.Join(",", distNumbers);
            _logger.Debug("Executing usp_tblDistributor_GetByDistNumbers in DSS with DistNumbers=" + distNumbersParam);

            return _dssDataContext.ExecuteStoredProcedureList<DSS.Distributor>(
                "usp_tblDistributor_GetByDistNumbers",
                _dataProvider.CreateParameter("DistNumbers", distNumbersParam, DbType.AnsiString));
        }

        protected Distributor ConvertDSSDistributor(DSS.Distributor dssDist, Distributor dist = null)
        {
            if (dist == null)
            {
                dist = new Distributor();
                dist.DistNumber = dssDist.DistributorNo;
            }

            dist.Name = dssDist.Name1;
            dist.Address1 = dssDist.Address1;
            dist.Address2 = dssDist.Address2;
            dist.Address3 = dssDist.Address3;
            dist.Address4 = dssDist.Address4;
            dist.City = dssDist.City;
            dist.JoinDate = GetDistributorDateValue(dssDist.JDATE.ToString());
            dist.ExpiryDate = GetDistributorDateValue(dssDist.EJDATE.ToString());

            // Set Phone number
            if (!string.IsNullOrWhiteSpace(dssDist.Mobile))
                dist.Telephone = dssDist.Mobile.Trim();
            else if (!string.IsNullOrWhiteSpace(dssDist.HPHONE))
                dist.Telephone = dssDist.HPHONE.Trim();
            else if (!string.IsNullOrWhiteSpace(dssDist.BPHONE))
                dist.Telephone = dssDist.BPHONE.Trim();

            return dist;
        }

        protected DateTime? GetDistributorDateValue(string dateString)
        {
            if (dateString.Length == 7)
                dateString = dateString.Substring(1);

            DateTime dt;
            if (DateTime.TryParseExact(dateString, _appSettings.AS400DistributorDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                return dt;

            return null;
        }

        protected string GetDistributorUpdateTypeDesc(params string[] updateTypes)
        {
            if (updateTypes == null)
                throw new ArgumentNullException("updateTypes");

            var descriptions = _distributorUpdateTypeRepository.TableNoTracking.Where(x => updateTypes.Contains(x.Type)).Select(x => x.Description).Distinct();
            return string.Join(",", descriptions);
        }

        public IPagedList<DistributorUpdate> SearchDistributorUpdates(DistributorUpdateSearchCriteria criteria, int page, int pageSize, string sortCol, string sortDir)
        {
            var query = _distributorUpdateRepository.Table.IncludeTable(x => x.Distributor);

            if (criteria.DistNumber.HasValue)
                query = query.Where(x => x.DistNumber == criteria.DistNumber);
            if (criteria.StartDate.HasValue)
                query = query.Where(x => x.UpdatedDate >= criteria.StartDate);
            if (criteria.EndDate.HasValue)
                query = query.Where(x => x.UpdatedDate <= criteria.EndDate);
            if (criteria.Statuses != null && criteria.Statuses.Count() > 0)
                query = query.Where(x => criteria.Statuses.Contains(x.StatusId));
            if (!string.IsNullOrEmpty(criteria.WarehouseId))
                query = query.Where(x => criteria.WarehouseId == x.WarehouseId);
            if (!string.IsNullOrEmpty(criteria.UpdateType))
                query = query.Where(x => x.UpdatedType.Contains(criteria.UpdateType));

            if (string.IsNullOrEmpty(sortCol))
                query = query.OrderBy(x => x.DistNumber);
            else
                query = query.SortBy(sortCol + " " + sortDir);

            return new PagedList<DistributorUpdate>(query, page, pageSize);
        }

        public void MarkDistributorUpdatesCompleted(IEnumerable<int> distUpdateIds)
        {
            if (distUpdateIds.Any()) // WARINING: there is a known issue with BulkUpdate so need to check before update
            {
                _distributorUpdateRepository.BulkUpdate(
                    filter => distUpdateIds.Contains(filter.Id),
                    update => new DistributorUpdate { StatusId = (short)DistributorUpdateStatus.Completed, UpdatedDate = DateTime.Now });
            }
        }

        public bool IsDistributorExisted(long distNumber)
        {
            return _distributorRepository.TableNoTracking.Count(x => x.DistNumber == distNumber) > 0;
        }

        public IList<string> GetAllDistributorUpdateTypeNames()
        {
            IList<string> allTypeNames = _cacheService.Get<IList<string>>(CacheName.DistributorUpdateTypes);
            if (allTypeNames == null)
            {
                allTypeNames = _distributorUpdateTypeRepository.TableNoTracking
                                    .OrderBy(x => x.Description)
                                    .Select(x => x.Description)
                                    .Distinct()
                                    .ToList();
                _cacheService.Set(CacheName.DistributorUpdateTypes, allTypeNames);
            }
            return allTypeNames;
        }

        #endregion


        public void AddNewLetter(DistributorLetter letter)
        {
            _distributorLetterRepository.Insert(letter);
        }

        public IPagedList<DistributorLetter> SearchDistributorLetter(long? distributorNumber, DateTime startDate, DateTime endDate, string warehouse, int page, int pageSize, string sortCol, string sortDir)
        {
            endDate = endDate.Date.AddDays(1);
            var query = _distributorLetterRepository.Table
                            .IncludeTable(x => x.Distributor)
                            .IncludeTable(x => x.Distributor1)
                            .IncludeTable(x => x.Distributor2)
                            .IncludeTable(x => x.Distributor3)
                            .IncludeTable(x => x.Warehouse).IncludeTable(x => x.User);
            if (distributorNumber != null)
            {
                query = query.Where(x => x.DistNumber == distributorNumber);
            }
            else
            {
                if (!string.IsNullOrEmpty(warehouse))
                    query = query.Where(x => x.WHId == warehouse);
                query = query.Where(x => x.LetterDate >= startDate && x.LetterDate <= endDate);
                query = query.OrderBy(x => x.Id);
            }

            query = query.OrderBy(x => x.Id);
            return new PagedList<DistributorLetter>(query, page, pageSize);
        }

        public IList<Distributor> GetDistributorsForDataPurge(DateTime purgeStart, DateTime purgeEnd)
        {
            var query = _distributorRepository.Table
                            .IncludeTable(x => x.Profiles);

            var deletedDate = purgeStart.AddYears(-1);
            query = query.Where(x => x.ExpiryDate < deletedDate);

            return query.ToList();
        }

        private string GetWarehouseId(string warehouseId)
        {
            if (warehouseId.Length >= 2)
                return warehouseId.Substring(0, 2);

            return warehouseId.PadLeft(2, '0');
        }
    }
}
