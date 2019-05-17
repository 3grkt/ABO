using MvcContrib.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABO.Web.Models.User
{
    public class UsersModel:ViewModelBase
    {
        public UsersModel(){
            Data = new List<UserGridModel>();
        }
        public UserSearchModel Search { get; set; }
        public IList<UserGridModel> Data { get; set; }
        public IPagination<UserGridModel> Pager { get; set; } 
    }
}