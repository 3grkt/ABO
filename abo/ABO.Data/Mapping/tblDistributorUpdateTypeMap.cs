using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ABO.Core.Domain;

namespace ABO.Data.Mapping
{
    public class DistributorUpdateTypeMap : EntityTypeConfiguration<DistributorUpdateType>
    {
        public DistributorUpdateTypeMap()
        {
			// Table
			this.ToTable("tblDistributorUpdateType");

            // Primary Key
            this.HasKey(t => t.Type);

            // Properties
            this.Property(t => t.Type)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.Description)
                .HasMaxLength(200);


            // Relationships
            this.HasRequired(t => t.ProfileType)
                .WithMany(t => t.DistributorUpdateTypes)
                .HasForeignKey(d => d.ProfileTypeId);

        }
    }
}

