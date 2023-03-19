using System;
using System.Reflection;
using System.Net.Http;
using System.IO;

namespace SharpAirplanesRadar.Util
{
    internal static class ResourceHelper
    {
        private const string resourceFolderUrl = "https://raw.githubusercontent.com/Liques/SharpAirplanesRadar/master/Resources/";
        private const string resourceFoldelLocal = "";
        public static string LoadExternalResource(string fileName)
        {
            var maxFileExpiration = new TimeSpan(15, 0, 0);

            string splitter = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows) ? "\\" : "/";

            var fileLocation = fileName;
            string fileContent = String.Empty;

            if (File.Exists(fileLocation) && System.IO.File.GetLastWriteTime(fileName).Add(maxFileExpiration) >= DateTime.Now)
            {
                var file = File.OpenText(fileLocation);
                fileContent = file.ReadToEnd();
                file.Close();
            }
            else
            {
                try
                {
                    LoggingHelper.LogBehavior($">> Trying to download resource '{fileName}' from server...");
                    HttpClient httpClient = new HttpClient();
                    HttpResponseMessage response = null;

                    response = httpClient.GetAsync(resourceFolderUrl + fileName).Result;
                    fileContent = response.Content.ReadAsStringAsync().Result;

                    File.WriteAllText(fileLocation, fileContent);

                    LoggingHelper.LogBehavior($">> File '{fileName}' dowloaded and saved from server.");
                }
                catch (Exception e)
                {
                    throw new ArgumentException("Error trying to download resource from server.", e);
                }

            }

            LoggingHelper.LogBehavior($">> Starting to converting file '{fileName}'...");

            return fileContent;
        }
    }
}