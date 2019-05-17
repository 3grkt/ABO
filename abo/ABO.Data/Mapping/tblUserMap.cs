using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ABO.Core.Domain;

namespace ABO.Data.Mapping
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
			// Table
			this.ToTable("tblUser");

            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.UserName)
                .IsRequired()
                .HasMaxLength(8);

            this.Property(t => t.FullName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.WarehouseId)
                .IsFixedLength()
                .HasMaxLength(2);


            // Relationships
            this.HasRequired(t => t.Role)
                .WithMany(t => t.Users)
                .HasForeignKey(d => d.RoleId);
            this.HasOptional(t => t.Warehouse)
                .WithMany(t => t.Users)
                .HasForeignKey(d => d.WarehouseId);

        }
    }
}

