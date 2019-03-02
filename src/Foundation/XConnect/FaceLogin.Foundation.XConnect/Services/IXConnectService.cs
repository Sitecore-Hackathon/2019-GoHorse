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
    }
}