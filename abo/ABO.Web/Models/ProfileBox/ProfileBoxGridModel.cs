using ABO.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABO.Web.Models.ProfileBox
{
    public class ProfileBoxGridModel : ViewModelBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserName { get; set; }
        public string WarehouseName { get; set; }
        public string Status { get; set; }
        public int ADACount { get; set; }
        //TODO: Add Decription field
    }
}