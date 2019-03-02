using System.Runtime.Serialization;
using FaceLogin.Foundation.Kairos.Responses;
using Kairos.Net;
using RestSharp;
using RestSharp.Deserializers;

namespace FaceLogin.Foundation.Kairos.Extensions
{
    public static class KairosClientExtensions
    {
        /// <summary>
        /// Remove subject from gallery
        /// </summary>
        /// <param name="kairosClient"></param>
        /// <param name="subjectId"></param>
        /// <param name="galleryId"></param>
        /// <returns></returns>
        public static GalleryRemoveResponse RemoveSubject(this KairosClient kairosClient, string subjectId, string galleryId)
        {
            var restClient = new RestClient("https://api.kairos.com");
            var restRequest1 = new RestRequest(Method.POST)
            {
                Resource = "gallery/remove_subject",
                RequestFormat = DataFormat.Json
            };
            restRequest1.AddHeader("app_id", kairosClient.ApplicationID);
            restRequest1.AddHeader("app_key", kairosClient.ApplicationKey);
            restRequest1.AddHeader("Content-Type", "application/json");
            var jsonDeserializer = new JsonDeserializer();
            restRequest1.AddBody(new
            {
                gallery_name = galleryId,
                subject_id = subjectId
            });
            var restRequest2 = restRequest1;
            var restResponse = restClient.Execute<GalleryRemoveResponse>((IRestRequest)restRequest2);
            try
            {
                return jsonDeserializer.Deserialize<GalleryRemoveResponse>((IRestResponse)restResponse);
            }
            catch (SerializationException ex)
            {
                throw new SerializationException("Error serializing JSON string. JSON string: " + restResponse.Content);
            }
        }

        /// <summary>
        /// Enroll advanced - accepts MultipleFaces
        /// </summary>
        /// <param name="kairosClient"></param>
        /// <param name="imageUrlOrBase64String"></param>
        /// <param name="subjectId"></param>
        /// <param name="galleryId"></param>
        /// <param name="selector"></param>
        /// <param name="multipleFaces"></param>
        /// <returns></returns>
        public static EnrollResponse Enroll(
            this KairosClient kairosClient,
            string imageUrlOrBase64String,
            string subjectId,
            string galleryId,
            string selector,
            bool multipleFaces)
        {
            var restClient = new RestClient("https://api.kairos.com");
            var restRequest1 = new RestRequest(Method.POST) {Resource = "enroll", RequestFormat = DataFormat.Json};
            restRequest1.AddHeader("app_id", kairosClient.ApplicationID);
            restRequest1.AddHeader("app_key", kairosClient.ApplicationKey);
            restRequest1.AddHeader("Content-Type", "application/json");
            var jsonDeserializer = new JsonDeserializer();
            restRequest1.AddBody(new
            {
                image = imageUrlOrBase64String,
                subject_id = subjectId,
                gallery_name = galleryId,
                multiple_faces = multipleFaces ? "true" : "false",
                selector = selector.ToUpper()
            });
            var restRequest2 = restRequest1;
            var restResponse = restClient.Execute<EnrollResponse>(restRequest2);
            try
            {
                return jsonDeserializer.Deserialize<EnrollResponse>(restResponse);
            }
            catch (SerializationException ex)
            {
                throw new SerializationException("Error serializing JSON string. JSON string: " + restResponse.Content);
            }
        }
    }
}