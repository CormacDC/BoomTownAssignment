using System.Net.Http.Headers;
using Newtonsoft.Json;

class BoomTownAPIService
{
    private readonly HttpClient HttpClient;
	private const string URL = "https://api.github.com/orgs/boomtownroi";
    private const string User = "CormacDC";

    public BoomTownAPIService()
	{
		HttpClient = new HttpClient();
        HttpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(User, "1.0"));
	}

	public async Task<object> GetBoomTownObject()
	{
		using (var request = new HttpRequestMessage(HttpMethod.Get, URL))
		{
			var response = await HttpClient.SendAsync(request);

			response.EnsureSuccessStatusCode();

            string responseString = await response.Content.ReadAsStringAsync();

            //dynamic returnObject = JsonSerializer.Deserialize<BoomTown>(responseString);
            dynamic returnObject = JsonConvert.DeserializeObject(responseString);

			return returnObject != null ? returnObject : string.Empty;
		}
	}
    
    static void Main(string[] args)
    {
        BoomTownAPIService boomTown = new BoomTownAPIService();
        dynamic topLevel = boomTown.GetBoomTownObject().Result;

        //string topLevelString = JsonSerializer.Serialize<BoomTown>(topLevel);
        string topLevelString = JsonConvert.SerializeObject(topLevel);

        Console.WriteLine("{0}",topLevelString);
    }
}

