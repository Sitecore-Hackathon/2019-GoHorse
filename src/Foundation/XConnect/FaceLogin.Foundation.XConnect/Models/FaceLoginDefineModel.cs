using FaceLogin.Foundation.XConnect.Facets;
using Sitecore.XConnect;
using Sitecore.XConnect.Schema;

namespace FaceLogin.Foundation.XConnect.Models
{
    public class FaceLoginDefineModel
    {
        public static XdbModel Model { get; } = BuildModel();

        private static XdbModel BuildModel()
        {
            var builder = new XdbModelBuilder("FaceLoginDefineModel", new XdbModelVersion(1, 0));
            builder.ReferenceModel(Sitecore.XConnect.Collection.Model.CollectionModel.Model);
            builder.DefineFacet<Contact, AdhereToFaceLoginFacet>(AdhereToFaceLoginFacet.DefaultFacetKey);
            return builder.BuildModel();
        }
    }
}