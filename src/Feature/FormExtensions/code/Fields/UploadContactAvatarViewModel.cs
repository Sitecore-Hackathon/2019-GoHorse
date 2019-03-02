using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.ExperienceForms.Mvc.Models.Fields;

namespace FaceLogin.Feature.FormExtensions.Fields
{
    [Serializable]
    public class UploadContactAvatarViewModel : InputViewModel<HttpPostedFileBase>
    {
        [DataType(DataType.Upload)]
        public override HttpPostedFileBase Value
        {
            get;
            set;
        }
        public string AvatarUrl { get; set; }

        protected override void InitItemProperties(Item item)
        {
            // on load
            base.InitItemProperties(item);
        }

        protected override void UpdateItemFields(Item item)
        {
            // On Save
            base.UpdateItemFields(item);
        }
    }
}