using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace ABO.Core.Domain.WTA
{
    [Table("tblprospectavatar")]
    public class ProspectAvatar: EntityBase
    {
        [Key]
        [Column(Order = 0, TypeName = "char")]
        [StringLength(20)]
        public string img_cmnd_id { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "char")]
        [StringLength(1)]
        public string img_status { get; set; }

        [Column(TypeName = "char")]
        [StringLength(4)]
        public string img_extension { get; set; }

        [Column(TypeName = "char")]
        [StringLength(255)]
        public string img_file_path { get; set; }

        public byte[] img_streamdata { get; set; }

        public override object Clone()
        {
            var newAvatar = (ProspectAvatar) this.MemberwiseClone();

            if (this.img_streamdata != null)
            {
                newAvatar.img_streamdata = this.img_streamdata.ToArray();
            }

            return newAvatar;
        }
    }

    public class ProspectAvatarKey
    {
        public string img_cmnd_id { get; set; }
        public string img_status { get; set; }
    }
}
