using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using ABO.Core.Domain.WTA;

namespace ABO.Data.Mapping.WTA
{
    class tblProspectAvatarMap : EntityTypeConfiguration<ProspectAvatar>
    {
        public tblProspectAvatarMap()
        {
            this.Property(e => e.img_cmnd_id).IsUnicode(false);
            this.Property(e => e.img_status).IsUnicode(false);
            this.Property(e => e.img_extension).IsUnicode(false);
            this.Property(e => e.img_file_path).IsUnicode(false);
        }
    }
}
