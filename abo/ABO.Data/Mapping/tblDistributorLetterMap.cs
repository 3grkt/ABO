using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ABO.Core.Domain;

namespace ABO.Data.Mapping
{
    public class DistributorLetterMap : EntityTypeConfiguration<DistributorLetter>
    {
        public DistributorLetterMap()
        {
            // Table
            this.ToTable("tblDistributorLetter");

            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.DistName)
                .IsRequired()
                .HasMaxLength(200);


            // Relationships
            this.HasRequired(t => t.Distributor)
                .WithMany(t => t.DistributorLetters)
                .HasForeignKey(d => d.DistNumber);
            this.HasOptional(t => t.Distributor1)
                .WithMany(t => t.DistributorLetters1)
                .HasForeignKey(d => d.SponsorId);
            this.HasOptional(t => t.Distributor2)
                .WithMany(t => t.DistributorLetters2)
                .HasForeignKey(d => d.PlatiniumSponsorId);
            this.HasOptional(t => t.Distributor3)
               .WithMany(t => t.DistributorLetters3)
               .HasForeignKey(d => d.OldDistNumber);
            this.HasOptional(t => t.User)
               .WithMany(t => t.DistributorLetter)
               .HasForeignKey(d => d.UserId);
            this.HasOptional(t => t.Warehouse)
               .WithMany(t => t.DistributorLetter)
               .HasForeignKey(d => d.WHId);
        }
    }
}

