using ABO.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABO.Services.Profiles
{
    public interface IProfileTypeService
    {
        IList<ProfileType> GetAllTypes(string sortColumn, string sortDir);
        Dictionary<string, string> GetAllTypes();
        ProfileType GetById(int id);

        void Insert(ProfileType entity);
        void Update(ProfileType entity);

        void Delete(ProfileType entity);
    }
}
