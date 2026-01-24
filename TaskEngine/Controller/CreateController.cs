using System.Runtime.CompilerServices;
using TaskEngine.Application.Interfaces;
using TaskEngine.Application.Services;
using TaskEngine.Domain;

namespace TaskEngine.Controller;

public class CreateController : Controller
{
    ETask task = new ETask();
    IETaskRepository _eTasksRepository;
    ETaskEngine _engine;

    public CreateController(IETaskRepository eTaskRepository, ETaskEngine _engine)
    {
        this._eTasksRepository = eTaskRepository;
        this._engine = _engine;
    }


    // Shows the lists of types available and asks the user to select one, returns the selected type
    private ETaskType ShowTypes()
    {
        while (true)
        {
            Clear().GetAwaiter();

            Console.WriteLine("Select a Task Type: ");
            foreach (var type in Enum.GetValues<ETaskType>())
                Console.WriteLine($"{(int)type} - {type}");

            Console.WriteLine("\n\nSelect Type: ");
            int typeOption = AskANumber();

            if (Enum.IsDefined(typeof(ETaskType), typeOption))
                return (ETaskType)typeOption;
            else
                Console.WriteLine("Invalid Type selected.");
        }
    }

    private void ShowEditor()
    {
        while (true)
        {
            Clear().GetAwaiter();
            Console.WriteLine("------- Task Editor ------- ");
            Console.WriteLine("1- Edit - Type: " + task.Type ?? "");
            Console.WriteLine("2- Edit - Payload: " + task.Payload);
            Console.WriteLine("Status: PENDING");
            Console.WriteLine("Created at: " + task.CreatedAt.ToString());
            Console.WriteLine("\n0- Back");

            int option = AskANumber();
            switch (option)
            {
                case 1:
                    task.Type = ShowTypes();
                    break;
                case 2:
                    Separator();
                    Console.WriteLine("Enter Payload: ");
                    var handler = _engine.GetHandler(task.Type);
                    string? payload = handler.GetPayloadFromUser();
                    task.Payload = payload ?? "";
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        }
    }

    public override async Task Show()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("------- Tasks Creator -------\n" +
                              "\nTask: " +
                              "\nType: " + (task.Type.ToString() ?? "") +
                              "\nPayload: " + task.Payload +
                              "\nStatus: PENDING" +
                              "\nCreated at: " + task.CreatedAt.ToString() + 
                              "\n\n\n\n\n" +
                              "\n1- Add task " +
                              "\n2- Edit task " +
                              "\n0- Back\n\n\n");

            Console.WriteLine("\n\n");
            Separator();
            int option = AskANumber();

            switch (option)
            {
                case 1:
                    Console.WriteLine("Confirm adding task? (y/n)");
                    var confirm = Console.ReadLine();
                    if (confirm == "y" || confirm == "Y")
                    {
                        task = await _eTasksRepository.AddAsync(task);
                        var wasEnQueued = _engine.EnQueue(task);
                        if (!wasEnQueued)
                            Console.WriteLine("Failed to enqueue task.");

                        if (task != null)
                        {
                            Console.WriteLine("Task added successfully!");
                            task = new ETask();
                        }
                    }
                    else
                        Console.WriteLine("Task creation cancelled.");

                    Console.ReadLine();
                    break;
                case 2:
                    ShowEditor();
                    break; 
                case 0:
                    Console.WriteLine("Coming Back");
                    return;
                default:
                    Console.WriteLine("Invalid option");
                    Console.ReadLine();
                    break;
            }
        }
    }
}
