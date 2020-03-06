using System;
using System.Net.Http;
using System.Threading.Tasks;
using NiceAirplanesRadar.Util;

namespace NiceAirplanesRadar.Util
{
    internal class DataLoader : IDataLoader
    {
        private string url;
        public DataLoader(string url)
        {
            this.url = url;
        }

        public async Task<string> Load(string customUrl = null)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = null;

            try
            {
                LoggingHelper.LogBehavior(">> Trying to load data from server...");

                var apiUrl = String.IsNullOrEmpty(customUrl) ? this.url : customUrl;
                response = await httpClient.GetAsync(apiUrl);
                LoggingHelper.LogBehavior(">> Done load data from server.");
            }
            catch (Exception e)
            {
                throw new ArgumentException("Server is out.", e);
            }

            return response.Content.ReadAsStringAsync().Result;
        }
    }
}
