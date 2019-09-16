using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace SheilaWard_BugTracker.Helpers
{
    public class ImageHelpers
    {
        public static bool IsWebFriendlyImage(HttpPostedFileBase file)
        {
            if (file == null) return false;
            if (file.ContentLength > 2 * 1024 * 1024 || file.ContentLength < 1024) return false;
            try
            {
                using (var img = Image.FromStream(file.InputStream))
                {
                    //File.Move(file, Path.ChangeExtension(file, Path.GetExtension(file).ToLower()));
                    return ImageFormat.Jpeg.Equals(img.RawFormat) ||
                           ImageFormat.Png.Equals(img.RawFormat) ||
                           ImageFormat.Icon.Equals(img.RawFormat) ||
                           ImageFormat.Bmp.Equals(img.RawFormat) ||
                           ImageFormat.Gif.Equals(img.RawFormat);
                }
            }
            catch
            {
                return false;
            }

        }

        public static bool IsValidAttachment(HttpPostedFileBase file)
        {
            try
            {
                if (file == null)
                    return false;

                if (file.ContentLength > 5 * 1024 * 1024)     // || file.ContentLength < 1024)
                    return false;

                var extValid = false;
                foreach (var ext in System.Web.Configuration.WebConfigurationManager.AppSettings["AllowedAttachmentExtensions"].Split(','))
                {
                    if (Path.GetExtension(file.FileName).ToLower() == ext)
                    {
                        extValid = true;
                        break;
                    }
                }

                return IsWebFriendlyImage(file) || extValid;
            }
            catch
            {
                return false;
            }
        }

        public static string GetIconPath(string filePath)
        {
            switch (Path.GetExtension(filePath).ToLower())
            {
                case ".png":
                case ".bmp":
                case ".tif":
                case ".ico":
                case ".jpg":
                case ".jpeg":
                    return filePath;
                case ".pdf":
                    return "/Images/PDF.png";
                case ".doc":
                    return "/Images/doc.png";
                case ".docx":
                    return "/Images/docx.png";
                case ".xls":
                    return "/Images/xls.ico";
                case ".xlsx":
                    return "/Images/xlsx.ico";
                case ".zip":
                    return "/Images/zip.png";
                default:
                    return "/Images/other.png";
            }

        }
    }
}