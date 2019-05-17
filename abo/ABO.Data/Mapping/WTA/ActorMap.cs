using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using ABO.Core.Domain.WTA;

namespace ABO.Data.Mapping.WTA
{
    public class ActorMap: EntityTypeConfiguration<Actor>
    {
        public ActorMap()
        {
            ToTable("sakila.actor");

            this.HasKey(t => t.ActorId);

            this.Property(t => t.FirstName).HasColumnName("first_name");
            this.Property(t => t.ActorId).HasColumnName("actor_id").IsRequired();
            this.Property(t => t.LastName).HasColumnName("last_name").IsRequired();
            this.Property(t => t.LastUpdate).HasColumnName("last_update").IsRequired();
        }
    }
}
