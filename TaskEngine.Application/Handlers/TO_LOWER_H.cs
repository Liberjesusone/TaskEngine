using System.Text.Json;
using TaskEngine.Application.Interfaces;

namespace TaskEngine.Application.Handlers;


// This handler process a payload that contains a string and returns the uppercase version of it.
// e.g: "Type": "TO_LOWER_H",
//      "Payload": "{\"Text\": \"Hello Everyone there, this is a normal text\"}",
public class TO_LOWER_H : IHandler
{
    // The class to hold the deserialized data
    private class TextData { public string Text { get; set; } }

    public object? Deserialize(string payload)
    {
        // We use the default options to ignore case sensitivity
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        return JsonSerializer.Deserialize<TextData>(payload, options);
    }

    public string GetPayloadFromUser()
    {
        Console.WriteLine("Introduce the text (example: hello there):");

        string input = Console.ReadLine() ?? "";

        // The handler creates the object and serializes it 
        return JsonSerializer.Serialize(new { Text = input });
    }

    /// <summary>
    /// Transforms the input string from the payload to lowercase.
    /// </summary>
    /// <param name="payload">String with the text to lowercase</param>
    /// <returns>The lowercased text</returns>
    public async Task<object?> HandleAsync(string payload)
    {
        try
        {
            var data = (TextData?)Deserialize(payload);

            if (data == null || string.IsNullOrEmpty(data.Text))
            {
                Console.WriteLine("Invalid payload in TO_LOWER_H " +
                                  "\nPayload: " + payload +
                                  "\nExpected format: \"{\\\"Text\\\": \\\"Hello Everyone there, this is a normal text\\\"}\"");
                return null;
            }

            await Task.Delay(15000); // OJO Just for testing parallelism Simulate a delay of 15 seconds

            string result = data.Text.ToLower();
            return new { LowerText = result };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Error in TO_LOWER_H] Critical failure processing: {ex.Message}" +
                               "\nPayload: " + payload +
                               "\nExpected format: \"{\\\"Text\\\": \\\"Hello Everyone there, this is a normal text\\\"}\"");
            return null;
        }
    }
}
