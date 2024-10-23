using System.Text.Json;

public class JsonProvider
{

    readonly string _jsonDataPath;

    public JsonProvider()
    {
        _jsonDataPath = "data";
    }

    public List<T>? Decode<T>(string targetFile)
    {
        try
        {
            var jsonString = File.ReadAllText($"{_jsonDataPath}/{targetFile}");
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                AllowTrailingCommas = true
            };

            return JsonSerializer.Deserialize<List<T>>(jsonString, options);
        }
        catch (Exception e)
        {
            // TODO: Add logging
            return null;
        }
    }
}