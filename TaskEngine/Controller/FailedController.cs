using TaskEngine.Application.Interfaces;
using TaskEngine.Domain;

namespace TaskEngine.Controller;

public class FailedController : Controller
{
    IETaskRepository _eTasksRepository;
    public FailedController(IETaskRepository eTaskRepository)
    {
        this._eTasksRepository = eTaskRepository;
    }

    public void ShowList()
    {
        _eTasksRepository.ForEach(
            task => task.Status == ETaskStatus.FAILED && !task.IsDeleted,
            task =>
            {
                Separator();
                Console.WriteLine("Id: " + task.Id);
                Console.WriteLine("Type: " + task.Type.ToString());
                Console.WriteLine("Payload: " + task.Payload);
                Console.WriteLine("Status: " + task.Status.ToString());
                Console.WriteLine("Created at: " + task.CreatedAt.ToString());
                Console.WriteLine("Finished at: " + task.FinishedAt.ToString());
                Console.WriteLine("Result: -FAILED-");
                Separator();
            });
    }

    public override async Task Show()
    {
        while (true)
        {
            await Clear();
            Console.WriteLine("Failed Tasks\n\n" +
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
