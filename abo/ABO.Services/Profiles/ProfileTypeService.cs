using System.Collections.Generic;
using System.Linq;
using ABO.Core;
using ABO.Core.Domain;
using ABO.Core.Data;

namespace ABO.Services.Profiles
{
    public class ProfileTypeService : IProfileTypeService
    {
        private readonly IRepository<ProfileType> _profileTypeRepository;

        public ProfileTypeService(IRepository<ProfileType> profileTypeRepository)
        {
            _profileTypeRepository = profileTypeRepository;
        }

        #region IProfileTypeService Members

        public IList<ProfileType> GetAllTypes(string sortColumn, string sortDir)
        {
            var query = _profileTypeRepository.TableNoTracking;

            if (string.IsNullOrEmpty(sortColumn))
                query = query.OrderBy(x => x.Name); // default sort
            else
                query = query.SortBy(sortColumn + " " + sortDir);

            return query.ToList();
        }

        public Dictionary<string, string> GetAllTypes()
        {
            var query = _profileTypeRepository.TableNoTracking;

            return query.Select(t => new { t.Id, t.Name }).ToDictionary(t => t.Id.ToString(), t => t.Name);
        }

        public ProfileType GetById(int id)
        {
            var query = _profileTypeRepository.Table;
            return query.FirstOrDefault(x => x.Id == id);
        }

        public void Insert(ProfileType entity)
        {
            _profileTypeRepository.Insert(entity);
        }

        public void Update(ProfileType entity)
        {
            _profileTypeRepository.Update(entity);
        }

        public void Delete(ProfileType entity)
        {
            _profileTypeRepository.Delete(entity);
        }

        #endregion
    }
}
