using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using ABO.Core.DTOs;
using ABO.Web.Framework;

namespace ABO.Web.Models.ProfileImageMerging
{
    public class ProfileImageMergingIndexModel : ViewModelBase
    {
        public ProfileImageMergingIndexModel()
        {
            DistNumber = 0;
            UndersignedPoint = new PdfOffset(412, 28.5f);
            SecondPersonPoint = new PdfOffset(507, 28.5f);
            ProfilePageIndex = 2;
            SupressWarning = false;
        }

        [ResourceDisplayName("ProfileImageMerging.AdaNo")]
        public long DistNumber { get; set; }

        [ResourceDisplayName("ProfileImageMerging.UndersignedPoint")]
        public PdfOffset UndersignedPoint { get; set; } 

        [ResourceDisplayName("ProfileImageMerging.SecondaryPoint")]
        public PdfOffset SecondPersonPoint { get; set; }

        [ResourceDisplayName("ProfileImageMerging.ProfilePageIndex")]
        public short ProfilePageIndex { get; set; }
        public bool SupressWarning { get; set; }
    }
}