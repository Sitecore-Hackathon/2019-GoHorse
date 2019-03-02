using FaceLogin.Foundation.XConnect.Facets;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Collection.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FaceLogin.Foundation.Kairos.Services;

namespace FaceLogin.Foundation.XConnect.Services
{
    public class XConnectService : IXConnectService
    {
        private IImageService _imageService;

        public XConnectService(IImageService imageService)
        {
            _imageService = imageService;
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
    }
}