using TaskEngine.Application.Interfaces;
using TaskEngine.Domain;

namespace TaskEngine.Controller;

public class PendingController : Controller
{
    IETaskRepository _eTasksRepository;
    public PendingController(IETaskRepository tasksRepository)
    {
        this._eTasksRepository = tasksRepository;
    }

    public void ShowList()
    {
        _eTasksRepository.ForEach(
            task => task.Status == ETaskStatus.PENDING, 
            task => 
            {
                Separator();
                Console.WriteLine("Id: " + task.Id);
                Console.WriteLine("Type: " + task.Type.ToString());
                Console.WriteLine("Payload: " + task.Payload);
                Console.WriteLine("Status: " + task.Status.ToString());
                Console.WriteLine("Created at: " + task.CreatedAt.ToString());
                Separator();
            });

    }

    public override void show()
    {
        bool loop = true;
        while (loop)
        {
            Console.Clear();
            Console.WriteLine("Pending Tasks\n\n" +
                             "1- Do all tasks\n" +
                             "2- Do an number of tasks\n" +
                             "0- Back\n\n\n");

            ShowList();
            Console.WriteLine("\n\n");
            Separator();
            int option = AskANumber();

            switch (option)
            {
                case 1:
                    Console.WriteLine("Doing all tasks");
                    Console.ReadLine();
                    break;
                case 2:
                    Console.WriteLine("Select the amount of tasks that you want to do");
                    Console.ReadLine();
                    break;
                case 0:
                    Console.WriteLine("Coming Back");
                    loop = false;
                    break;
                default:
                    Console.WriteLine("Invalid option");
                    Console.ReadLine();
                    break;
            }
        }
    }

}
