﻿using Sitecore.XConnect;
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
        bool UpdateContactConsent(Contact contact, bool consent);
        bool UpdateContactAvatar(Contact contact, string avatar);
        object FaceLogin(string identifierName, string base64);
        void Identify(string identifierName, string identifierValue);
        bool UpdateContactPersonal(Contact contact, string firstName, string lastName);
    }
}