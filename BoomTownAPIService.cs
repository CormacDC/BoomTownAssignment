using System.Net.Http.Headers;

class BoomTownAPIService
{
    private readonly HttpClient HttpClient;
	private const string URL = "https://api.github.com/orgs/boomtownroi";
    private const string User = "CormacDC";

    public BoomTownAPIService()
	{
		HttpClient = new HttpClient();
        HttpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("CormacDC", "1"));
	}
	public async Task<string> GetData()
	{
		using (var request = new HttpRequestMessage(HttpMethod.Get, URL))
		{
			var response = await HttpClient.SendAsync(request);

			response.EnsureSuccessStatusCode();

			return await response.Content.ReadAsStringAsync();
		}
	}
    
    static void Main(string[] args)
    {
        BoomTownAPIService boomTown = new BoomTownAPIService();
        Console.WriteLine("{0}",boomTown.GetData().Result);
    }
}

