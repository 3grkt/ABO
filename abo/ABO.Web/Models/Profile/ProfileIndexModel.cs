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
            ProfileBoxes = new List<ProfileBoxModel>();
        }

        public long DistNumber { get; set; }
        public DistributorModel Distributor { get; set; }
        public IList<ProfileBoxModel> ProfileBoxes { get; set; }
        public MvcContrib.Pagination.IPagination<ProfileBoxModel> Pager { get; set; }

        #region Nested classes
        public class DistributorModel: ViewModelBase
        {
            public string Name { get; set; }
            [UIHint("DateReadonlyTextBox")]
            public DateTime? JoinDate { get; set; }
            [UIHint("DateReadonlyTextBox")]
            public DateTime? ExpiryDate { get; set; }
            public string Address { get; set; }
            public string WarehouseName { get; set; }
        }

        public class ProfileBoxModel : ViewModelBase
        {
            public int Id { get; set; }
            public DateTime CreatedDate { get; set; }
            public string TypeName { get; set; }
            public string BoxName { get; set; }
            public string CreatedByUserName { get; set; }
            public string WarehouseName { get; set; }
            public string Description { get; set; }
            public string StatusName { get; set; }
        } 
        #endregion
    }

}