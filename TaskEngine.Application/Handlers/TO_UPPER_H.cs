using System.Text.Json;
using TaskEngine.Application.Interfaces;

namespace TaskEngine.Application.Handlers;


// This handler process a payload that contains a string and returns the uppercase version of it.
// e.g: "Type": "TO_UPPER_H",
//      "Payload": "{\"Text\": \"Hello Everyone there this is a normal text\"}",
public class TO_UPPER_H : IHandler
{
    // The class to hold the deserialized data
    private class TextData { public string Text { get; set; } }

    public object? Deserialize(string payload)
    {
        // We use the default options to ignore case sensitivity
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        return JsonSerializer.Deserialize<TextData>(payload, options);

    }

    /// <summary>
    /// Transforms the input string from the payload to uppercase.
    /// </summary>
    /// <param name="payload">String with the text to uppercase</param>
    /// <returns>The uppercased text</returns>
    public Task<object?> HandleAsync(string payload)
    {
        var data = (TextData?)Deserialize(payload);

        if (data == null || string.IsNullOrEmpty(data.Text))
        {
            Console.WriteLine("Invalid payload in TO_UPPER_H " +
                              "\nPayload: " + payload + 
                              "\nExpected format: Any Kind Of Text\n\n");
            return null;
        }

        string result = data.Text.ToUpper();
        return Task.FromResult<object?>(new { UpperText = result });
    }
}
