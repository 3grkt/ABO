using ABO.Web.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ABO.Web.Models.Profiles
{
    public class ProfileIndexModel : ViewModelBase
    {
        public ProfileIndexModel()
        {
            Distributor = new DistributorModel();
            ProfileBoxes = new List<ProfileModel>();
        }

        [ResourceDisplayName("Profile.Name")]
        public long DistNumber { get; set; }
        public DistributorModel Distributor { get; set; }
        public IList<ProfileModel> ProfileBoxes { get; set; }
        public MvcContrib.Pagination.IPagination<ProfileModel> Pager { get; set; }

        #region Nested classes
        public class DistributorModel: ViewModelBase
        {
            [ResourceDisplayName("Profile.Distributor.Name")]
            public string Name { get; set; }
            [ResourceDisplayName("Profile.Distributor.JoinDate")]
            [UIHint("DateReadonlyTextBox")]
            public DateTime? JoinDate { get; set; }
            [ResourceDisplayName("Profile.Distributor.ExpiryDate")]
            [UIHint("DateReadonlyTextBox")]
            public DateTime? ExpiryDate { get; set; }
            [ResourceDisplayName("Profile.Distributor.Address")]
            public string Address { get; set; }
            [ResourceDisplayName("Profile.Distributor.Warehouse")]
            public string WarehouseName { get; set; }
        }

        public class ProfileModel : ViewModelBase
        {
            public int Id { get; set; }
            [ResourceDisplayName("Profile.Box.CreatedDate")]
            public DateTime CreatedDate { get; set; }
            [ResourceDisplayName("Profile.Box.TypeName")]
            public string TypeName { get; set; }
            [ResourceDisplayName("Profile.Box.BoxName")]
            public string BoxName { get; set; }
            [ResourceDisplayName("Profile.Box.CreatedByUserName")]
            public string CreatedByUserName { get; set; }
            [ResourceDisplayName("Profile.Box.WarehouseName")]
            public string WarehouseName { get; set; }
            [ResourceDisplayName("Profile.Box.Description")]
            public string Description { get; set; }
            [ResourceDisplayName("Profile.Box.StatusName")]
            public string StatusName { get; set; }
        } 
        #endregion
    }
}