using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ABO.Core.Domain;

namespace ABO.Data.Mapping
{
    public class DistributorMap : EntityTypeConfiguration<Distributor>
    {
        public DistributorMap()
        {
			// Table
			this.ToTable("tblDistributor");

            // Primary Key
            this.HasKey(t => t.DistNumber);

            // Properties
            this.Property(t=>t.DistNumber)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.Address1)
                .HasMaxLength(50);

            this.Property(t => t.Address2)
                .HasMaxLength(50);

            this.Property(t => t.Address3)
                .HasMaxLength(50);

            this.Property(t => t.Address4)
                .HasMaxLength(50);

            this.Property(t => t.City)
                .HasMaxLength(50);

            this.Property(t => t.WarehouseId)
                .IsFixedLength()
                .HasMaxLength(2);

            // Relationships
            this.HasOptional(t => t.Warehouse)
                .WithMany(t => t.Distributors)
                .HasForeignKey(d => d.WarehouseId);
        }
    }
}

