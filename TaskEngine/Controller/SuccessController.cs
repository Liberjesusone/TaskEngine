using System.Text.Json;
using TaskEngine.Application.Interfaces;
using TaskEngine.Domain;

namespace TaskEngine.Controller;

class SuccessController : Controller
{
    IETaskRepository _eTasksRepository;
    public SuccessController(IETaskRepository eTaskRepository)
    {
        this._eTasksRepository = eTaskRepository;
    }

    // TODO: we can also show in screen the payload in a better way and still store it as json 
    public void ShowList()
    {
        _eTasksRepository.ForEach(
            task => task.Status == ETaskStatus.SUCCESS && !task.IsDeleted,
            task =>
            {
                Separator();
                Console.WriteLine("Id: " + task.Id);
                Console.WriteLine("Type: " + task.Type.ToString());
                Console.WriteLine("Payload: " + task.Payload);
                Console.WriteLine("Status: " + task.Status.ToString());
                Console.WriteLine("Created at: " + task.CreatedAt.ToString());
                Console.WriteLine("Finished at: " + task.FinishedAt.ToString());

                // We show the result in a better way
                if (!string.IsNullOrEmpty(task.Result))
                {
                    try
                    {
                        // We parse the result to check if it's a JSON object
                        using (JsonDocument doc = JsonDocument.Parse(task.Result))
                        {
                            Console.Write("Result: ");
                            var root = doc.RootElement;

                            // If it's an object, we print each property
                            if (root.ValueKind == JsonValueKind.Object)
                            {
                                foreach (var property in root.EnumerateObject())
                                    Console.Write($"{property.Name}: {property.Value} ");
                                Console.WriteLine();
                            }
                            else
                                Console.WriteLine(root.ToString());
                        }
                    }
                    catch
                    {
                        Console.WriteLine($"Result: {task.Result}");
                    }
                }
                Separator();
            });
    }

    public override async Task Show()
    {
        while (true)
        {
            await Clear();
            Console.WriteLine("Successful Tasks\n\n" +
                             "0- Back\n\n\n");

            ShowList();
            Console.WriteLine("\n\n");
            Separator();
            int option = AskANumber();

            switch (option)
            {
                case 0:
                    Console.WriteLine("Coming Back");
                    return;
                default:
                    Console.WriteLine("Invalid option");
                    PressAnyKey();
                    break;
            }
        }
    }
}
