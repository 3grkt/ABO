using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO.Ports;
using System.Linq;
using System.Web;
using Autofac.Core.Lifetime;
using Microsoft.Ajax.Utilities;

namespace ABO.Web.Extensions
{
    public static class HttpPostedFileBaseExtensions
    {
        private static readonly Dictionary<string, ImageFormat> ImageMimeTypes = new Dictionary<string, ImageFormat>()
        {
            {"image/jpg", ImageFormat.Jpeg },
            {"image/jpeg", ImageFormat.Jpeg },
            {"image/pjpeg", ImageFormat.Jpeg },
            {"image/gif", ImageFormat.Gif },
            {"image/png", ImageFormat.Png },
            {"image/x-png", ImageFormat.Png },
            {"image/bmp", ImageFormat.Bmp }
        };

        private static readonly Dictionary<string, ImageFormat> ImageFileExtensions = new Dictionary<string, ImageFormat>()
        {
            {".jpg", ImageFormat.Jpeg },
            {".png", ImageFormat.Png },
            {".gif", ImageFormat.Gif },
            {".jpeg", ImageFormat.Jpeg },
            {".bmp", ImageFormat.Bmp }
        };

        public static bool IsImageFile(this HttpPostedFileBase postedFile)
        {
            var contentType = postedFile.ContentType.ToLower();

            if (ImageMimeTypes.All(x => x.Key != contentType))
            {
                return false;
            }

            if (ImageFileExtensions.All(x=> !postedFile.FileName.EndsWith(x.Key)))
            {
                return false;
            }

            return true;
        }

        public static ImageFormat GetImageFormat(this HttpPostedFileBase postedFile)
        {
            var contentType = postedFile.ContentType.ToLower();

            var pairMime = ImageMimeTypes.FirstOrDefault(x => x.Key == contentType);
            if (!pairMime.Equals(default(KeyValuePair<string, ImageFormat>)))
            {
                return pairMime.Value;
            }

            var pairExtension = ImageFileExtensions.FirstOrDefault(x => postedFile.FileName.EndsWith(x.Key));
            if (!pairExtension.Equals(default(KeyValuePair<string, ImageFormat>)))
            {
                return pairExtension.Value;
            }

            return ImageFormat.Jpeg;
        }
    }
}