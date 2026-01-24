using System.Threading.Tasks;
using TaskEngine.Application.Interfaces;
using TaskEngine.Application.Services;
using TaskEngine.Domain;

namespace TaskEngine.Controller;

public class PendingController : Controller
{
    IETaskRepository _eTasksRepository;
    ETaskEngine _engine;
    
    public PendingController(IETaskRepository tasksRepository, ETaskEngine engine)
    {
        this._eTasksRepository = tasksRepository;
        this._engine = engine;
    }

    public async Task DeleteTask()
    {
        Console.WriteLine("Select the Id of the task that you want to delete");
        var idToDelete = AskANumber();
        ETask taskToDelete = _eTasksRepository.GetById(idToDelete);
        if (taskToDelete != null && taskToDelete.Status == ETaskStatus.PENDING)
        {
            bool wasDeleted = await _eTasksRepository.DeleteAsync(idToDelete);
            if (wasDeleted)
            {
                Console.WriteLine("The task was deleted successfully");
            }
        }
        else
            Console.WriteLine("The task was not found or is not pending");
        return;
    }
    public void ShowList()
    {
        _eTasksRepository.ForEach(
            task => task.Status == ETaskStatus.PENDING && !task.IsDeleted, 
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

    public async Task show()
    {
        while (true)
        {
            await Clear();
            Console.WriteLine("Pending Tasks\n\n" +
                             "1- Do all tasks\n" +
                             "2- Do an number of tasks\n" +
                             "3- Delete a task\n" +
                             "0- Back\n\n\n");

            ShowList();
            Console.WriteLine("\n\n");
            Separator();
            int option = AskANumber();

            switch (option)
            {
                case 1:
                    Console.WriteLine("Doing all tasks");
                    await _engine.HandleAllAsync();
                    PressAnyKey();
                    break;
                case 2:
                    Console.WriteLine("Select the amount of tasks that you want to do");
                    await _engine.HandleTasksAsync(AskANumber());
                    PressAnyKey();
                    break;
                case 3:
                    await DeleteTask();
                    PressAnyKey();
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Invalid option");
                    PressAnyKey();
                    break;
            }
        }
    }

}
