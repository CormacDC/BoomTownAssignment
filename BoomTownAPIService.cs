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
                Console.WriteLine("Something went wrong; got response code " + status + " from URL " + url + "\n");
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
            return;
        }

        dynamic obj = JsonConvert.DeserializeObject(objString);

        foreach (var group in obj)
        {
            foreach (var element in group)
            {
                if (element.Name == "id")
                {
                    Console.WriteLine("ID: " + element.Value);
                }
                else if (element.Name == "name")
                {
                    Console.WriteLine("Name: " + element.Value + "\n");
                }
            }
        }
    }

    public void CrawlURLs(string jsonString, string url)
    {
        Dictionary<string,string> json = new Dictionary<string, string>();
        json = JsonConvert.DeserializeObject<Dictionary<string,string>>(jsonString);

        foreach (var element in json)
        {
            if (element.Value != null && 
                element.Value.ToLower() != url &&
                element.Value.ToLower().Contains(TOPLEVELURL))
            {
                printIDs(element.Value);
            }
        }
    }

    public Boolean verifyUpdateCreateDates(string jsonString)
    {
        Dictionary<string,string> json = new Dictionary<string, string>();
        json = JsonConvert.DeserializeObject<Dictionary<string,string>>(jsonString);

        DateTime created = Convert.ToDateTime(json["created_at"]);
        DateTime updated = Convert.ToDateTime(json["updated_at"]);

        Console.WriteLine("created_at: " + created);
        Console.WriteLine("updated_at: " + updated);

        return DateTime.Compare(created,updated) < 0;
    }

    public int countRepos(string jsonString)
    {
        Dictionary<string,string> json = new Dictionary<string, string>();
        json = JsonConvert.DeserializeObject<Dictionary<string,string>>(jsonString);

        string objString = GetBoomTownObjectString(json["repos_url"]).Result;

        if (objString == string.Empty)
        {
            return -1;
        }

        dynamic obj = JsonConvert.DeserializeObject(objString);

        int count = 0;
        foreach (var element in obj)
        {
            count++;
        }

        return count;
    }

    public bool areRepoCountsEqual(string jsonString){
        Dictionary<string,string> json = new Dictionary<string, string>();
        json = JsonConvert.DeserializeObject<Dictionary<string,string>>(jsonString);

        int public_repos = int.Parse(json["public_repos"]);
        int count = countRepos(jsonString);

        Console.WriteLine("public_repos contains the value {0} and the repos array contains {1} repos.",public_repos,count);

        return int.Parse(json["public_repos"]) == countRepos(jsonString);
    }
    
    static void Main(string[] args)
    {
        BoomTownAPIService boomTown = new BoomTownAPIService();

        Console.WriteLine("=========================================================");
        Console.WriteLine("Displaying all IDs contained within the top level object's api urls as well as their associated names, if they exist: ");

        string topLevelString = boomTown.GetBoomTownObjectString(TOPLEVELURL).Result;
        
        boomTown.CrawlURLs(topLevelString, TOPLEVELURL);

        Console.WriteLine("=========================================================");
        Console.WriteLine("The updated_at value in the top level object contains a later date than the created_at value: " + boomTown.verifyUpdateCreateDates(topLevelString));
        Console.WriteLine("=========================================================");

        Console.WriteLine("The public_repos value matches the number of elements in the repos array in the repos url: " + boomTown.areRepoCountsEqual(topLevelString));
        Console.WriteLine("=========================================================");
    }
}
