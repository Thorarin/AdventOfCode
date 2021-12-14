using System.Text.Json.Nodes;

namespace Thorarin.AdventOfCode.Framework;

public class SecretStore
{
    private readonly Dictionary<string, string> _secrets;

    private SecretStore(Dictionary<string, string> secrets)
    {
        _secrets = secrets;
    }
    
    public static SecretStore Load(string fileName)
    {
        var content = (IDictionary<string, JsonNode?>)JsonNode.Parse(File.ReadAllText("secrets.json"))!;
        return new SecretStore(content.ToDictionary(kvp => kvp.Key, kvp => kvp.Value!.ToString()));
    }
    
    public string GetSecret(string key)
    {
        return _secrets[key];
    }
}