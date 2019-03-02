using System;
using Sitecore.XConnect;
using Sitecore.XConnect.Collection.Model;

namespace FaceLogin.Foundation.XConnect.Facets
{
    [FacetKey(DefaultFacetKey)]
    public class AdhereToFaceLoginFacet : Facet
    {
        public const string DefaultFacetKey = "AdhereToFaceLoginFacet";

        public bool FaceRecognitionAllowed { get; set; }
    }
}