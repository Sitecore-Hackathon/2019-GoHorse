using Sitecore.XConnect;
using Sitecore.XConnect.Collection.Model;

namespace FaceLogin.Foundation.XConnect.Services
{
    public interface IXConnectService
    {
        Contact GetCurrentContact();
        Contact GetCurrentContact(params string[] facets);
        bool ContactHasAvatar(Contact contact = null);
        Avatar GetContactAvatar(Contact contact = null);
        bool AddContactIdentifier(Contact contact, string identifierName, string identifierValue);
        bool RemoveContactIdentifier(Contact contact, string identifierName);
        bool UpdateContactBookshelfConsent(Contact contact, bool consent);
        bool UpdateContactAvatar(Contact contact, string avatar);
    }
}