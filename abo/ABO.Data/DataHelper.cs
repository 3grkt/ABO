using ABO.Core;
using ABO.Core.Data;
using ABO.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace ABO.Data
{
    public class DataHelper : IDataHelper
    {
        private readonly IRepository<Profile> _profileRepository;
        private readonly IRepository<ProfileBox> _profileBoxRepository;

        public DataHelper(IRepository<Profile> profileRepository, IRepository<ProfileBox> profileBoxRepository)
        {
            _profileRepository = profileRepository;
            _profileBoxRepository = profileBoxRepository;
        }

        #region IDataHelper Members

        public IQueryable<Profile> GetNeedToDeleteProfiles(int distributorExpiryYears)
        {
            var query = _profileRepository.Table;
            query = query.Where(x => x.StatusId == (short)ProfileStatus.Valid &&
                (
                    (DateTime.Now > DbFunctions.AddYears(x.CreatedDate, x.ProfileType.StoredYears)) || // Now > CreatedDate + StoredYears
                    (DateTime.Now > DbFunctions.AddYears(x.Distributor.ExpiryDate, distributorExpiryYears)) // Now > Distributor Expiry Date + n year
                ));
            return query;
        }

        public IQueryable<ProfileBox> GetNotScannedProfileBoxesOlderThanStoredYears()
        {
            var query = _profileBoxRepository.Table;
            query = query.Where(x => (x.StatusId == (short)ProfileBoxStatus.Stored || x.StatusId == (short)ProfileBoxStatus.Packed) &&
                    !x.ProfileType.Scanned &&
                    DateTime.Now > DbFunctions.AddYears(x.CreatedDate, x.ProfileType.StoredYears));
            return query;
        }

        #endregion
    }
}
