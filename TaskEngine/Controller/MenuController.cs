using TaskEngine.Application.Interfaces;

namespace TaskEngine.Controller;

public class MenuController : Controller
{
    IETaskRepository _eTasksRepository;
    public MenuController(IETaskRepository eTasksRepository)
    {
        _eTasksRepository = eTasksRepository;
    }

    public override void show()
    {
        while (true)
        { 
            Console.Clear();
            Console.WriteLine("Welcome to the Task Engine\n\n" +
                              "1- Create a Task\n" +
                              "2- Pending tasks\n" +
                              "3- Running tasks\n" +
                              "4- Completed tasks\n" +
                              "5- Failed tasks\n" +
                              "0- Close the program\n\n\n");
            Separator();
            int option = AskANumber();

            switch (option)
            {
                case 1:
                    Console.WriteLine("Create a Task");
                    Console.ReadLine();
                    break;
                case 2:
                    Console.WriteLine("Pending tasks");
                    var pendingController = new PendingController(_eTasksRepository);
                    pendingController.show();
                    Console.ReadLine();
                    break;
                case 3:
                    Console.WriteLine("Running Tasks");
                    var runningController = new RunningController(_eTasksRepository);
                    runningController.show();
                    Console.ReadLine();
                    break;
                case 4:
                    Console.WriteLine("Completed tasks");
                    var successController= new SuccessController(_eTasksRepository);
                    successController.show();
                    Console.ReadLine();
                    break;
                case 5:
                    Console.WriteLine("Failed tasks");

                    var failedController = new FailedController(_eTasksRepository);
                    failedController.show();
                    Console.ReadLine();
                    break;
                case 0:
                    Console.WriteLine("Closing the program");
                    Console.ReadLine();
                    System.Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid option");
                    Console.ReadLine();
                    break;
            }
        }
    }

}
