using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FaceLogin.Foundation.XConnect.Extensions;
using FaceLogin.Foundation.XConnect.Services;
using Sitecore;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.ExperienceForms.Mvc.Models.Fields;

namespace FaceLogin.Feature.FormExtensions.Fields
{
    [Serializable]
    public class ContactAvatarViewModel : FieldViewModel
    {
        private const string ThumbnailWidthField = "Thumbnail Width";
        private const string ThumbnailHeightField = "Thumbnail Height";
        private const string ExtraParametersField = "Extra Parameters";
        public string ThumbnailWidth { get; set; }
        public string ThumbnailHeight { get; set; }
        public string ExtraParameters { get; set; }
        public string AvatarUrl { get; set; }

        public string Parameters
        {
            get
            {
                var parsed = HttpUtility.ParseQueryString(ExtraParameters);
                return parsed.AllKeys.Aggregate("", (current, key) => current + $"{key}=\"{parsed[key]}\" ");
            }
        }

        protected override void InitItemProperties(Item item)
        {
            // on load
            base.InitItemProperties(item);

            ThumbnailHeight = item.Fields[ThumbnailHeightField]?.Value;
            ThumbnailWidth = item.Fields[ThumbnailWidthField]?.Value;

            ExtraParameters = "";
            var paramsField = (NameValueListField)item.Fields[ExtraParametersField];
            if (paramsField != null)
                ExtraParameters = StringUtil.NameValuesToString(paramsField.NameValues, "&");

            // Load Avatar from Contact
            var xConnectService = DependencyResolver.Current.GetService<IXConnectService>();
            var avatar = xConnectService.GetContactAvatar();
            var avatarUrl = avatar != null ? avatar.GetPictureForImage() : "";
            AvatarUrl = avatarUrl;
        }

        protected override void UpdateItemFields(Item item)
        {
            // On Save
            base.UpdateItemFields(item);

            item.Fields[ThumbnailHeightField]?.SetValue(ThumbnailHeight, true);
            item.Fields[ThumbnailWidthField]?.SetValue(ThumbnailWidth, true);
            item.Fields[ExtraParametersField]?.SetValue(ExtraParameters, true);
        }
    }
}