using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ABO.Core;
using ABO.Core.Domain;
using ABO.Core.Data;
using ABO.Data;

namespace ABO.Services.Users
{
    public class UserService : IUserService
    {
        #region Fields
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;
        #endregion

        #region Ctor.
        public UserService(IRepository<User> userRepository, IRepository<Role> roleRepository)
        {
            this._userRepository = userRepository;
            this._roleRepository = roleRepository;
        }
        #endregion

        #region Methods

        public IPagedList<User> GetAllUsers(int pageIndex, int pageSize)
        {
            var query = _userRepository.Table;
            query = query.OrderBy(u => u.UserName);
            return new PagedList<User>(query, pageIndex, pageSize);
        }

        public User GetUserById(int userID)
        {
            return _userRepository.GetById(userID);
        }

        public User GetUserByUsername(string userName)
        {
            var query = _userRepository.TableNoTracking
                .IncludeTable(x => x.Role)
                .IncludeTable(x => x.Role.Permissions)
                .IncludeTable(x => x.Warehouse);
            return query.FirstOrDefault(x => x.UserName == userName);
        }

        public void InsertUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            _userRepository.Insert(user);
        }

        public void UpdateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            _userRepository.Update(user);
        }

        public UserRole[] GetRolesByUserName(string username)
        {
            var query = _userRepository.TableNoTracking
                    .Where(u => u.UserName == username);
            return query.Select(u => (UserRole)u.RoleId).ToArray();
        }

        #endregion


        public Dictionary<string, string> GetAllUsers()
        {
            var query = _userRepository.TableNoTracking;
            return query.Select(t => new { t.Id, t.UserName }).ToDictionary(t => t.Id.ToString(), t => t.UserName);
        }


        public IPagedList<User> GetUsers(int pageIndex, int pageSize, string warehouseId = null, int userId = 0)
        {
            var query = _userRepository.TableNoTracking.IncludeTable(x => x.Role).IncludeTable(x => x.Warehouse);

            if (userId != 0)
                query = query.Where(t => t.Id == userId);

            if (warehouseId != null)
                query = query.Where(t => t.WarehouseId == warehouseId);
            query = query.OrderBy(u => u.Id);
            return new PagedList<User>(query, pageIndex, pageSize);

        }

        public Dictionary<string, string> GetRoles()
        {
            var query = _roleRepository.TableNoTracking;
            return query.Select(t => new { t.Id, t.Name }).ToDictionary(t => t.Id.ToString(), t => t.Name);
        }

        public void DeleteUser(User entity)
        {
            _userRepository.Delete(entity);

        }
    }
}
