using System.Net.Http;
using System.Threading.Tasks;

public class HtmlLoader
{
    public async Task<string> Load(string url)
    {
        var client = new HttpClient();
        return await client.GetStringAsync(url);
    }
}
