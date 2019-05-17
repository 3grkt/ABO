using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ABO.Core.Domain;

namespace ABO.Data.Mapping
{
    public class ProfileScanMap : EntityTypeConfiguration<ProfileScan>
    {
        public ProfileScanMap()
        {
			// Table
			this.ToTable("tblProfileScan");

            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.WarehouseId)
                .IsFixedLength()
                .HasMaxLength(2);

            //this.Property(t => t.Description)
            //    .HasMaxLength(1000);


            // Relationships
            this.HasRequired(t => t.ProfileBox)
                .WithMany(t => t.ProfileScans)
                .HasForeignKey(d => d.BoxId);
            this.HasOptional(t => t.Warehouse)
                .WithMany(t => t.ProfileScans)
                .HasForeignKey(d => d.WarehouseId);

        }
    }
}

