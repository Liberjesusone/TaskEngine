using System.Text.Json;
using TaskEngine.Application.Interfaces;

namespace TaskEngine.Application.Handlers;

// This handler process a payload that contains a list of numbers and returns the mean value.
// e.g: "Type": "GET_MEAN_H",
//      "Payload": "{\"Numbers\": [10, 20, 30, 40, 50]}",
public class GET_MEAN_H : IHandler
{
    // the class to hold the deserialized data
    private class MeanData { public List<double> Numbers { get; set; } }

    public object? Deserialize(string payload)
    {
        // We use the default options to ignore case sensitivity
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        return JsonSerializer.Deserialize<MeanData>(payload, options);
    }

    /// <summary>
    /// Calculates the mean of a list of numbers provided in the payload.
    /// </summary>
    /// <param name="payload">A string containing a JSON list of numbers</param>
    /// <returns>An object containing the calculated average</returns>
    /// <exception cref="Exception"></exception>
    public async Task<object?> HandleAsync(string payload)
    {
        var data = (MeanData?)Deserialize(payload);

        if (data == null || data.Numbers == null)
        {
            Console.WriteLine("Invalid payload in GET_MEDIAN_H " +
                              "\nPayload: " + payload +
                              "\nExpected format: \"{\\\"Numbers\\\": [10, 20, 30, 40, 50]}\"\n\n");
            return null; 
        }

        double result = data.Numbers.Average();

        return await Task.FromResult<object?>(new { Mean = result });
    }
}
