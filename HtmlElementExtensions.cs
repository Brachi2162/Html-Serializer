using System.Collections.Generic;
using System.Linq;

public static class HtmlElementExtensions
{
    public static HashSet<HtmlElement> QuerySelector(
        this HtmlElement root, string query)
    {
        var selector = Selector.Parse(query);
        var result = new HashSet<HtmlElement>();
        Find(root, selector, result);
        return result;
    }

    private static void Find(
        HtmlElement element,
        Selector selector,
        HashSet<HtmlElement> result)
    {
        var matches = element.Descendants()
            .Where(e => Match(e, selector));

        if (selector.Child == null)
        {
            foreach (var m in matches)
                result.Add(m);
        }
        else
        {
            foreach (var m in matches)
                Find(m, selector.Child, result);
        }
    }

    private static bool Match(HtmlElement e, Selector s)
    {
        return
            (s.TagName == null || e.Name == s.TagName) &&
            (s.Id == null || e.Id == s.Id) &&
            s.Classes.All(c => e.Classes.Contains(c));
    }

    public static void PrintTree(this HtmlElement el, int depth = 0)
    {
        var indent = new string(' ', depth * 2);
        System.Console.WriteLine($"{indent}<{el.Name}>");

        foreach (var c in el.Children)
            c.PrintTree(depth + 1);
    }
}
