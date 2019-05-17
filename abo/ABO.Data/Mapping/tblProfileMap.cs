using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ABO.Core.Domain;

namespace ABO.Data.Mapping
{
    public class ProfileMap : EntityTypeConfiguration<Profile>
    {
        public ProfileMap()
        {
			// Table
			this.ToTable("tblProfile");

            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.WarehouseId)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.FileName)
                .HasMaxLength(100);

            this.Property(t => t.Description)
                .HasMaxLength(1000);


            // Relationships
            this.HasOptional(t => t.Distributor)
                .WithMany(t => t.Profiles)
                .HasForeignKey(d => d.DistNumber);
            this.HasOptional(t => t.ProfileBox)
                .WithMany(t => t.Profiles)
                .HasForeignKey(d => d.BoxId);
            this.HasRequired(t => t.ProfileType)
                .WithMany(t => t.Profiles)
                .HasForeignKey(d => d.TypeId);
            this.HasRequired(t => t.Status)
                .WithMany(t => t.Profiles)
                .HasForeignKey(d => d.StatusId);
            this.HasOptional(t => t.User)
                .WithMany(t => t.Profiles)
                .HasForeignKey(d => d.UserId);
            this.HasOptional(t => t.Warehouse)
                .WithMany(t => t.Profiles)
                .HasForeignKey(d => d.WarehouseId);

        }
    }
}

