using System;
using System.Reflection;
using System.Net.Http;
using System.IO;

namespace NiceAirplanesRadar.Util
{
    internal static class ResourceHelper
    {
        private const string resourceFolderUrl = "https://raw.githubusercontent.com/Liques/NiceAirplanesRadar/master/Resources/";
        private const string resourceFoldelLocal = "";
        public static string LoadExternalResource(string fileName)
        {
            string splitter = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows) ? "\\" : "/";
             
            var fileLocation = fileName;
            string fileContent = String.Empty;

            if (!File.Exists(fileLocation)  && System.IO.File.GetLastWriteTime(fileName).AddDays(15) < DateTime.Now)
            {
                try
                {
                    LoggingHelper.LogBehavior($">> Trying to download resource '{fileName}' from server...");
                    HttpClient httpClient = new HttpClient();
                    HttpResponseMessage response = null;

                    response = httpClient.GetAsync(resourceFolderUrl + fileName).Result;

                    //var newfile = File.Create(fileLocation);
                 
                    File.WriteAllText(fileLocation,response.Content.ReadAsStringAsync().Result);
                    //newfile.Close();
                    LoggingHelper.LogBehavior($">> File '{fileName}' dowloaded and saved from server.");
                    

                }
                catch (Exception e)
                {
                    throw new ArgumentException("Error trying to download resource from server.", e);
                }
            }

            var file = File.OpenText(fileLocation);
            fileContent = file.ReadToEnd();
            file.Close();

            LoggingHelper.LogBehavior($">> Starting to converting file '{fileName}'...");

            return fileContent;
        }
    }
}