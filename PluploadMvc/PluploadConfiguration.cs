namespace XperiCode.PluploadMvc
{
    public static class PluploadConfiguration
    {
        static PluploadConfiguration()
        {
            UploadPath = "~/aApp_Data/PluploadMvc";
            HandlerPath = "~/Plupload.axd?reference={0}";
        }

        // TODO: Validation in setters

        public static string UploadPath { get; set; }
        public static string HandlerPath { get; set; }
    }
}
