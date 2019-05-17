using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ABO.Core.Domain;

namespace ABO.Data.Mapping
{
    public class ProfileTypeMap : EntityTypeConfiguration<ProfileType>
    {
        public ProfileTypeMap()
        {
			// Table
			this.ToTable("tblProfileType");

            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.Description)
                .HasMaxLength(1000);

        }
    }
}

