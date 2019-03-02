using System.Web.Mvc;
using FaceLogin.Foundation.XConnect.Facets;
using FaceLogin.Foundation.XConnect.Services;
using Sitecore.Data.Items;
using Sitecore.ExperienceForms.Mvc.Models.Fields;

namespace FaceLogin.Feature.FormExtensions.Fields
{
    public class AdhereToFaceLoginViewModel : CheckBoxViewModel
    {
        protected override void InitItemProperties(Item item)
        {
            // on load
            base.InitItemProperties(item);

            var xConnectService = DependencyResolver.Current.GetService<IXConnectService>();
            var contact = xConnectService.GetCurrentContact(AdhereToFaceLoginFacet.DefaultFacetKey);
            if (contact == null)
                return;
            var consentObject = contact.GetFacet<AdhereToFaceLoginFacet>(AdhereToFaceLoginFacet.DefaultFacetKey);
            var isAllowedInContact = consentObject != null && consentObject.FaceRecognitionAllowed;
            Value = isAllowedInContact;
        }

        protected override void UpdateItemFields(Item item)
        {
            // On Save
            base.UpdateItemFields(item);
        }
    }
}