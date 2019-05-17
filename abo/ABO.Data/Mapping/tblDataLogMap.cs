using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ABO.Core.Domain;

namespace ABO.Data.Mapping
{
    public class DataLogMap : EntityTypeConfiguration<DataLog>
    {
        public DataLogMap()
        {
			// Table
			this.ToTable("tblDataLog");

            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.TableName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.FieldName)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.PK)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.OldValue)
                .HasMaxLength(1000);

            this.Property(t => t.NewValue)
                .HasMaxLength(1000);

            this.Property(t => t.LogUser)
                .HasMaxLength(100);

            this.Property(t => t.LogInfo)
                .HasMaxLength(200);

        }
    }
}

