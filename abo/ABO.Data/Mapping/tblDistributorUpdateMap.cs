using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ABO.Core.Domain;

namespace ABO.Data.Mapping
{
    public class DistributorUpdateMap : EntityTypeConfiguration<DistributorUpdate>
    {
        public DistributorUpdateMap()
        {
            // Table
            this.ToTable("tblDistributorUpdate");

            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.WarehouseId)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.UpdatedType)
                .IsRequired()
                .HasMaxLength(100);


            // Relationships
            this.HasRequired(t => t.Distributor)
                .WithMany(t => t.DistributorUpdates)
                .HasForeignKey(d => d.DistNumber);
            this.HasOptional(t => t.Warehouse)
                .WithMany(t => t.DistributorUpdates)
                .HasForeignKey(d => d.WarehouseId);
            this.HasRequired(t => t.Status)
                .WithMany(d => d.DistributorUpdates)
                .HasForeignKey(t => t.StatusId);

        }
    }
}

