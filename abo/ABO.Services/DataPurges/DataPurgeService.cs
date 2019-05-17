using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using ABO.Core;
using ABO.Core.Domain;
using ABO.Core.Data;
using ABO.Data;
using ABO.Services.Logging;

namespace ABO.Services.DataPurges
{
    public class DataPurgeService : ServiceBase, IDataPurgeService
    {
        #region Fields
        private readonly IRepository<DataPurge> _dataPurgeRepository;
        private readonly IRepository<Profile> _profileRepository;
        private readonly IRepository<ProfileBox> _profileBoxRepository;
        private readonly IRepository<ProfileScan> _profileScanRepository;
        private readonly IRepository<DistributorLetter> _distributorLetterRepository;
        private readonly IRepository<Distributor> _distributorRepository;
        private readonly IRepository<DistributorUpdate> _distributorUpdateRepository;
        private readonly IRepository<Warehouse> _warehouseRepository;
        private readonly IAppSettings _appSettings;
        private readonly ILogger _logger;
        #endregion

        #region Ctor.
        public DataPurgeService(
            IRepository<DataPurge> dataPurgeRepository,
            IRepository<Profile> profileRepository,
            IRepository<ProfileBox> profileBoxRepository,
            IRepository<ProfileScan> profileScanRepository,
            IRepository<DistributorLetter> distributorLetterRepository,
            IRepository<Distributor> distributorRepository,
            IRepository<DistributorUpdate> distributorUpdateRepository,
            IRepository<Warehouse> warehouseRepository,
            IAppSettings appSettings,
            ILogger logger)
        {
            _dataPurgeRepository = dataPurgeRepository;
            _profileRepository = profileRepository;
            _profileBoxRepository = profileBoxRepository;
            _profileScanRepository = profileScanRepository;
            _distributorLetterRepository = distributorLetterRepository;
            _distributorRepository = distributorRepository;
            _distributorUpdateRepository = distributorUpdateRepository;
            _warehouseRepository = warehouseRepository;
            _appSettings = appSettings;
            _logger = logger;
        }
        #endregion

        #region IDataPurgeService Members

        public IPagedList<DataPurge> SearchDataPurges(int page, int pageSize, string sortCol, string sortDir)
        {
            var query = _dataPurgeRepository.TableNoTracking.IncludeTable(x => x.User);

            if (string.IsNullOrEmpty(sortCol))
                query = query.OrderBy(x => x.PurgeDate);
            else
                query = query.SortBy(sortCol + " " + sortDir);

            return new PagedList<DataPurge>(query, page, pageSize);
        }

        public void Insert(DataPurge entity)
        {
            _dataPurgeRepository.Insert(entity);
        }

        public IList<DataPurge> GetTodayDataPurge()
        {
            var query = _dataPurgeRepository.Table;
            query = query.Where(x => x.PurgeDate == DateTime.Today);
            return query.ToList();
        }

        public void PurgeProfiles(IList<Profile> purgedProfiles, out int purgeCount)
        {
            purgeCount = 0;
            foreach (var profile in purgedProfiles)
            {
                var physicalFile = Path.Combine(_appSettings.ProfileBoxFolder, profile.ProfileBox.Name + "\\" + profile.FileName);
                try
                {
                    if (File.Exists(physicalFile))
                        File.Delete(physicalFile);

                    _profileRepository.Delete(profile);

                    _logger.WriteLog(string.Format("Success to purge profile ({0})", physicalFile));

                    purgeCount++;
                }
                catch (Exception ex)
                {
                    _logger.WriteLog(string.Format("Failed to purge profile ({0})", physicalFile), ex);
                }
            }
        }

        public void PurgeProfileBoxes(IList<ProfileBox> purgedBoxes)
        {
            foreach (var profileBox in purgedBoxes)
            {
                if (profileBox.Profiles.Count > 0 || profileBox.ProfileScans.Count > 0)
                {
                    _logger.WriteLog(string.Format("Could not purge profile box \"\" because there are some profiles or profileScans depends on this", profileBox.Name), LogLevel.Warning);
                }
                else
                {
                    try
                    {
                        var boxFolder = Path.Combine(_appSettings.ProfileBoxFolder, profileBox.Name);
                        if (Directory.Exists(boxFolder))
                            Directory.Delete(boxFolder, true);

                        _profileBoxRepository.Delete(profileBox);

                        _logger.WriteLog(string.Format("Success to purge profile box \"{0}\".", profileBox.Name));
                    }
                    catch (Exception ex)
                    {
                        _logger.WriteLog(string.Format("Failed to purge profile box \"{0}\".", profileBox.Name), ex);
                    }
                }
            }
        }

        public void PurgeProfileScans(IList<ProfileScan> purgedProfileScans)
        {
            foreach (var profileScan in purgedProfileScans)
            {
                try
                {
                    _profileScanRepository.Delete(profileScan);
                    _logger.WriteLog(string.Format("Success to purge profile scan (ScannedDate: {0})", profileScan.ScannedDate));
                }
                catch (Exception ex)
                {
                    _logger.WriteLog(string.Format("Failed to purge profile (ScannedDate: {0})", profileScan.ScannedDate), ex);
                }
            }
        }

        public void PurgeDistributors(IList<Distributor> purgedDistributors)
        {
            foreach (var distributor in purgedDistributors)
            {
                if (distributor.Profiles.Count > 0)
                {
                    _logger.WriteLog(string.Format("Could not purge distributor \"\" because there are some profiles depends on this", distributor.DistNumber), LogLevel.Warning);
                }
                else
                {
                    try
                    {
                        // Delete distributor letters
                        _distributorLetterRepository.BulkDelete(x => x.DistNumber == distributor.DistNumber || x.SponsorId == distributor.DistNumber || x.PlatiniumSponsorId == distributor.DistNumber);

                        // Delete distributor updates
                        _distributorUpdateRepository.BulkDelete(x => x.DistNumber == distributor.DistNumber);

                        // Delete distributor
                        _distributorRepository.Delete(distributor);

                        _logger.WriteLog(string.Format("Success to purge distributor {0}", distributor.DistNumber));
                    }
                    catch (Exception ex)
                    {
                        _logger.WriteLog(string.Format("Failed to purge distributor {0}", distributor.DistNumber), ex);
                    }
                }
            }
        }

        public void ResetWarehouseBoxCount()
        {
            int currentYear = DateTime.Now.Year;
            var warehouses = _warehouseRepository.Table.Where(x => x.Year != currentYear).ToList();
            foreach (var warehouse in warehouses)
            {
                warehouse.BoxCount = 1;
                warehouse.Year = (short)currentYear;
                _warehouseRepository.Update(warehouse);
            }
        }

        public DataPurge GetDataPurgeById(int id)
        {
            return _dataPurgeRepository.Table.FirstOrDefault(x => x.Id == id);
        }

        public void Delete(DataPurge entity)
        {
            _dataPurgeRepository.Delete(entity);
        }

        public void Update(DataPurge entity)
        {
            _dataPurgeRepository.Update(entity);
        }

        #endregion
    }
}
