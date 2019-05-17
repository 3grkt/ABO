using System.Collections.Generic;
using ABO.Core.Domain.DSS;
using ABO.Core.Domain.WTA;
using ABO.Core.DTOs;

namespace ABO.Services.Profiles
{
    public interface IProfileImageMergingService
    {
        byte[] MergeImageToProfile(long adaNo, short pageNum, PdfOffset image1Offset, PdfOffset image2Offset);
        ProspectAvatar[] GetImageByAdaNo(long adaNo, bool isReadOnly = true);
        KeyValuePair<string, ProspectAvatar>[] GetImageByAdaNoWithDistId(long adaNo);
        ProspectAvatar[] GetImageByAdaNo(Distributor distributor, bool isReadOnly = true);
        byte[] GetDistributorPhysicalProfile(long adaNo);
        void UploadUsersImage(long adaNo, ProspectAvatar avatar1, ProspectAvatar avatar2);
        byte[] GetDistributorPhysicalProfile(Distributor distributor);
    }
}