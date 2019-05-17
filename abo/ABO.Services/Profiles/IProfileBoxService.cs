using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ABO.Core;
using ABO.Core.Domain;
using ABO.Core.Data;
using ABO.Data;
using ABO.Core.SearchCriteria;
using ABO.Core.DTOs;
namespace ABO.Services.Profiles
{
    public interface IProfileBoxService
    {
        IPagedList<ProfileBox> Search(int pageIndex, int pageSize, ProfileBoxSearchCriteria criteria);
        ProfileBox GetById(int id);
        void Insert(ProfileBox entity);
        IList<ProfileBox> GetProfileBoxByIds(IEnumerable<int> ids);
        Dictionary<string, string> GetProfileBoxByStatusType(int statusId,int? typeId,int? excludedBoxId);
        void DeleteProfileBoxs(IEnumerable<int> ids);
        void UpdateStatus(int id, short statusId, int locationId, int? profileCount, string WarehouseId);
        Dictionary<string, string> GetLocationByWarehouseId(String warehouseId);
        ProfileBox GetByFolderPath(string path);
        WarehouseBoxNumberDTO GetNextBoxNumber(string warehouseId);
    }
}
