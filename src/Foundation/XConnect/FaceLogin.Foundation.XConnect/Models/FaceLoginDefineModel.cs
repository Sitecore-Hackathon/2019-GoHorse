using FaceLogin.Foundation.XConnect.Facets;
using Sitecore.XConnect;
using Sitecore.XConnect.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaceLogin.Foundation.XConnect.Models
{
    public class FaceLoginDefineModel
    {
        private static XdbModel BuildModel()
        {
            var builder = new XdbModelBuilder("FaceLoginDefineModel", new XdbModelVersion(1, 0));
            builder.ReferenceModel(Sitecore.XConnect.Collection.Model.CollectionModel.Model);
            builder.DefineFacet<Contact, AdhereToFaceLoginFacet>(AdhereToFaceLoginFacet.DefaultFacetKey);
            return builder.BuildModel();
        }
    }
}