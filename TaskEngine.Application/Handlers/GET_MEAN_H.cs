using System.Runtime.InteropServices.ObjectiveC;
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

    public string GetPayloadFromUser()
    {
        Console.WriteLine("Introduce the numbers separated by \" , \" (example: 10, 20.5, 30):");

        string input = Console.ReadLine() ?? "";

        // We convert the input in a list of doubles
        var numbers = input.Split(',')
                           .Select(n => double.TryParse(n.Trim(), out double val) ? val : 0)
                           .ToList();

        // The handler creates the object and serializes it 
        return JsonSerializer.Serialize(new { Numbers = numbers });
    }

    /// <summary>
    /// Calculates the mean of a list of numbers provided in the payload.
    /// </summary>
    /// <param name="payload">A string containing a JSON list of numbers</param>
    /// <returns>An object containing the calculated average</returns>
    /// <exception cref="Exception"></exception>
    public async Task<object?> HandleAsync(string payload)
    {
        try
        {
            var data = (MeanData?)Deserialize(payload);

            if (data == null || data.Numbers == null)
            {
                Console.WriteLine("Invalid payload in GET_MEDIAN_H " +
                                  "\nPayload: " + payload +
                                  "\nExpected format: \"{\\\"Numbers\\\": [10, 20, 30, 40, 50]}\"");
                return null;
            }

            await Task.Delay(5000); // TODO: Just for testing parallelism Simulate a delay of 5 seconds

            double result = data.Numbers.Average();

            return new { Mean = result };
        }
        catch (Exception ex)
        {
            Console.WriteLine("[Error in GET_MEAN_H] Critical failure processing:" + ex.Message +
                              "\nPayload: " + payload +
                              "\nExpected format: \"{\\\"Numbers\\\": [10, 20, 30, 40, 50]}\"");
            return null;
        }
    }
}
