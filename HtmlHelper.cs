using System;
using System.IO;
using System.Text.Json;

public class HtmlHelper
{
    private static readonly HtmlHelper _instance = new();
    public static HtmlHelper Instance => _instance;

    public string[] AllTags { get; }
    public string[] VoidTags { get; }

    private HtmlHelper()
    {
        AllTags = JsonSerializer.Deserialize<string[]>(
            File.ReadAllText("HtmlTags.json")) ?? Array.Empty<string>();

        VoidTags = JsonSerializer.Deserialize<string[]>(
            File.ReadAllText("HtmlVoidTags.json")) ?? Array.Empty<string>();
    }

    public bool IsValidTag(string tag) =>
        Array.Exists(AllTags, t => t == tag);

    public bool IsVoidTag(string tag) =>
        Array.Exists(VoidTags, t => t == tag);
}
