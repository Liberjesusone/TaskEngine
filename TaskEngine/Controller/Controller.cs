namespace TaskEngine.Controller;

public class Controller
{
    // Prints a line: ----------------------
    public virtual void Separator()
    {
        Console.WriteLine("---------------------------------");
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

    public virtual void show() { }


}
