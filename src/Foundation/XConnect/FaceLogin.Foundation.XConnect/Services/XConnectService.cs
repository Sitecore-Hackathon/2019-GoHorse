using FaceLogin.Foundation.XConnect.Facets;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Collection.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaceLogin.Foundation.XConnect.Services
{
    public class XConnectService : IXConnectService
    {
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

    }
}