namespace FaceLogin.Foundation.Kairos.Services
{
    public interface IImageService
    {
        string GetBase64FromImage(System.Drawing.Image image);
        byte[] ImageToByteArray(System.Drawing.Image imageIn);
        byte[] Base64ToByteArray(string base64);
    }
}
