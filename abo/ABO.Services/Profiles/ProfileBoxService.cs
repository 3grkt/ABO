using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ABO.Core;
using ABO.Data;
using ABO.Core.Data;
using ABO.Core.Domain;
using System.Data;
using ABO.Core.DTOs;

namespace ABO.Services.Profiles
{
    public class ProfileBoxService : IProfileBoxService
    {
        private readonly IDbContext _dbContext;
        private readonly IDataProvider _dataProvider;
        private readonly IRepository<ProfileBox> _profileBoxRepository;
        private readonly IRepository<Location> _locationRepository;
        private readonly IRepository<Profile> _profileRepository;
        private readonly IRepository<Warehouse> _warehouseRepository;

        public ProfileBoxService(
            IDbContext dbContext,
            IDataProvider dataProvider,
            IRepository<ProfileBox> profileBoxRepository,
            IRepository<Location> locationRepository,
            IRepository<Profile> profileRepository,
            IRepository<Warehouse> warehouseRepository)
        {
            this._dbContext = dbContext;
            this._dataProvider = dataProvider;
            this._profileBoxRepository = profileBoxRepository;
            _locationRepository = locationRepository;
            _profileRepository = profileRepository;
            _warehouseRepository = warehouseRepository;
        }


        public Core.IPagedList<ProfileBox> Search(int pageIndex, int pageSize, Core.SearchCriteria.ProfileBoxSearchCriteria criteria)
        {
            //var paramUserID = _dataProvider.CreateParameter("UserId", criteria.UserID, DbType.Int32);
            //var paramWarehouseId = _dataProvider.CreateParameter("WarehouseId", criteria.WarehouseID, DbType.String);
            //var paramTypeId = _dataProvider.CreateParameter("TypeId", criteria.ProfileTypeID, DbType.Int32);
            //var paramStatusId = _dataProvider.CreateParameter("StatusId", criteria.StatusID, DbType.Int32);
            //var paramPageIndex = _dataProvider.CreateParameter("PageIndex", pageIndex, DbType.Int32);
            //var paramPageSize = _dataProvider.CreateParameter("PageSize", pageSize, DbType.Int32);
            //var paramTotalCount = _dataProvider.CreateParameter("TotalCount", 0, DbType.Int32, ParameterDirection.Output);

            //var profileBox = _dbContext.ExecuteStoredProcedureList<SearchProfileBox>("usp_tblProfileBox_Search",
            //    paramUserID,
            //    paramWarehouseId,
            //    paramTypeId,
            //    paramStatusId,
            //    paramPageIndex,
            //    paramPageSize,
            //    paramTotalCount);

            //return new PagedList<SearchProfileBox>(profileBox, pageIndex, pageSize, Convert.ToInt32(paramTotalCount.Value));

            var query = _profileBoxRepository.Table
               .IncludeTable(x => x.Warehouse)
               .IncludeTable(x => x.ProfileType)
               .IncludeTable(x => x.Status)
               .IncludeTable(x => x.User);

            if (!string.IsNullOrEmpty(criteria.ProfileBoxName))
            {
                query = query.Where(x => x.Name.Contains(criteria.ProfileBoxName) || x.Name == criteria.ProfileBoxName);
            }
            else
            {
                if (criteria.UserID != 0)
                    query = query.Where(x => x.CreatedBy == criteria.UserID);


                if (criteria.StatusID != 0)
                    query = query.Where(x => x.StatusId == criteria.StatusID);

                if (criteria.WarehouseID != null)
                    query = query.Where(x => x.WarehouseId == criteria.WarehouseID);

                if (criteria.ProfileTypeID != 0)
                    query = query.Where(x => x.TypeId == criteria.ProfileTypeID);

            }
            query = query.OrderBy(x => x.CreatedBy);


            return new PagedList<ProfileBox>(query, pageIndex, pageSize);
        }

        public ProfileBox GetById(int id)
        {
            var query = _profileBoxRepository.Table
                .IncludeTable(m => m.User)
                .IncludeTable(x => x.Warehouse)
                .IncludeTable(x => x.Office)
                .IncludeTable(x => x.ProfileType)
                .IncludeTable(x => x.Status)
                .IncludeTable(x => x.Location);

            return query.Where(t => t.Id == id).FirstOrDefault();
        }
        public ProfileBox GetByFolderPath(string path)
        {
            var query = _profileBoxRepository.TableNoTracking;

            return query.Where(t => t.ScannedFolder == path).FirstOrDefault();
        }
        public void Insert(ProfileBox entity)
        {
            _profileBoxRepository.Insert(entity);
        }


        public IList<ProfileBox> GetProfileBoxByIds(IEnumerable<int> ids)
        {
            var query = _profileBoxRepository.Table;
            query = query.Where(x => ids.Contains(x.Id));
            return query.ToList();
        }

        public void DeleteProfileBoxs(IEnumerable<int> ids)
        {
            if (ids.Any())// WARINING: there is a known issue with BulkUpdate so need to check before update
            {
                _profileBoxRepository.BulkUpdate(
                   f => ids.Contains(f.Id),
                   u => new ProfileBox() { StatusId = (short)ProfileBoxStatus.Discarded });
            }
        }


        public void UpdateStatus(int id, short statusId, int locationId, int? profileCount = null, string WarehouseId = null)
        {
            var profileBox = _profileBoxRepository.Table.Where(x => x.Id == id).FirstOrDefault();
            if (profileBox != null)
            {
                profileBox.StatusId = statusId;
                if (locationId != 0)
                    profileBox.LocationId = locationId;
                else profileBox.LocationId = null;
                if (profileCount != null)
                    profileBox.ProfileCount = profileCount;
                if (WarehouseId != null)
                    profileBox.WarehouseId = WarehouseId;
                _profileBoxRepository.Update(profileBox);
            }
        }


        public Dictionary<string, string> GetProfileBoxByStatusType(int statusId, int? typeId, int? excludedBoxId)
        {
            var query = _profileBoxRepository.TableNoTracking.Where(x => x.StatusId == statusId);
            if (typeId != null)
                query = query.Where(x => x.TypeId == typeId.Value);
            if (excludedBoxId != null)
                query = query.Where(x => x.Id != excludedBoxId.Value);

            return query.Select(t => new { t.Id, t.Name }).ToDictionary(t => t.Id.ToString(), t => t.Name);
        }

        public Dictionary<string, string> GetLocationByWarehouseId(String warehouseId)
        {
            var query = _locationRepository.TableNoTracking.Where(x => x.WHcode == warehouseId);
            return query.Select(t => new { t.Id, t.Name }).ToDictionary(t => t.Id.ToString(), t => t.Name);
        }

        public WarehouseBoxNumberDTO GetNextBoxNumber(string warehouseId)
        {
            var warehouse = _warehouseRepository.Table.FirstOrDefault(x => x.WarehouseId == warehouseId);
            if (warehouse == null)
                return null;

            var boxCount = warehouse.BoxCount;

            // Update new value
            warehouse.BoxCount++;
            _warehouseRepository.Update(warehouse);

            return new WarehouseBoxNumberDTO { BoxNumber = boxCount, Year = warehouse.Year };
        }
    }
}
