using Sitecore.XConnect.Collection.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaceLogin.Foundation.XConnect.Extensions
{
    public static class AvatarExtensions
    {
        /// <summary>
        /// Get Avatar content to be rendered as <img src="{0}"></img>
        /// </summary>
        /// <param name="avatar"></param>
        /// <returns></returns>
        public static string GetPictureForImage(this Avatar avatar)
        {
            return avatar != null && avatar.Picture.Any()
                ? $"data:image/jpeg;base64, {Convert.ToBase64String(avatar.Picture)}"
                : "";
        }

        /// <summary>
        /// Get Base64 of the Picture
        /// </summary>
        /// <param name="avatar"></param>
        /// <returns></returns>
        public static string GetPictureBase64(this Avatar avatar)
        {
            return avatar != null && avatar.Picture.Any()
                ? Convert.ToBase64String(avatar.Picture)
                : "";
        }
    }
}