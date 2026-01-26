using TaskEngine.Application.Interfaces;
using TaskEngine.Domain;
using TaskEngine.Infrastructure.Repositories;

namespace TaskEngine.Controller;

public class RunningController : Controller
{
    IETaskRepository _eTasksRepository;
    public RunningController(IETaskRepository eTaskRepository)
    {
        this._eTasksRepository = eTaskRepository;                                                    
    }

    // TODO: here we can add the time expected with some logic after doing it , now its hardcoded
    public void ShowList()
    {
        _eTasksRepository.ForEach(
            task => task.Status == ETaskStatus.RUNNING && !task.IsDeleted,
            task =>
            {
                Separator();
                Console.WriteLine("Id: " + task.Id);
                Console.WriteLine("Type: " + task.Type.ToString());
                Console.WriteLine("Payload: " + task.Payload);
                Console.WriteLine("Status: " + task.Status.ToString());
                Console.WriteLine("Created at: " + task.CreatedAt.ToString());
                Console.WriteLine("Time expected: 1 sec ");
                Separator();
            });

    }

    public override async Task Show()
    {
        while (true)
        {
            await Clear();
            Console.WriteLine("Running Tasks\n\n" +
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
