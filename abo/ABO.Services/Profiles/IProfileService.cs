using ABO.Core;
using ABO.Core.Domain;
using ABO.Core.SearchCriteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABO.Services.Profiles
{
    public interface IProfileService
    {
        /// <summary>
        /// Scans and copies profiles to configured folder.
        /// </summary>
        void ScanAndCopyProfiles();
        /// <summary>
        /// Scans and updates profile status.
        /// </summary>
        void ScanAndUpdateProfileStatus();
        /// <summary>
        /// Scans and updates box status.
        /// </summary>
        void ScanAndUpdateBoxStatus();
        /// <summary>
        /// Scans and deletes folder containing packed boxes.
        /// </summary>
        void ScanAndDeletePackedFolders();

        IPagedList<Profile> SearchProfilesByDistNumber(long distNumber, int page, int pageSize, string sortCol, string sortDir);
        IPagedList<Profile> SearchProfilesByStatus(int profileBoxId, int statusId, int page, int pageSize, string sortCol, string sortDir);


        /// <summary>
        /// Gets profile by id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeProfileBox"></param>
        /// <param name="includeProfileType"></param>
        /// <returns></returns>
        Profile GetProfileById(int id, bool includeProfileBox = false, bool includeProfileType = false);
        /// <summary>
        /// Gets profiles by ids.
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="tracking"></param>
        /// <returns></returns>
        IList<Profile> GetProfilesByIds(IEnumerable<int> ids, bool tracking = false);
        /// <summary>
        /// Changes box of a profile.
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="newBoxId"></param>
        /// <param name="logUser"></param>
        void ChangeProfileBox(Profile profile, int newBoxId, string logUser);
        /// <summary>
        /// Gets available profile boxes.
        /// </summary>
        /// <returns></returns>
        IList<ProfileBox> GetAvailableProfileBoxes();
        /// <summary>
        /// Deletes profiles from a distributor.
        /// </summary>
        /// <param name="distributor"></param>
        /// <param name="profileIds"></param>
        void DeleteProfiles(Distributor distributor, IEnumerable<int> profileIds, string logUser);
        /// <summary>
        /// Changes ADA of a profile.
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="newDist"></param>
        /// <param name="logUser"></param>
        void ChangeADA(Profile profile, Distributor newDist, string logUser);

        IList<Profile> GetDistributorProfilesForExport(long distNumber);
        /// <summary>
        /// Searches profile scans.
        /// </summary>
        /// <param name="profileScanSearchCriteria"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortCol"></param>
        /// <param name="sortDir"></param>
        /// <returns></returns>
        IPagedList<ProfileScan> SearchProfileScans(ProfileScanSearchCriteria profileScanSearchCriteria, int page, int pageSize, string sortCol, string sortDir);

        IList<Profile> GetProfilesForDataPurge(DateTime purgeStart, DateTime purgeEnd);

        IList<ProfileBox> GetProfileBoxesForDataPurge(DateTime purgeStart, DateTime purgeEnd);

        IList<ProfileScan> GetProfileScansForDataPurge(DateTime purgeStart, DateTime purgeEnd);

        void MoveProfile(Profile profile, int newBoxId);


        /// <summary>
        /// Updates count on ADA and Profile of box.
        /// </summary>
        /// <param name="ids"></param>
        void UpdateBoxCount(IEnumerable<int> boxIds, string logUser, string logInfo);
    }
}
