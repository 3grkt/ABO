using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ABO.Core.Domain;

namespace ABO.Data.Mapping
{
    public class WarehouseMap : EntityTypeConfiguration<Warehouse>
    {
        public WarehouseMap()
        {
			// Table
			this.ToTable("tblWarehouse");

            // Primary Key
            this.HasKey(t => t.WarehouseId);

            // Properties
            this.Property(t => t.WarehouseId)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.WarehouseName)
                .IsRequired()
                .HasMaxLength(150);

        }
    }
}

