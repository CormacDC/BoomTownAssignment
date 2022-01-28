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

	public async Task<object> GetBoomTownObject(string url)
	{
		using (var request = new HttpRequestMessage(HttpMethod.Get, url))
		{
			var response = await HttpClient.SendAsync(request);

			response.EnsureSuccessStatusCode();

            string responseString = await response.Content.ReadAsStringAsync();

            //dynamic returnObject = JsonSerializer.Deserialize<BoomTown>(responseString);
            object returnObject = JsonConvert.DeserializeObject(responseString);

			return returnObject != null ? returnObject : string.Empty;
		}
	}
    
    static void Main(string[] args)
    {
        BoomTownAPIService boomTown = new BoomTownAPIService();
        object topLevel = boomTown.GetBoomTownObject(TOPLEVELURL).Result;

        //string topLevelString = JsonSerializer.Serialize<BoomTown>(topLevel);
        string topLevelString = JsonConvert.SerializeObject(topLevel);
        Console.WriteLine("{0}",topLevelString);

        
    }
}

