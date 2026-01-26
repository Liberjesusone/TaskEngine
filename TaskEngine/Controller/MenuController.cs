using TaskEngine.Application.Interfaces;
using TaskEngine.Application.Services;

namespace TaskEngine.Controller;

public class MenuController : Controller
{
    IETaskRepository _eTasksRepository;
    ETaskEngine _engine; 
    public MenuController(IETaskRepository eTasksRepository, ETaskEngine engine)
    {
        _eTasksRepository = eTasksRepository;
        _engine = engine;
    }

    public override async Task Show()
    {
        while (true)
        {
            await Clear();
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
                case 1: //Create a Task
                    var createController = new CreateController(_eTasksRepository, _engine);
                    await createController.Show();
                    PressAnyKey();
                    break;
                case 2: // Pending tasks
                    var pendingController = new PendingController(_eTasksRepository, _engine);
                    await pendingController.show();
                    PressAnyKey();
                    break;
                case 3: // Running tasks
                    var runningController = new RunningController(_eTasksRepository);
                    await runningController.Show();
                    PressAnyKey();
                    break;
                case 4: // Completed tasks
                    var successController= new SuccessController(_eTasksRepository);
                    await successController.Show();
                    PressAnyKey();
                    break;
                case 5: // Failed tasks
                    var failedController = new FailedController(_eTasksRepository);
                    await failedController.Show();
                    PressAnyKey();
                    break;
                case 0: // Close the program
                    Console.WriteLine("Closing the program");
                    PressAnyKey();
                    return;
                default:
                    Console.WriteLine("Invalid option");
                    PressAnyKey();
                    break;
            }
        }
    }

}
