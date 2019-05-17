using ABO.Core;
using ABO.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABO.Services.Users
{
    public interface IUserService
    {
        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<User> GetAllUsers(int pageIndex, int pageSize);
        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns>Dictionary of user ID and Name</returns>
        Dictionary<string, string> GetAllUsers();

        /// <summary>
        /// Get User by Warehouse ID and User ID
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="warehouseId"></param>
        /// <param name="userId"></param>
        /// <returns>List of user</returns>
        IPagedList<User> GetUsers(int pageIndex, int pageSize,string warehouseId,int userId);
        /// <summary>
        /// Gets user by id.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        User GetUserById(int userID);
        /// <summary>
        /// Gets user by userName.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        User GetUserByUsername(string userName);
        /// <summary>
        /// Inserts a user.
        /// </summary>
        /// <param name="user"></param>
        void InsertUser(User user);
        /// <summary>
        /// Updates a user.
        /// </summary>
        /// <param name="user"></param>
        void UpdateUser(User user);
        /// <summary>
        /// Gets roles by username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        UserRole[] GetRolesByUserName(string username);
        /// <summary>
        /// Gets roles by username.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> GetRoles();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        void DeleteUser(User entity);
    }
}
