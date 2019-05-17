using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ABO.Core.Domain;

namespace ABO.Data.Mapping
{
    public class RoleMap : EntityTypeConfiguration<Role>
    {
        public RoleMap()
        {
			// Table
			this.ToTable("tblRole");

            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Description)
                .HasMaxLength(255);


            // Relationships
            this.HasMany(t => t.Permissions)
                .WithMany(t => t.Roles)
                .Map(m =>
                    {
                        m.ToTable("tblRolePermission");
                        m.MapLeftKey("RoleId");
                        m.MapRightKey("PermissionId");
                    });


        }
    }
}

