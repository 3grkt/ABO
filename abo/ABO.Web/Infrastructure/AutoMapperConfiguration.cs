using ABO.Core;
using ABO.Core.Domain;
using ABO.Core.Infrastructure;
using ABO.Core.SearchCriteria;
//using AOB.Core.SearchCriteria;
using ABO.Services.Localization;
using ABO.Web.Extensions;
using ABO.Web.Models.DataPurges;
using ABO.Web.Models.Distributors;
using ABO.Web.Models.ProfileBox;
using ABO.Web.Models.Profiles;
using ABO.Web.Models.ProfileScans;
using ABO.Web.Models.ProfileTypes;
using ABO.Web.Models.User;
//using AOB.Web.Models.Assets;
//using AOB.Web.Models.Inventories;
//using AOB.Web.Models.Users;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABO.Web.Infrastructure
{
    public class AutoMapperConfiguration
    {
        private static Lazy<IResourceManager> _resourceManagerLazyObject = new Lazy<IResourceManager>(() => EngineContext.Current.Resolve<IResourceManager>());
        protected static IResourceManager ResourceManager
        {
            get { return _resourceManagerLazyObject.Value; }
        }

        public static void ConfigMapping()
        {
            // Profile Types
            Mapper.CreateMap<ProfileType, ProfileTypeModel>();
            Mapper.CreateMap<ProfileTypeModel, ProfileType>();

            // Profiles
            Mapper.CreateMap<Distributor, ProfileIndexModel.DistributorModel>()
                .ForMember(x => x.WarehouseName, mo => mo.MapFrom(x => x.Warehouse.WarehouseName))
                .ForMember(x => x.Address, mo => mo.MapFrom(x => CommonHelper.GetAddressString(x.Address1, x.Address2, x.Address3, x.Address4)));

            Mapper.CreateMap<ABO.Core.Domain.Profile, ProfileIndexModel.ProfileModel>()
                .ForMember(x => x.TypeName, mo => mo.MapFrom(x => x.ProfileType.Name))
                .ForMember(x => x.BoxName, mo => mo.MapFrom(x => x.ProfileBox.Name))
                .ForMember(x => x.CreatedByUserName, mo => mo.MapFrom(x => x.ProfileBox.User.UserName))
                .ForMember(x => x.WarehouseName, mo => mo.MapFrom(x => x.Warehouse.WarehouseName))
                .ForMember(x => x.StatusName, mo => mo.MapFrom(x => x.Status.Name));

            //Profile Box
            Mapper.CreateMap<ABO.Core.Domain.ProfileBox, ProfileBoxGridModel>()
              .ForMember(x => x.UserName, mo => mo.MapFrom(x => x.User.UserName))
              .ForMember(x => x.WarehouseName, mo => mo.MapFrom(x => x.Warehouse.WarehouseName))
              .ForMember(x => x.Status, mo => mo.MapFrom(x => x.Status.Name));
            Mapper.CreateMap<ProfileBoxSearchModel, ABO.Core.SearchCriteria.ProfileBoxSearchCriteria>();
            Mapper.CreateMap<ABO.Core.Domain.ProfileBox, ProfileBoxGridModel>()
                 .ForMember(x => x.Id, mo => mo.MapFrom(x => x.Id))
                 .ForMember(x => x.CreatedDate, mo => mo.MapFrom(x => x.CreatedDate))
                 .ForMember(x => x.UserName, mo => mo.MapFrom(x => x.User.UserName))
                 .ForMember(x => x.WarehouseName, mo => mo.MapFrom(x => x.Warehouse.WarehouseName))
                 .ForMember(x => x.Status, mo => mo.MapFrom(x => x.Status.Name))
                 .ForMember(x => x.ADACount, mo => mo.MapFrom(x => x.ADACount))
                 .ForMember(x => x.Name, mo => mo.MapFrom(x => x.Name));



            Mapper.CreateMap<ABO.Core.Domain.ProfileBox, ProfileBoxDetailModel>()
                .ForMember(x => x.Status, mo => mo.MapFrom(x => x.StatusId))
                .ForMember(x => x.StatusName, mo => mo.MapFrom(x => x.Status.Name))
                .ForMember(x => x.Warehouse, mo => mo.MapFrom(x => x.WarehouseId))
                .ForMember(x => x.WarehouseName, mo => mo.MapFrom(x => x.Warehouse.WarehouseName.Trim()))
                .ForMember(x => x.ProfileType, mo => mo.MapFrom(x => x.TypeId))
                .ForMember(x => x.ProfileTypeName, mo => mo.MapFrom(x => x.ProfileType.Name))
                .ForMember(x => x.Creator, mo => mo.MapFrom(x => x.User.UserName))
                .ForMember(x => x.OfficeName, mo => mo.MapFrom(x => x.Office.WarehouseName.Trim()))
                .ForMember(x => x.Location, mo => mo.MapFrom(x => x.LocationId))
                .ForMember(x => x.ProfileCount, mo => mo.MapFrom(x => x.ProfileCount))
                .ForMember(x => x.FolderPath, mo => mo.MapFrom(x => x.ScannedFolder))
                .ForMember(x => x.OfficeId, mo => mo.MapFrom(x => x.OfficeId))
                .ForMember(x => x.Scanned, mo => mo.MapFrom(x => x.ProfileType.Scanned))
                .ForMember(x => x.LocationName, mo => mo.MapFrom(x => x.Location.Name));

            Mapper.CreateMap<ABO.Core.Domain.Profile, ProfileModel>()
                .ForMember(x => x.StatusName, mo => mo.MapFrom(x => x.Status.Name));



            Mapper.CreateMap<ABO.Core.Domain.Profile, ProfileBoxChangeModel>()
                .ForMember(x => x.AvailableBoxes, mo => mo.Ignore());

            Mapper.CreateMap<ABO.Core.Domain.Profile, ProfileADAChangeModel>()
                .ForMember(x => x.NewDistNumber, mo => mo.Ignore());

            // Profile Scan
            Mapper.CreateMap<ProfileScan, ProfileScanModel>()
                .ForMember(x => x.BoxName, mo => mo.MapFrom(x => x.ProfileBox.Name))
                .ForMember(x => x.Folder, mo => mo.MapFrom(x => x.ProfileBox.ScannedFolder))
                .ForMember(x => x.Description, mo => mo.MapFrom(x => x.Description.Replace("\r\n", "<br />")));

            Mapper.CreateMap<ProfileScanSearchModel, ProfileScanSearchCriteria>()
                .ForMember(x => x.EndDate, mo => mo.MapFrom(x => x.EndDate.EndOfDate()));

            // Distributors
            Mapper.CreateMap<DistributorUpdate, NotScannedModel.DistributorUpdateModel>()
                .ForMember(x => x.DistName, mo => mo.MapFrom(x => x.Distributor.Name))
                .ForMember(x => x.JoinDate, mo => mo.MapFrom(x => x.Distributor.JoinDate))
                .ForMember(x => x.ExpiryDate, mo => mo.MapFrom(x => x.Distributor.ExpiryDate));

            Mapper.CreateMap<DistributorLetter, DistributorGridModel>()
                .ForMember(x => x.DistNumber, mo => mo.MapFrom(x => x.DistNumber))
                .ForMember(x => x.DistName, mo => mo.MapFrom(x => x.Distributor.Name))
                .ForMember(x => x.DistAddress, mo => mo.MapFrom(x => x.Distributor.Address1 + " " + x.Distributor.Address2 + " " + x.Distributor.Address3 + " " + x.Distributor.Address4))
                .ForMember(x => x.SponsorAddress, mo => mo.MapFrom(x => x.Distributor1.Address1 + " " + x.Distributor1.Address2 + " " + x.Distributor1.Address3 + " " + x.Distributor1.Address4))
                .ForMember(x => x.PlatiumAddress, mo => mo.MapFrom(x => x.Distributor2.Address1 + " " + x.Distributor2.Address2 + " " + x.Distributor2.Address3 + " " + x.Distributor2.Address4))
                .ForMember(x => x.OldDistAddress, mo => mo.MapFrom(x => x.Distributor3.Address1 + " " + x.Distributor3.Address2 + " " + x.Distributor3.Address3 + " " + x.Distributor3.Address4))
                .ForMember(x => x.DistTelephone, mo => mo.MapFrom(x => x.Distributor.Telephone))
                .ForMember(x => x.SponsorTelephone, mo => mo.MapFrom(x => x.Distributor1.Telephone))
                .ForMember(x => x.PlatiumTelephone, mo => mo.MapFrom(x => x.Distributor2.Telephone))
                .ForMember(x => x.OldDistTelephone, mo => mo.MapFrom(x => x.Distributor3.Telephone))
                .ForMember(x => x.SponsorNum, mo => mo.MapFrom(x => x.SponsorId))
                .ForMember(x => x.SponsorName, mo => mo.MapFrom(x => x.Distributor1.Name))
                .ForMember(x => x.LetterDate, mo => mo.MapFrom(x => x.LetterDate))
                .ForMember(x => x.OldDistNumber, mo => mo.MapFrom(x => x.OldDistNumber))
                .ForMember(x => x.OldDistName, mo => mo.MapFrom(x => x.Distributor3.Name))
                .ForMember(x => x.Creator, mo => mo.MapFrom(x => x.User.UserName))
                .ForMember(x => x.Warehouse, mo => mo.MapFrom(x => x.Warehouse.WarehouseName))
                .ForMember(x => x.PlatiumNumber, mo => mo.MapFrom(x => x.PlatiniumSponsorId))
                .ForMember(x => x.PlatiumName, mo => mo.MapFrom(x => x.Distributor2.Name));


            // Data Purge
            Mapper.CreateMap<DataPurge, DataPurgeModel>()
                .ForMember(x => x.UserName, mo => mo.MapFrom(x => x.User.UserName));

            Mapper.CreateMap<DataPurgeModel, DataPurge>()
                .ForMember(x => x.FileCount, mo => mo.Ignore());

            // user
            Mapper.CreateMap<User, UserGridModel>()
                .ForMember(x => x.UserId, mo => mo.MapFrom(x => x.Id))
                .ForMember(x => x.UserName, mo => mo.MapFrom(x => x.UserName))
                .ForMember(x => x.UserFullName, mo => mo.MapFrom(x => x.FullName))
                .ForMember(x => x.WarehouseName, mo => mo.MapFrom(x => x.Warehouse.WarehouseName))
                .ForMember(x => x.Role, mo => mo.MapFrom(x => x.Role.Name));
        }
    }
}