using System.Data;

namespace TaskEngine.Controller;

public class Controller
{
    // Prints a line: ----------------------
    public void Separator()
    {
        Console.WriteLine("---------------------------------");
    }

    // Waits for any key to be pressed
    public void PressAnyKey()
    {
        Console.Write("\nPress Enter to continue...");
        Console.ReadKey(true);
    }

    // Clears the console and waits a bit to avoid flickering 
    public async Task Clear()
    {
        // Cleans the input buffer to avoid old "Enters" triggering actions
        while (Console.KeyAvailable) Console.ReadKey(true);
        Console.Clear();
        Console.WriteLine("\x1b[3J");
        Console.SetCursorPosition(0, 0);
        await Task.Delay(200);               // Delay to avoid flickering
    }

    // Ask for a number in the Console managing errors
    public int AskANumber()
    {
        Console.WriteLine("\nSelect an option: ");
        string? response = Console.ReadLine();
        int option = -1;
        try
        {
            if (!string.IsNullOrWhiteSpace(response))
            {
                string trimmed = response.Trim();
                if (!int.TryParse(trimmed, out option))
                {
                    Console.WriteLine("No valid option try again");
                    option = -1;
                }
            }
            else
                option = -1;
        }
        catch (Exception)
        {
            option = -1;
        }
        return option;
    }

    public virtual async Task Show() { }


}
