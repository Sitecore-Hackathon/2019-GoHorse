using System.Collections.Generic;

namespace FaceLogin.Foundation.Kairos.Responses
{
    public class GalleryRemoveResponse
    {
        public GalleryRemoveResponse()
        {
            this.Errors = new List<global::Kairos.Net.Errors>();
        }

        public string status { get; set; }
        public string message { get; set; }

        public List<global::Kairos.Net.Errors> Errors { get; set; }
    }
}