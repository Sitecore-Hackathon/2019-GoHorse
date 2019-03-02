using System;
using System.IO;
using System.Text;
using FaceLogin.Foundation.XConnect.Models;

namespace FaceLogin.Foundation.XConnect.sitecore_modules.FaceLogin
{
    public partial class ModelBuilder : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var model = Sitecore.XConnect.Serialization.XdbModelWriter.Serialize(FaceLoginDefineModel.Model);
                var fileName = FaceLoginDefineModel.Model.FullName + ".json";
                var basePath = Server.MapPath("~");
                var filePath = $"{basePath}sitecore modules\\FaceLogin\\{fileName}";
                File.WriteAllText(filePath, model);
                divMessage.InnerHtml = $"File saved - '{filePath}'";
            }
            catch (Exception exception)
            {
                var sb = new StringBuilder();
                sb.AppendLine($"<p>EXCEPTION: </p>");
                sb.AppendLine($"<pre>{exception}</pre>");
                divMessage.InnerHtml = sb.ToString();
            }
            finally
            {
                //File.Delete(imagePath);
            }
        }
    }
}