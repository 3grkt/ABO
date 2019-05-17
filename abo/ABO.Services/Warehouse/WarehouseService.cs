using ABO.Core;
using ABO.Core.Data;
using ABO.Core.Domain;
using ABO.Services.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABO.Services.WareHouse
{
    public class WareHouseService : IWarehouseService
    {
        #region Fields
        private readonly IRepository<Warehouse> _wareHouseRepository;
        private readonly ICacheService _cacheService;
        #endregion

        public WareHouseService(IRepository<Warehouse> wareHouseRepository, ICacheService cacheService)
        {
            _wareHouseRepository = wareHouseRepository;
            _cacheService = cacheService;
        }

        public Dictionary<string, string> GetAllWarehouses()
        {
            Dictionary<string, string> dctWarehouse = _cacheService.Get<Dictionary<string, string>>(CacheName.Warehouses);
            if (dctWarehouse == null)
            {
                var query = _wareHouseRepository.TableNoTracking;
                dctWarehouse = query.Select(t => new { t.WarehouseId, t.WarehouseName }).ToDictionary(t => t.WarehouseId, t => t.WarehouseName.Trim());
                _cacheService.Set(CacheName.Warehouses, dctWarehouse);
            }
            return dctWarehouse;
        }

    }
}
