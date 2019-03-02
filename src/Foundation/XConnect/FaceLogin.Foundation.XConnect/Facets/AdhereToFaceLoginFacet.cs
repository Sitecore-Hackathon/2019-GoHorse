using Sitecore.XConnect;

namespace FaceLogin.Foundation.XConnect.Facets
{
    [FacetKey(DefaultFacetKey)]
    public class AdhereToFaceLoginFacet : Facet
    {
        public const string DefaultFacetKey = "AdhereToFaceLoginFacet";

        public bool FaceRecognitionAllowed { get; set; }
    }
}