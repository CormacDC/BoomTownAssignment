public class BoomTown
{
    public string login { get; set; } = string.Empty;
    public int id { get; set; }
    public string node_id { get; set; } = string.Empty;
    public string url { get; set; } = string.Empty;
    public string repos_url { get; set; } = string.Empty;
    public string events_url { get; set; } = string.Empty;
    public string hooks_url { get; set; } = string.Empty;
    public string issues_url { get; set; } = string.Empty;
    public string members_url { get; set; } = string.Empty;
    public string public_members_url { get; set; } = string.Empty;
    public string avatar_url { get; set; } = string.Empty;
    public string description { get; set; } = string.Empty;
    public string name { get; set; } = string.Empty;
    public object company { get; set; } = string.Empty;
    public string blog { get; set; } = string.Empty;
    public object location { get; set; } = string.Empty;
    public object email { get; set; } = string.Empty;
    public object twitter_username { get; set; } = string.Empty;
    public bool is_verified { get; set; }
    public bool has_organization_projects { get; set; }
    public bool has_repository_projects { get; set; }
    public int public_repos { get; set; }
    public int public_gists { get; set; }
    public int followers { get; set; }
    public int following { get; set; }
    public string html_url { get; set; } = string.Empty;
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public string type { get; set; } = string.Empty;
}