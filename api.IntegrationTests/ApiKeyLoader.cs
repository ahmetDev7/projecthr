using System.IO;
using System.Text.Json;

public static class ApiKeyLoader
{
    public static string LoadApiKeyFromJson(string filePath, string keyName)
    {
        if (!File.Exists(filePath))
        {
            return null;
        }

        var json = File.ReadAllText(filePath);
        var jsonObject = JsonSerializer.Deserialize<JsonElement>(json);

        return jsonObject.TryGetProperty(keyName, out var apiKey) ? apiKey.GetString() : null;
    }
}
