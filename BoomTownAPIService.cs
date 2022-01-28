using System.Net.Http.Headers;
using Newtonsoft.Json;

class BoomTownAPIService
{
    private readonly HttpClient HttpClient;
	private const string TOPLEVELURL = "https://api.github.com/orgs/boomtownroi";

    // constructor for this class; initializes the httpclient with a generic useragent header in order to work with github api
    public BoomTownAPIService()
	{
		HttpClient = new HttpClient();
        HttpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Product", "1.0"));
	}

    // async method that will return a string of the input url's response body
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

    // prints the id value of any id key/value pair on the page of the input url
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

    // iterates through each url in the top level object that contains the top level url and prints the id's contained within
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

    // verifies that the creation date is earlier than the update date in the top level object
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

    // counts the number of repos contained within the repo page's array
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

    // verifies whether or not the number of repos on the repo page matches the value stored in public_repos
    public bool areRepoCountsEqual(string jsonString){
        Dictionary<string,string> json = new Dictionary<string, string>();
        json = JsonConvert.DeserializeObject<Dictionary<string,string>>(jsonString);

        int public_repos = int.Parse(json["public_repos"]);
        int count = countRepos(jsonString);

        Console.WriteLine("public_repos contains the value {0} and the repos array contains {1} repos.",public_repos,count);

        return int.Parse(json["public_repos"]) == countRepos(jsonString);
    }

    // static main method; acts as a driver
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
