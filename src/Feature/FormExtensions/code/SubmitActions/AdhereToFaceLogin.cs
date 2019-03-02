using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FaceLogin.Feature.FormExtensions.Fields;
using FaceLogin.Feature.Login;
using FaceLogin.Foundation.Kairos;
using FaceLogin.Foundation.Kairos.Extensions;
using FaceLogin.Foundation.Kairos.Services;
using FaceLogin.Foundation.XConnect.Extensions;
using FaceLogin.Foundation.XConnect.Facets;
using FaceLogin.Foundation.XConnect.Services;
using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;
using static System.FormattableString;

namespace FaceLogin.Feature.FormExtensions.SubmitActions
{
    public class AdhereToFaceLogin : SubmitActionBase<string>
    {
        public AdhereToFaceLogin(ISubmitActionData submitActionData) : base(submitActionData)
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
                    // Get Adhere field
                    var adhereField = (AdhereToFaceLoginViewModel)
                        formSubmitContext.Fields.FirstOrDefault(p => p.GetType() == typeof(AdhereToFaceLoginViewModel));
                    if (adhereField == null)
                        return true;

                    // Adhere Logic
                    var xConnectService = DependencyResolver.Current.GetService<IXConnectService>();
                    var kairosService = DependencyResolver.Current.GetService<IKairosService>();

                    var contact = xConnectService.GetCurrentContact(AdhereToFaceLoginFacet.DefaultFacetKey);
                    var subjectId = $"{Login.Constants.Identifiers.FaceIdentifier}_{contact.Id}";

                    var consentObject = contact.GetFacet<AdhereToFaceLoginFacet>(AdhereToFaceLoginFacet.DefaultFacetKey);
                    var isAllowedInContact = consentObject != null && consentObject.FaceRecognitionAllowed;
                    var isAllowedInForm = adhereField.Value;
                    if (isAllowedInForm != isAllowedInContact)
                    {
                        if (isAllowedInForm)
                        {
                            // Enable in Kairos
                            var avatar = xConnectService.GetContactAvatar();
                            var base64 = avatar != null ? avatar.GetPictureBase64() : "";
                            var enrollResult = kairosService.EnrollPictureString(base64, subjectId);
                            if (!string.IsNullOrEmpty(enrollResult))
                            {
                                formSubmitContext.Errors.Add(new FormActionError { ErrorMessage = enrollResult });
                                return false;
                            }

                            // Save new Identifier to XDB
                            xConnectService.AddContactIdentifier(contact, Login.Constants.Identifiers.FaceIdentifier, subjectId);
                        }
                        else
                        {
                            // Disable in Kairos
                            var removeResponse = kairosService.Client().RemoveSubject(subjectId, Configurations.Config.KairosGalleryName);
                            if (removeResponse.Errors.Any())
                            {
                                var msg = new StringBuilder();
                                foreach (var error in removeResponse.Errors)
                                    msg.AppendLine($"{error.Message} ({error.ErrCode})");
                                formSubmitContext.Errors.Add(new FormActionError { ErrorMessage = msg.ToString() });
                                return false;
                            }

                            // Remove Identifier from XDB
                            xConnectService.RemoveContactIdentifier(contact, Constants.Identifiers.FaceIdentifier);
                        }

                        // Update XConnect
                        if (!xConnectService.UpdateContactBookshelfConsent(contact, isAllowedInForm))
                        {
                            formSubmitContext.Errors.Add(new FormActionError { ErrorMessage = "Error updating Adhere to Face Login" });
                            return false;
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.LogError(
                        Invariant(
                            $"Form {formSubmitContext.FormId} submitted with errors: {string.Join(", ", formSubmitContext.Errors.Select(t => t.ErrorMessage))}."),
                        this);
                    return false;
                }
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