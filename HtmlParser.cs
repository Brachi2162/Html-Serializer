using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class HtmlParser
{
    private const string HtmlRegex = "<[^>]+>|[^<]+";

    public List<string> Split(string html)
    {
        return Regex.Matches(html, HtmlRegex)
            .Select(m => m.Value.Trim())
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToList();
    }

    public HtmlElement Parse(List<string> tokens)
    {
        var root = new HtmlElement { Name = "root" };
        var current = root;

        foreach (var token in tokens)
        {
            if (token.StartsWith("</"))
            {
                current = current.Parent ?? root;
                continue;
            }

            if (token.StartsWith("<"))
            {
                var content = token.Trim('<', '>');
                var parts = content.Split(' ', 2);
                var tagName = parts[0].Trim('/').ToLower();

                if (!HtmlHelper.Instance.IsValidTag(tagName))
                    continue;

                var element = new HtmlElement
                {
                    Name = tagName,
                    Parent = current
                };

                if (parts.Length > 1)
                    ParseAttributes(element, parts[1]);

                current.Children.Add(element);

                bool selfClosing =
                    token.EndsWith("/>") ||
                    HtmlHelper.Instance.IsVoidTag(tagName);

                if (!selfClosing)
                    current = element;

                continue;
            }

            current.InnerHtml += token;
        }

        return root;
    }

    private void ParseAttributes(HtmlElement el, string attr)
    {
        var matches = Regex.Matches(attr,
            @"([\w-]+)(?:=""([^""]*)"")?");

        foreach (Match m in matches)
        {
            var key = m.Groups[1].Value;
            var val = m.Groups[2].Value;

            if (key == "id") el.Id = val;
            else if (key == "class")
                el.Classes.AddRange(val.Split(' ',
                    System.StringSplitOptions.RemoveEmptyEntries));
            else
                el.Attributes.Add($"{key}=\"{val}\"");
        }
    }
}
