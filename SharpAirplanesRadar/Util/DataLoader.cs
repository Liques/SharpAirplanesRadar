using System;
using System.Net.Http;
using System.Threading.Tasks;
using SharpAirplanesRadar.Util;

namespace SharpAirplanesRadar.Util
{
    internal class DataLoader : IDataLoader
    {
        public async Task<string> Load(string url)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = null;

            try
            {
                LoggingHelper.LogBehavior(">> Trying to load data from server...");

                response = await httpClient.GetAsync(url);
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
