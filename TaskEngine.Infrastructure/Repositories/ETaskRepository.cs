using TaskEngine.Domain;
using TaskEngine.Application.Interfaces;
using TaskEngine.Infrastructure.JSONFactory;

namespace TaskEngine.Infrastructure.Repositories;

public class ETaskRepository : IETaskRepository
{
    private readonly DataJSONFactory _dataFactory;
    private List<ETask> _eTasksList = new List<ETask>();

    public ETaskRepository(DataJSONFactory _dataFactory)
    {
        this._dataFactory = _dataFactory;

        // Load existing tasks from the JSON file
        _eTasksList = _dataFactory.GetAllInternalAsync().GetAwaiter().GetResult();
    }


    // OJO no se esta haciendo ningun control de los true or false de los saveAllAsync
    // OJO hay que buscar una mejor manera de manejar los IDS
    /// <summary>
    /// Asynchronously adds a new ETask to the collection and assigns it a unique Id.
    /// </summary>
    /// <param name="task">The ETask instance to add.</param>
    /// <returns>The added ETask with its assigned Id.</returns>
    public async Task<ETask> AddAsync(ETask task)
    {
        // Assign a new Id
        task.Id = _eTasksList.Any() ? _eTasksList.Max(t => t.Id) + 1 : 1;

        _eTasksList.Add(task);
        var result = await _dataFactory.SaveAllAsync(_eTasksList);
        if (result)
            Console.WriteLine("Task added successfully.");
        else
            Console.WriteLine("Task added but not saved in memory");

        return task;
    }

    /// <summary>
    /// Returns the ETask with the specified Id, or null if not found.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ETask? GetById(int id)
    {
        return _eTasksList.FirstOrDefault(t => t.Id == id);
    }

    /// <summary>
    /// Looks for the ETask in the collection with the same Id and updates its properties using the task parameter.
    /// </summary>
    /// <param name="editedTask"></param>
    /// <returns>The edited ETask </returns>
    public async Task<ETask> EditAsync(ETask editedTask)
    {
        for (int i = 0; i < _eTasksList.Count; ++i)
        {
            if (editedTask.Id == _eTasksList[i].Id)
            {
                _eTasksList[i] = editedTask;
                break;
            }
        }

        var result = await _dataFactory.SaveAllAsync(_eTasksList);
        if (result)
            Console.WriteLine("Task edited successfully.");
        else
            Console.WriteLine("Task edited but not saved in memory");

        return editedTask;
    }

    /// <summary>
    /// Marks the ETask with the specified Id as deleted.
    /// </summary>
    /// <param name="id">Of the ETask that we want to delete.</param>
    /// <returns> 
    /// <c>true</c> if the ETask was found and marked as deleted; otherwise, <c>false</c>.
    /// </returns>
    public async Task<bool> DeleteAsync(int id)
    {
        var taskToDelete = _eTasksList.FirstOrDefault(t => t.Id == id);

        if (taskToDelete == null)
            return false;
        else 
            taskToDelete.IsDeleted = true;

        var result = await _dataFactory.SaveAllAsync(_eTasksList);

        if (result)
            Console.WriteLine("Task deleted successfully and saved in memory.");
        else
            Console.WriteLine("Task deleted but not updated in memory");
        return true;
    }

    /// <summary>
    /// Goes for each task that meets a condition and performs an action on it.
    /// </summary>
    public void ForEach(Predicate<ETask> condition, Action<ETask> action)
    {
        foreach (var task in _eTasksList.Where(t => condition(t)))
        {
            // We pass a clone to avoid external modifications
            action(task.Clone());
        }
    }

    /// <summary>
    /// Returns the list of pending tasks that are not marked as deleted.
    /// </summary>
    public List<ETask> GetPendingTasks()
    {
        return _eTasksList.Where(t => t.Status == ETaskStatus.PENDING && !t.IsDeleted).ToList();
    }

    public bool SaveAll()
    {
        var result = _dataFactory.SaveAllAsync(_eTasksList).GetAwaiter().GetResult();
        if (result)
            Console.WriteLine("All tasks saved in memory.");
        else
            Console.WriteLine("Tasks not saved in memory");
        return result;
    }

}
