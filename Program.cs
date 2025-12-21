using System;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        var loader = new HtmlLoader();
        var html = await loader.Load("https://example.com/");

        var parser = new HtmlParser();
        var tokens = parser.Split(html);
        var root = parser.Parse(tokens);

        Console.WriteLine("===== HTML TREE =====");
        root.PrintTree();

        var results = root.QuerySelector("div");
        Console.WriteLine($"\nFound {results.Count} elements");
    }
}
