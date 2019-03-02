using Kairos.Net;
using System;
using System.Linq;
using System.Text;
using FaceLogin.Foundation.Kairos.Extensions;

namespace FaceLogin.Foundation.Kairos.Services
{
    public class KairosService : IKairosService
    {
        private readonly KairosClient _kairosClient;       

        public KairosService()
        {
            _kairosClient = new KairosClient
            {
                ApplicationID = Configurations.Config.ApplicationId,
                ApplicationKey = Configurations.Config.ApplicationKey
            };
        }

        public KairosClient Client()
        {
            return _kairosClient;
        }

        public string EnrollPictureString(string base64, string subjectId)
        {
            var ret = EnrollPicture(base64, subjectId);

            var success = ret?.GetType().GetProperty("success")?.GetValue(ret, null);
            var message = (string)ret?.GetType().GetProperty("message")?.GetValue(ret, null);

            return message;
        }

        /// <summary>
        /// Enroll a picture in Kairos with all validations
        /// </summary>
        /// <param name="base64"></param>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        public object EnrollPicture(string base64, string subjectId)
        {
            var detectResponse = Client().Detect(base64);

            // Check results

            // Handle Errors
            if (detectResponse.Errors.Any())
            {
                var msg = new StringBuilder();
                foreach (var error in detectResponse.Errors)
                    msg.AppendLine($"{error.Message} ({error.ErrCode})");
                return new { success = false, message = msg.ToString(), faceError = true };
            }

            // No faces detected - return error
            if (!detectResponse.HasFaces())
                return new { success = false, message = "No faces were detected at this image", faceError = true };

            // Faces found - check if the quality is good
            var face = detectResponse.Images.First().Faces.First();
            if (face.quality < Configurations.Config.MinimumQuality)
                return new
                {
                    success = false,
                    message =
                        $"This picture did not match the minimum quality of {Configurations.Config.MinimumQuality} - please upload another (Quality: {face.quality})",
                    faceError = true
                };

            // #### Enroll on Face API            
            var enrollResponse = Enroll(subjectId, base64);

            // Handle Errors
            if (enrollResponse.Errors.Any())
            {
                var msg = new StringBuilder();
                foreach (var error in detectResponse.Errors)
                    msg.AppendLine($"{error.Message} ({error.ErrCode})");
                return new { success = false, message = msg.ToString(), faceError = true };
            }

            // Handle non-success error
            var transaction = enrollResponse.Images.First().Transaction;
            if (transaction.status != "success")
                return new
                {
                    success = false,
                    message = "This image does not have a good quality - please upload another",
                    faceError = true
                };

            // No enough accuracy 
            var confidence = float.Parse(transaction.confidence);
            if (confidence < Configurations.Config.MinimumConfidence)
            {
                // Remove from Kairos
                Client().RemoveSubject(subjectId, Configurations.Config.KairosGalleryName);
     
                
                // Return error
                return new
                {
                    success = false,
                    message =
                        $"This picture did not match the minimum confidence of {Configurations.Config.MinimumConfidence} - please upload another (Confidence: {confidence})",
                    faceError = true
                };
            }

            return null;
        }

        public EnrollResponse Enroll(string subjectId, string imageBase64)
        {
            var enrollResponse = _kairosClient.Enroll(imageBase64, subjectId, Configurations.Config.KairosGalleryName, "FRONTAL");
            return enrollResponse;
        }
    }
}