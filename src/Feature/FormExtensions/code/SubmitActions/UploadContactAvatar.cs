using System;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;
using FaceLogin.Feature.FormExtensions.Fields;
using FaceLogin.Foundation.Kairos.Services;
using FaceLogin.Foundation.XConnect.Services;
using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;
using Sitecore.XConnect.Collection.Model;
using static System.FormattableString;

namespace FaceLogin.Feature.FormExtensions.SubmitActions
{
    public class UploadContactAvatar : SubmitActionBase<string>
    {
        public UploadContactAvatar(ISubmitActionData submitActionData) : base(submitActionData)
        {
        }

        protected override bool TryParse(string value, out string target)
        {
            target = string.Empty;
            return true;
        }

        protected override bool Execute(string data, FormSubmitContext formSubmitContext)
        {
            Assert.ArgumentNotNull(formSubmitContext, nameof(formSubmitContext));
            if (!formSubmitContext.HasErrors)
            {
                try
                {
                    // Get upload field
                    var uploadField = (UploadContactAvatarViewModel)
                        formSubmitContext.Fields.FirstOrDefault(p => p.GetType() == typeof(UploadContactAvatarViewModel));
                    if (uploadField?.Value == null)
                        return true;

                    // Get base64
                    var image = Image.FromStream(uploadField.Value.InputStream);
                    var imageService = DependencyResolver.Current.GetService<IImageService>();
                    var base64 = imageService.GetBase64FromImage(image);

                    // Save to Contact
                    var xConnectService = DependencyResolver.Current.GetService<IXConnectService>();
                    var currentContact =
                        xConnectService.GetCurrentContact(PersonalInformation.DefaultFacetKey, Avatar.DefaultFacetKey);
                    xConnectService.UpdateContactAvatar(currentContact, base64);
                }
                catch (Exception)
                {
                    Logger.LogError(
                        Invariant(
                            $"Form {formSubmitContext.FormId} submitted with errors: {string.Join(", ", formSubmitContext.Errors.Select(t => t.ErrorMessage))}."),
                        this);
                    return false;
                }

                return true;
            }
            else
            {
                Logger.Warn(
                    Invariant(
                        $"Form {formSubmitContext.FormId} submitted with errors: {string.Join(", ", formSubmitContext.Errors.Select(t => t.ErrorMessage))}."),
                    this);
            }

            return true;
        }
    }
}