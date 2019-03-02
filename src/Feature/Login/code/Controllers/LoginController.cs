using System;
using System.Web.Mvc;
using FaceLogin.Foundation.XConnect.Services;

namespace FaceLogin.Feature.Login.Controllers
{
    public class LoginController : Controller
    {
        private readonly IXConnectService _xConnectService = DependencyResolver.Current.GetService<IXConnectService>();

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FaceLoginExecute(string base64)
        {
            try
            {
                return Json(_xConnectService.FaceLogin(Constants.Identifiers.FaceIdentifier, base64));
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(
                    $"Error on [FaceLoginExecute]: Message: {ex.Message}; Source:{ex.Source}", this);
                return Json(new { success = false, message = "Error face authenticating user" });
            }
        }
    }
}