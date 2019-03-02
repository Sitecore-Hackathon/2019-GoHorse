using Kairos.Net;

namespace FaceLogin.Foundation.Kairos.Services
{
    public interface IKairosService
    {
        KairosClient Client();
        string EnrollPictureString(string base64, string subjectId);
        object EnrollPicture(string base64, string subjectId);
        EnrollResponse Enroll(string subjectId, string imageBase64);        
    }
}
