using System;
using System.Net.Http.Headers;
using Newtonsoft.Json;

class BoomTownAPIService
{
    private readonly HttpClient HttpClient;
	private const string TOPLEVELURL = "https://api.github.com/orgs/boomtownroi";

    public BoomTownAPIService()
	{
		HttpClient = new HttpClient();
        HttpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Product", "1.0"));
	}

	public async Task<string> GetBoomTownObjectString(string url)
	{
		using (var request = new HttpRequestMessage(HttpMethod.Get, url))
		{
			var response = await HttpClient.SendAsync(request);

            int status = (int)response.StatusCode;
            if (status != 200)
            {
                Console.WriteLine("Something went wrong; got response code " + status);
                return string.Empty;
            }

            string responseString = await response.Content.ReadAsStringAsync();

			return responseString != null ? responseString : string.Empty;
		}
	}

    public void printIDs(string url)
    {
        string objString = GetBoomTownObjectString(url).Result;

        if (objString == string.Empty)
        {
            Console.WriteLine("Something went wrong getting IDs from URL " + url + "\n");
            return;
        }

        dynamic obj = JsonConvert.DeserializeObject(objString);

        foreach (var group in obj)
        {
            //Console.WriteLine((string)group.id);
            Console.WriteLine("iterates correctly");
        }
    }

    public void CrawlURLs(string jsonString, string url)
    {
        Dictionary<string,string> json = new Dictionary<string, string>();
        json = JsonConvert.DeserializeObject<Dictionary<string,string>>(jsonString);

        foreach (var element in json)
        {
            if (element.Value != null && element.Value.Contains(TOPLEVELURL, comparisonType: StringComparison.OrdinalIgnoreCase))
            {
                printIDs(element.Value);
            }
        }
    }
    
    static void Main(string[] args)
    {
        BoomTownAPIService boomTown = new BoomTownAPIService();

        string topLevelString = boomTown.GetBoomTownObjectString(TOPLEVELURL).Result;
        //Console.WriteLine("{0}",topLevelString);

        boomTown.CrawlURLs(topLevelString, TOPLEVELURL);

    }
}

