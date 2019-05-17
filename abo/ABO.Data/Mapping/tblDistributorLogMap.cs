using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ABO.Core.Domain;

namespace ABO.Data.Mapping
{
    public class DistributorLogMap : EntityTypeConfiguration<DistributorLog>
    {
        public DistributorLogMap()
        {
			// Table
			this.ToTable("tblDistributorLog");

            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.WAREHOUSE)
                .IsFixedLength()
                .HasMaxLength(3);

            this.Property(t => t.UPDATETYPE)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(3);


            // Relationships
            // ...
        }
    }
}

