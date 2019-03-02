namespace FaceLogin.Foundation.Kairos
{
    public static class Configurations
    {
        public struct ConfigKeys
        {
            public const string KairosGalleryName = "FaceLogin.Kairos_GalleryName";
            public const string KairosApplicationId = "FaceLogin.Kairos_ApplicationID";
            public const string KairosApplicationKey = "FaceLogin.Kairos_ApplicationKey";
            public const string MinimumConfidence = "FaceLogin.MinimumConfidence";
            public const string MinimumQuality = "FaceLogin.MinimumQuality";
            public const string XConnectFaceIdentifierName = "FaceLogin.XConnect.FaceIdentifierName";
        }

        public struct Config
        {
            public static string XConnectFaceIdentifierName =>
                Sitecore.Configuration.Settings.GetSetting(ConfigKeys.XConnectFaceIdentifierName, "FaceLogin");
            public static string KairosGalleryName =>
                Sitecore.Configuration.Settings.GetSetting(ConfigKeys.KairosGalleryName, "FaceLogin");
            public static string ApplicationId =>
                Sitecore.Configuration.Settings.GetSetting(ConfigKeys.KairosApplicationId, "");
            public static string ApplicationKey =>
                Sitecore.Configuration.Settings.GetSetting(ConfigKeys.KairosApplicationKey,
                    "");
            public static double MinimumConfidence =>
                Sitecore.Configuration.Settings.GetDoubleSetting(ConfigKeys.MinimumConfidence, 0.60);
            public static double MinimumQuality =>
                Sitecore.Configuration.Settings.GetDoubleSetting(ConfigKeys.MinimumQuality, 0.60);
        }
    }
}