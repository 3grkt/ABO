using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace ABO.Core.Domain.WTA
{
    [Table("sakila.actor")]
    public class Actor: EntityBase
    {
        [Key]
        [Column("actor_id")]
        public int ActorId { get; set; }
        [Column("first_name")]
        public string FirstName { get; set; }
        [Column("last_name")]
        public string LastName { get; set; }

        [Column("last_update")]
        public DateTime LastUpdate { get; set; }
    }
}
