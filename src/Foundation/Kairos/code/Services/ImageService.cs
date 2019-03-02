using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace FaceLogin.Foundation.Kairos.Services
{
    public class ImageService : IImageService
    {
        public string GetBase64FromImage(Image image)
        {
            using (var m = new MemoryStream())
            {
                image.Save(m, ImageFormat.Jpeg);
                var imageBytes = m.ToArray();

                // Convert byte[] to Base64 String
                var base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        public byte[] ImageToByteArray(Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }

        public byte[] Base64ToByteArray(string base64)
        {
            var imageBytes = Convert.FromBase64String(base64);
            using (var ms1 = new MemoryStream(imageBytes))
            {
                var img = Image.FromStream(ms1);
                return ImageToByteArray(img);
            }
        }
    }
}