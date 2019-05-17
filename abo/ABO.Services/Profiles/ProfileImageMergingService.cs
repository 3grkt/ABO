using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using ABO.Core;
using ABO.Core.Data;
using ABO.Core.Domain;
using ABO.Core.Domain.WTA;
using ABO.Core.DTOs;
using ABO.Data;
using ABO.Services.Distributors;
using ABO.Services.Localization;
using iTextSharp.text.pdf;
using Distributor = ABO.Core.Domain.DSS.Distributor;

namespace ABO.Services.Profiles
{
    public class ProfileImageMergingService : IProfileImageMergingService
    {
        private readonly IDistributorService _distributorService;
        private readonly IRepository<ProspectAvatar> _prospectAvatarRepository;
        private readonly IRepository<Profile> _profileRepository;
        private readonly IAppSettings _appSettings;
        private readonly IResourceManager _resourceManager;
        private readonly IDbContext _wtaDbContext;

        public ProfileImageMergingService(IDistributorService distributorService, IRepository<ProspectAvatar> prospectAvatarRepository, IRepository<ProfileBox> profileBoxRepository, IRepository<Profile> profileRepository, IAppSettings appSettings, IResourceManager resourceManager, IDbContext wtaDbContext)
        {
            _distributorService = distributorService;
            _prospectAvatarRepository = prospectAvatarRepository;
            _profileRepository = profileRepository;
            _appSettings = appSettings;
            _resourceManager = resourceManager;
            _wtaDbContext = wtaDbContext;
        }

        public byte[] MergeImageToProfile(long adaNo, short pageNum, PdfOffset image1Offset, PdfOffset image2Offset)
        {
            var distributor = _distributorService.GetDSSDistributors(new[] { adaNo }).FirstOrDefault();
            if (distributor == null)
            {
                throw new ABOException(_resourceManager.GetString("ProfileImageMerging.FailedToFindDistributor"));
            }

            var images = this.GetImageByAdaNo(distributor);
            var profileBin = this.GetDistributorPhysicalProfile(distributor);

            using (var memory = new MemoryStream())
            {
                PdfReader reader = new PdfReader(profileBin);
                PdfStamper stamper = new PdfStamper(reader, memory);
                PdfContentByte contentByte = stamper.GetOverContent(pageNum);

                if (images[0] != null)
                {
                    var image = iTextSharp.text.Image.GetInstance(images[0].img_streamdata);
                    image.SetAbsolutePosition(image1Offset.X,
                        reader.GetPageSize(pageNum).Height - image1Offset.Y - image.Height * image1Offset.Scale);

                    image.ScalePercent(image1Offset.Scale * 100);
                    contentByte.AddImage(image);
                }

                if (images[1] != null)
                {
                    var image = iTextSharp.text.Image.GetInstance(images[1].img_streamdata);
                    image.SetAbsolutePosition(image2Offset.X, reader.GetPageSize(pageNum).Height - image2Offset.Y - image.Height * image2Offset.Scale);

                    image.ScalePercent(image2Offset.Scale * 100);
                    contentByte.AddImage(image);
                }

                stamper.Close();

                return memory.ToArray();
            }
        }

        public ProspectAvatar[] GetImageByAdaNo(long adaNo, bool isReadOnly = true)
        {
            var distributor = _distributorService.GetDSSDistributors(new[] { adaNo }).FirstOrDefault();
            if (distributor == null)
            {
                throw new ABOException(_resourceManager.GetString("ProfileImageMerging.FailedToFindDistributor"));
            }

            return this.GetImageByAdaNo(distributor, isReadOnly);
        }

        public KeyValuePair<string, ProspectAvatar>[] GetImageByAdaNoWithDistId(long adaNo)
        {
            var distributor = _distributorService.GetDSSDistributors(new[] { adaNo }).FirstOrDefault();
            if (distributor == null)
            {
                throw new ABOException(_resourceManager.GetString("ProfileImageMerging.FailedToFindDistributor"));
            }

            ProspectAvatar[] avatars = this.GetImageByAdaNo(distributor);
            var dict = new KeyValuePair<string, ProspectAvatar>[2];
            if (!string.IsNullOrEmpty(distributor.ID1))
            {
                dict[0] = new KeyValuePair<string, ProspectAvatar>(distributor.ID1, avatars[0]);
            }

            if (!string.IsNullOrEmpty(distributor.ID2))
            {
                dict[1] = new KeyValuePair<string, ProspectAvatar>(distributor.ID1, avatars[1]);
            }

            return dict;
        }


        public ProspectAvatar[] GetImageByAdaNo(Distributor distributor, bool isReadOnly = true)
        {
            const string imgStatus = "A";
            var avatars = new ProspectAvatar[2];
            short numOfNoImg = 0;

            var repositoryTable = isReadOnly
                ? _prospectAvatarRepository.TableNoTracking
                : _prospectAvatarRepository.Table;
            if (!string.IsNullOrEmpty(distributor.ID1))
            {
                avatars[0] = repositoryTable.FirstOrDefault(c => c.img_cmnd_id == distributor.ID1 && c.img_status == imgStatus);

                if (avatars[0] == null)
                {
                    numOfNoImg++;
                }
            }

            if (!string.IsNullOrEmpty(distributor.ID2))
            {
                avatars[1] = repositoryTable.FirstOrDefault(c => c.img_cmnd_id == distributor.ID2 && c.img_status == imgStatus);

                if (avatars[1] == null)
                {
                    numOfNoImg++;
                }
            }

            if (numOfNoImg == 2)
            {
                throw new ABOException(_resourceManager.GetString("ProfileImageMerging.FailedToFindImages"));
            }

            return avatars;
        }

        public byte[] GetDistributorPhysicalProfile(long adaNo)
        {
            var distributor = _distributorService.GetDSSDistributors(new[] { adaNo }).FirstOrDefault();
            if (distributor == null)
            {
                throw new ABOException(_resourceManager.GetString("ProfileImageMerging.FailedToFindDistributor"));
            }

            return this.GetDistributorPhysicalProfile(distributor);
        }

        public void UploadUsersImage(long adaNo, ProspectAvatar avatar1, ProspectAvatar avatar2)
        {
            var currentAvatars = this.GetImageByAdaNo(adaNo, false);
            var inactiveToken = "I";

            using (var transaction = _wtaDbContext.BeginDbTransaction())
            {
                try
                {
                    for (int i = 0; i < 2; i++)
                    {
                        var clonedAvatar = i == 0 ? (ProspectAvatar)avatar1.Clone() : (ProspectAvatar) avatar2.Clone();
                        if (clonedAvatar.img_streamdata != null && clonedAvatar.img_streamdata.Length != 0 && !string.IsNullOrEmpty(currentAvatars[i].img_cmnd_id))
                        {
                            var currentCmndId = currentAvatars[i].img_cmnd_id;
                            var inactiveAvatar = _prospectAvatarRepository.Table.FirstOrDefault(
                                c => c.img_cmnd_id == currentCmndId && c.img_status == inactiveToken);

                            if (inactiveAvatar != null)
                            {
                                _prospectAvatarRepository.Delete(inactiveAvatar);
                            }

                            var remoteAvatar1 = (ProspectAvatar)currentAvatars[i].Clone();
                            remoteAvatar1.img_status = "I";

                            _prospectAvatarRepository.Delete(currentAvatars[i]);
                            _prospectAvatarRepository.Insert(remoteAvatar1);

                            clonedAvatar.img_cmnd_id = remoteAvatar1.img_cmnd_id;
                            clonedAvatar.img_status = "A";
                            clonedAvatar.img_file_path = remoteAvatar1.img_file_path;

                            _prospectAvatarRepository.Insert(clonedAvatar);
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public byte[] GetDistributorPhysicalProfile(Distributor distributor)
        {
            var profile =
                _profileRepository.TableNoTracking.IncludeTable(c => c.ProfileBox)
                    .FirstOrDefault(c => c.DistNumber == distributor.DistributorNo && c.ProfileBox.TypeId == 1);
            if (profile == null)
            {
                throw new ABOException(_resourceManager.GetString("ProfileImageMerging.FailedToFindProfile"));
            }

            var filePath = Path.Combine(_appSettings.ProfileBoxFolder, profile.ProfileBox.Name + "\\" + profile.FileName);
            if (File.Exists(filePath))
            {
                return File.ReadAllBytes(filePath);
            }

            throw new ABOException(string.Format(_resourceManager.GetString("ProfileImageMerging.ContractFileNotFound"), profile.FileName));
        }
    }
}
