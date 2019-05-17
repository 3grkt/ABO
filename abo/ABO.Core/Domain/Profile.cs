
using System;
using System.Collections.Generic;

namespace ABO.Core.Domain
{

    public partial class Profile : EntityBase
    {
        public int Id { get; set; }
        public string WarehouseId { get; set; }
        public int TypeId { get; set; }
        public Nullable<int> BoxId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<long> DistNumber { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ScannedDate { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public short StatusId { get; set; }
        public virtual Distributor Distributor { get; set; }
        public virtual ProfileBox ProfileBox { get; set; }
        public virtual ProfileType ProfileType { get; set; }
        public virtual Status Status { get; set; }
        public virtual User User { get; set; }
        public virtual Warehouse Warehouse { get; set; }

        public override object Clone()
        {
            return new Profile()
            {
                //Id = this.Id,
                WarehouseId = this.WarehouseId,
                TypeId = this.TypeId,
                BoxId = this.BoxId,
                UserId = this.UserId,
                DistNumber = this.DistNumber,
                CreatedDate = this.CreatedDate,
                ScannedDate = this.ScannedDate,
                Description = this.Description,
                StatusId = this.StatusId,
            };
        }
    }
}

