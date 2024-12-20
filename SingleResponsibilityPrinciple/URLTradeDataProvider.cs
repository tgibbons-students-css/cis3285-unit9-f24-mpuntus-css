using SingleResponsibilityPrinciple.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SingleResponsibilityPrinciple
{
    public class URLTradeDataProvider : ITradeDataProvider
    {
        string url;
        ILogger logger;
        public URLTradeDataProvider(string url, ILogger logger)
        {
            this.url = url;
            this.logger = logger;
        }

        //public IEnumerable<string> GetTradeData()
        public async Task<IEnumerable<string>> GetTradeDataAsync()

        {
            List<string> tradeData = new List<string>();
            logger.LogInfo("Reading trades from URL: " + url);

            using (HttpClient client = new HttpClient())
            {
                //HttpResponseMessage response = client.GetAsync(url).Result;
                HttpResponseMessage response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    logger.LogWarning($"Failed to retrieve data. Status code: {response.StatusCode}");
                    throw new Exception($"Error retrieving data from URL: {url}");
                }

                //using (Stream stream = response.Content.ReadAsStreamAsync().Result)
                using (Stream stream = await response.Content.ReadAsStreamAsync())

                using (StreamReader reader = new StreamReader(stream))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        tradeData.Add(line);
                    }
                }
            }
            return tradeData;

        }


        public IEnumerable<string> GetTradeData()
        {
            return GetTradeDataAsync().GetAwaiter().GetResult();
        }


    }
}
