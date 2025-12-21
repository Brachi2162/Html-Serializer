using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Selector
{
    public string TagName;
    public string Id;
    public List<string> Classes = new();

    public Selector Parent;
    public Selector Child;

    public static Selector Parse(string query)
    {
        var parts = query.Split(' ',
            System.StringSplitOptions.RemoveEmptyEntries);

        Selector root = null;
        Selector current = null;

        foreach (var part in parts)
        {
            var sel = new Selector();
            var matches = Regex.Matches(part,
                @"(#\w+)|(\.\w+)|(\w+)");

            foreach (Match m in matches)
            {
                var v = m.Value;
                if (v.StartsWith("#")) sel.Id = v[1..];
                else if (v.StartsWith(".")) sel.Classes.Add(v[1..]);
                else sel.TagName = v.ToLower();
            }

            if (root == null) root = sel;
            if (current != null)
            {
                current.Child = sel;
                sel.Parent = current;
            }

            current = sel;
        }

        return root;
    }
}
