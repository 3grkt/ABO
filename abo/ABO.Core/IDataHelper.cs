using ABO.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABO.Core
{
    /// <summary>
    /// Contains helper methods used to retrieve data in Data layer.
    /// </summary>
    public interface IDataHelper
    {
        /// <summary>
        /// Gets profiles that systems need to delete.
        /// </summary>
        /// <param name="distributorExpiryYears"></param>
        /// <returns></returns>
        IQueryable<Profile> GetNeedToDeleteProfiles(int distributorExpiryYears);
        /// <summary>
        /// Gets profile boxes that system needs to discard.
        /// </summary>
        /// <returns></returns>
        IQueryable<ProfileBox> GetNotScannedProfileBoxesOlderThanStoredYears();
    }
}
