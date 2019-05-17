using ABO.Core;
using ABO.Core.Data;
using ABO.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABO.Services.Security
{
    public class PermissionService : IPermissionService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IWorkContext _workContext;

        public PermissionService(IRepository<User> userRepository, IWorkContext workContext)
        {
            _userRepository = userRepository;
            _workContext = workContext;
        }

        #region IPermissionService Members

        public bool Authorize(params UserPermission[] permissions)
        {
            //return Authorize(_workContext.User.UserID, permissions);
            return _workContext.User.Permissions.Any(x => permissions.Contains(x));
        }

        public bool Authorize(int userId, params UserPermission[] permissions)
        {
            var permIds = permissions.Select(p => (short)p);
            var query = _userRepository.TableNoTracking;
            return query.Any(u => u.Id == userId && u.Role.Permissions.Any(p => permIds.Contains(p.Id)));
        }

        #endregion
    }
}
