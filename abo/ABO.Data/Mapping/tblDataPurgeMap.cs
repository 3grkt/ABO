using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ABO.Core.Domain;

namespace ABO.Data.Mapping
{
    public class DataPurgeMap : EntityTypeConfiguration<DataPurge>
    {
        public DataPurgeMap()
        {
			// Table
			this.ToTable("tblDataPurge");

            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.PurgeLog)
                .HasMaxLength(4000);


            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.DataPurges)
                .HasForeignKey(d => d.UserID);

        }
    }
}

