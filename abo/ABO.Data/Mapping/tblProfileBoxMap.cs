using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ABO.Core.Domain;

namespace ABO.Data.Mapping
{
    public class ProfileBoxMap : EntityTypeConfiguration<ProfileBox>
    {
        public ProfileBoxMap()
        {
			// Table
			this.ToTable("tblProfileBox");

            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.WarehouseId)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.ScannedFolder)
                .HasMaxLength(255);


            // Relationships
            this.HasOptional(t => t.Location)
                .WithMany(t => t.ProfileBoxes)
                .HasForeignKey(d => d.LocationId);
            this.HasRequired(t => t.ProfileType)
                .WithMany(t => t.ProfileBoxes)
                .HasForeignKey(d => d.TypeId);
            this.HasRequired(t => t.Status)
                .WithMany(t => t.ProfileBoxes)
                .HasForeignKey(d => d.StatusId);
            this.HasRequired(t => t.User)
                .WithMany(t => t.ProfileBoxes)
                .HasForeignKey(d => d.CreatedBy);
            this.HasOptional(t => t.Warehouse)
                .WithMany(t => t.ProfileBoxes)
                .HasForeignKey(d => d.WarehouseId);
            this.HasOptional(t => t.Office)
               .WithMany(t => t.ProfileBoxes2)
               .HasForeignKey(d => d.OfficeId);
        }
    }
}

