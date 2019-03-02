using FaceLogin.Foundation.XConnect.Facets;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Collection.Model;
using System.Linq;
using System.Text;
using FaceLogin.Foundation.Kairos;
using FaceLogin.Foundation.Kairos.Services;
using Sitecore.Analytics.Tracking;
using Contact = Sitecore.XConnect.Contact;

namespace FaceLogin.Foundation.XConnect.Services
{
    public class XConnectService : IXConnectService
    {
        private readonly IImageService _imageService;
        private readonly IKairosService _kairosService;

        public XConnectService(IImageService imageService, IKairosService kairosService)
        {
            _imageService = imageService;
            _kairosService = kairosService;
        }

        public Contact GetCurrentContact()
        {
            return GetCurrentContact(
                PersonalInformation.DefaultFacetKey,
                AdhereToFaceLoginFacet.DefaultFacetKey,
                Avatar.DefaultFacetKey
            );
        }

        public Contact GetCurrentContact(params string[] facets)
        {
            using (var client = Sitecore.XConnect.Client.Configuration.SitecoreXConnectClientConfiguration.GetClient())
            {
                try
                {
                    var contactId = Sitecore.Analytics.Tracker.Current != null &&
                                    Sitecore.Analytics.Tracker.Current.Contact != null ?
                        Sitecore.Analytics.Tracker.Current.Contact.ContactId.ToString("N") : "";

                    if (string.IsNullOrEmpty(contactId))
                        return null;
                    // Get from current contact 
                    var trackerIdentifier = new IdentifiedContactReference(
                        Sitecore.Analytics.XConnect.DataAccess.Constants.IdentifierSource,
                        contactId);
                    var contact = client.Get<Contact>(trackerIdentifier,
                        new ContactExpandOptions(facets));
                    return contact;
                }
                catch (XdbExecutionException ex)
                {
                    // Manage exception
                    Sitecore.Diagnostics.Log.Error(
                        $"[FaceLogin] Error Getting Contact",
                        ex, ex.GetType());
                }
            }
            return null;
        }

        public bool ContactHasAvatar(Contact contact = null)
        {
            var avatar = GetContactAvatar(contact);
            return avatar != null && avatar.Picture.Any();
        }

        public Avatar GetContactAvatar(Contact contact = null)
        {
            using (var client = Sitecore.XConnect.Client.Configuration.SitecoreXConnectClientConfiguration.GetClient())
            {
                try
                {
                    if (contact == null)
                    {
                        var contactId = Sitecore.Analytics.Tracker.Current != null &&
                                        Sitecore.Analytics.Tracker.Current.Contact != null ?
                            Sitecore.Analytics.Tracker.Current.Contact.ContactId.ToString("N") : "";

                        if (!string.IsNullOrEmpty(contactId))
                        {
                            // Get from current contact 
                            var trackerIdentifier = new IdentifiedContactReference(
                                Sitecore.Analytics.XConnect.DataAccess.Constants.IdentifierSource,
                                contactId);
                            contact = client.Get<Contact>(trackerIdentifier,
                                new ContactExpandOptions(Avatar.DefaultFacetKey));
                        }
                    }

                    var avatar = contact?.GetFacet<Avatar>(Avatar.DefaultFacetKey);
                    return avatar;
                }
                catch (XdbExecutionException ex)
                {
                    // Manage exception
                    Sitecore.Diagnostics.Log.Error(
                        $"[FaceLogin] Error Getting Contact Avatar",
                        ex, ex.GetType());
                }
            }
            return null;
        }

        public bool AddContactIdentifier(Contact contact, string identifierName, string identifierValue)
        {
            using (var client = Sitecore.XConnect.Client.Configuration.SitecoreXConnectClientConfiguration.GetClient())
            {
                try
                {
                    if (contact == null)
                    {
                        // Get from current contact 
                        var trackerIdentifier = new IdentifiedContactReference(
                            Sitecore.Analytics.XConnect.DataAccess.Constants.IdentifierSource,
                            Sitecore.Analytics.Tracker.Current.Contact.ContactId.ToString("N"));
                        contact = client.Get<Contact>(trackerIdentifier,
                            new ContactExpandOptions(Avatar.DefaultFacetKey));
                    }

                    client.AddContactIdentifier(contact,
                        new ContactIdentifier(identifierName, identifierValue,
                            ContactIdentifierType.Known));
                    client.Submit();

                    return true;
                }
                catch (XdbExecutionException ex)
                {
                    // Manage exception
                    Sitecore.Diagnostics.Log.Error(
                        $"[FaceLogin] Error adding identifier {identifierName}='{identifierValue}'",
                        ex, ex.GetType());
                    return false;
                }
            }
        }

        public bool RemoveContactIdentifier(Contact contact, string identifierName)
        {
            using (var client = Sitecore.XConnect.Client.Configuration.SitecoreXConnectClientConfiguration.GetClient())
            {
                try
                {
                    if (contact == null)
                    {
                        // Get from current contact 
                        var trackerIdentifier = new IdentifiedContactReference(
                            Sitecore.Analytics.XConnect.DataAccess.Constants.IdentifierSource,
                            Sitecore.Analytics.Tracker.Current.Contact.ContactId.ToString("N"));
                        contact = client.Get<Contact>(trackerIdentifier,
                            new ContactExpandOptions(Avatar.DefaultFacetKey));
                    }

                    var identifierToRemove = contact.Identifiers.FirstOrDefault(x => x.Source == identifierName);
                    if (identifierToRemove == null)
                        return true;

                    client.RemoveContactIdentifier(contact, identifierToRemove);
                    client.Submit();
                    return true;
                }
                catch (XdbExecutionException ex)
                {
                    // Manage exception
                    Sitecore.Diagnostics.Log.Error(
                        $"[FaceLogin] Error removing identifier {identifierName}",
                        ex, ex.GetType());
                    return false;
                }
            }
        }

        public bool UpdateContactBookshelfConsent(Contact contact, bool consent)
        {
            using (var client = Sitecore.XConnect.Client.Configuration.SitecoreXConnectClientConfiguration.GetClient())
            {
                try
                {
                    // Retrieve contact
                    if (contact == null)
                        return false;

                    // Retrieve facet (or create one)
                    var facet = contact.GetFacet<AdhereToFaceLoginFacet>(AdhereToFaceLoginFacet.DefaultFacetKey) ??
                                new AdhereToFaceLoginFacet();

                    // Change facet properties
                    facet.FaceRecognitionAllowed = consent;

                    // Set the updated facet
                    client.SetFacet(contact, AdhereToFaceLoginFacet.DefaultFacetKey, facet);
                    client.Submit();
                    return true;
                }
                catch (XdbExecutionException ex)
                {
                    // Manage exception
                    Sitecore.Diagnostics.Log.Error(
                        $"[Bookshelf] Error updating avatar.",
                        ex, ex.GetType());
                    return false;
                }
            }
        }

        public bool UpdateContactAvatar(Contact contact, string avatar)
        {
            using (var client = Sitecore.XConnect.Client.Configuration.SitecoreXConnectClientConfiguration.GetClient())
            {
                try
                {
                    // Retrieve contact
                    if (contact == null)
                        return false;

                    // Retrieve facet (or create one)
                    var facet = contact.GetFacet<Avatar>(Avatar.DefaultFacetKey);

                    // Change facet properties
                    var byteArray = _imageService.Base64ToByteArray(avatar);
                    if (facet == null)
                        facet = new Avatar("image/jpeg", byteArray);
                    else
                    {
                        facet.MimeType = "image/jpeg";
                        facet.Picture = byteArray;
                    }

                    // Set the updated facet
                    client.SetAvatar(contact, facet);
                    client.Submit();
                    return true;
                }
                catch (XdbExecutionException ex)
                {
                    // Manage exception
                    Sitecore.Diagnostics.Log.Error(
                        $"[FaceLogin] Error updating avatar.",
                        ex, ex.GetType());
                    return false;
                }
            }
        }

        public object FaceLogin(string identifierName, string base64)
        {
            // Recognize Face
            var recognizeResponse = _kairosService.Client().Recognize(base64, Configurations.Config.KairosGalleryName);

            // Error handling
            if (recognizeResponse.Errors.Any())
            {
                var msg = new StringBuilder();
                foreach (var error in recognizeResponse.Errors)
                    msg.AppendLine($"{error.Message} ({error.ErrCode})");
                return new { success = false, message = msg.ToString(), faceError = true };
            }

            // Face recognized?
            var images = recognizeResponse.Images.FirstOrDefault();
            var isFaceRecognized = images?.Transaction != null && images.Transaction.status == "success";
            if (!isFaceRecognized)
                return new { success = false, message = "Your face has not been recognized" };

            // Will identify contact in Sitecore
            var subjectId = images.Transaction.subject_id;

            // Identify Contact
            Identify(identifierName, subjectId);

            // Return Success
            return new { success = true, message = "Success" };
        }

        public void Identify(string identifierName, string identifierValue)
        {
            if (!(Sitecore.Configuration.Factory.CreateObject("tracking/contactManager", true) is ContactManager manager))
                return;

            Sitecore.Analytics.Tracker.Current.Session.IdentifyAs(identifierName, identifierValue);
            Sitecore.Analytics.Tracker.Current.Session.Contact =
                manager.LoadContact(Sitecore.Analytics.Tracker.Current.Contact.ContactId);
        }
    }
}