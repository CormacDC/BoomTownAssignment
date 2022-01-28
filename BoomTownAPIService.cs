﻿using System.Net.Http.Headers;
using System.Text.Json;

class BoomTownAPIService
{
    private readonly HttpClient HttpClient;
	private const string URL = "https://api.github.com/orgs/boomtownroi";
    private const string User = "CormacDC";

    public BoomTownAPIService()
	{
		HttpClient = new HttpClient();
        HttpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(User, "1"));
	}
	public async Task<BoomTown> GetBoomTownObject()
	{
		using (var request = new HttpRequestMessage(HttpMethod.Get, URL))
		{
			var response = await HttpClient.SendAsync(request);

			response.EnsureSuccessStatusCode();

            string responseString = await response.Content.ReadAsStringAsync();

            BoomTown? returnObject = JsonSerializer.Deserialize<BoomTown>(responseString);

			return returnObject != null ? returnObject : new BoomTown();
		}
	}
    
    static void Main(string[] args)
    {
        BoomTownAPIService boomTown = new BoomTownAPIService();
        BoomTown topLevel = boomTown.GetBoomTownObject().Result;
        string topLevelString = JsonSerializer.Serialize<BoomTown>(topLevel);

        Console.WriteLine("{0}",topLevelString);
    }
}
