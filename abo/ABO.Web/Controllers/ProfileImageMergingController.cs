using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using ABO.Core;
using ABO.Core.Domain.WTA;
using ABO.Services.Distributors;
using ABO.Services.Localization;
using ABO.Services.Logging;
using ABO.Services.Profiles;
using ABO.Web.Extensions;
using ABO.Web.Framework.ActionResults;
using ABO.Web.Framework.Controllers;
using ABO.Web.Models.ProfileImageMerging;

namespace ABO.Web.Controllers
{
    public class ProfileImageMergingController : WebControllerBase
    {
        private readonly IProfileImageMergingService _profileImageMergingService;
        private readonly IDistributorService _distributorService;
        private readonly ILogger _logger;
        private readonly IAppSettings _appSettings;
        private readonly IResourceManager _resourceManager;

        public ProfileImageMergingController(IProfileImageMergingService profileImageMergingService, ILogger logger, IDistributorService distributorService, IWorkContext workContext, IAppSettings appSettings, IResourceManager resourceManager)
        {
            _profileImageMergingService = profileImageMergingService;
            _logger = logger;
            _distributorService = distributorService;
            _appSettings = appSettings;
            _resourceManager = resourceManager;
        }

        //
        // GET: /ProfileImageMerging/

        public ActionResult Index(ProfileImageMergingIndexModel model)
        {
            if (model == null)
            {
                model = new ProfileImageMergingIndexModel();
            }

            // return this.Submit(model);
            ViewData["UploadAvatarWidth"] = _appSettings.UploadAvatarWidth;
            ViewData["UploadAvatarHeight"] = _appSettings.UploadAvatarHeight;

            return View(model);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult LoadProfile(long adaNo)
        {
            try
            {
                var bin = _profileImageMergingService.GetDistributorPhysicalProfile(adaNo);

                return File(bin, "application/pdf");
            }
            catch (ABOException ex)
            {
                return new JsonErrorResult(ex.Message, HttpStatusCode.BadRequest);
            }

        }

        [HttpPost]
        public ActionResult LoadProspectAvatar(long adaNo)
        {
            try
            {
                var avatars = _profileImageMergingService.GetImageByAdaNoWithDistId(adaNo);
                var model = new ProspectAvatarModel
                {
                    DistNumber = adaNo,
                    FirstPersonImage = avatars[0],
                    SecondPersonImage = avatars[1]
                };

                return PartialView("_LoadProspectAvatar", model);
            }
            catch (ABOException ex)
            {
                _logger.Error("Error occurred when loading prospect avatar: ", ex);
                return PartialView("_LoadProspectAvatar", new ProspectAvatarModel());
            }

        }

        [HttpPost]
        public ActionResult GetDistIdsPerAdaNo(long adaNo)
        {
            try
            {
                var dist = _distributorService.GetDSSDistributors(new long[] { adaNo });
                if (dist.Count == 1)
                {
                    return Json(new { Id1 = dist[0].ID1, Id2 = dist[0].ID2 });
                }

                return new JsonErrorResult(_resourceManager.GetString("ProfileImageMerging.FailedToFindDistributor"), HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return new JsonErrorResult(ex.Message, HttpStatusCode.NotFound);
            }
        }

        public ActionResult Submit(ProfileImageMergingIndexModel model)
        {

            byte[] result;

            try
            {
                result = _profileImageMergingService.MergeImageToProfile(model.DistNumber, model.ProfilePageIndex, model.UndersignedPoint,
                    model.SecondPersonPoint);

                if (result == null)
                {
                    throw new NullReferenceException();
                }
            }
            catch (ABOException ex)
            {
                if (ex.ExceptionType == AboExceptionType.Warning)
                {
                    WarningNotification(ex.Message);

                    model.SupressWarning = true;
                    ModelState.Clear();
                    return View("Index", model);
                }

                ErrorNotification(ex.Message);
                return View("Index", model);
            }
            catch (Exception ex)
            {
                _logger.Error("Error occurred when merging image: ", ex);
                return PageError();
            }

            var cd = new ContentDisposition()
            {
                FileName = model.DistNumber + ".pdf",
                Inline = false
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(result, "application/pdf");
        }

        [HttpPost]
        public ActionResult UploadImage(long adaNo, HttpPostedFileBase file1, HttpPostedFileBase file2)
        {
            try
            {
                ProspectAvatar avatar1 = new ProspectAvatar();
                if (file1 != null && file1.IsImageFile())
                {
                    avatar1.img_streamdata = ConvertResizedImageToByteArray(file1);
                    avatar1.img_extension = file1.GetImageFormat().ToString().ToLower();
                }

                ProspectAvatar avatar2 = new ProspectAvatar();
                if (file2 != null && file2.IsImageFile())
                {
                    avatar2.img_streamdata = ConvertResizedImageToByteArray(file2);
                    avatar2.img_extension = file2.GetImageFormat().ToString().ToLower();
                }

                if (file1 == null && file2 == null)
                {
                    return new JsonErrorResult(_resourceManager.GetString("ProfileImageMerging.UploadImage.NoFile"), HttpStatusCode.BadRequest);
                }

                _profileImageMergingService.UploadUsersImage(adaNo, avatar1, avatar2);

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.Error("Error when uploading users' images: ", ex);
                return new JsonErrorResult(ex.Message, HttpStatusCode.InternalServerError);
            }
            
        }

        #region Utilities
        private byte[] ConvertResizedImageToByteArray(HttpPostedFileBase input)
        {
            using (var image = Image.FromStream(input.InputStream, true, true))
            {
                var bitmap = image.ResizeImage(_appSettings.UploadAvatarWidth, _appSettings.UploadAvatarHeight).ToByteArray(input.GetImageFormat());

                return bitmap;
            }
        } 
        #endregion
    }
}
