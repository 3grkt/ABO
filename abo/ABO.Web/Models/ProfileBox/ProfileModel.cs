using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABO.Web.Models.ProfileBox
{
    public class ProfileModel:ViewModelBase
    {
        public int Id { get; set; }
        public string DistNumber { get; set; }
        public DateTime ScannedDate { get; set; }
        public string StatusName { get; set; }

    }
}