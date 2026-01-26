using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using TaskEngine.Application.Handlers;
using TaskEngine.Application.Interfaces;
using TaskEngine.Domain;

namespace TaskEngine.Application.Services;

public class ETaskEngine
{
    private Queue<ETask> TaskQueue { get; set; } = new Queue<ETask>();
    private readonly IETaskRepository _repository;

    // Handlers dictionary to map ETask types to their respective handlers
    // Any new handler must be added here to be used by the engine
    private Dictionary<ETaskType, IHandler> handlers = new Dictionary<ETaskType, IHandler>()
    {
        { ETaskType.GET_MEAN_H, new GET_MEAN_H() },
        { ETaskType.TO_UPPER_H, new TO_UPPER_H() },
        { ETaskType.TO_LOWER_H, new TO_LOWER_H() }
    };

    public ETaskEngine(IETaskRepository repository)
    {
        _repository = repository;
        foreach (var task in repository.GetPendingTasks())
        {
            TaskQueue.Enqueue(task);
        }
    }

    public async Task HandleAllAsync()
    {
        await HandleTasksAsync(TaskQueue.Count);
    }

    /// TODO: in this method we save in memory just after processing all tasks, we can change this behavior if needed
    /// <summary>
    /// Processes up to 'n' tasks from the queue.
    /// </summary>
    /// <param name="n">The amount of eTasks that are going to be process</param>
    public async Task HandleTasksAsyncNotParallel(int n)
    {
        if (n < 0)
            Console.WriteLine("Number of tasks to process must be positive");

        while (n > 0)
        {
            // If there is a task to process and it's not deleted or null
            if (DeQueue(out ETask? taskToProcess))
            {
                if (taskToProcess == null || taskToProcess.IsDeleted)
                    continue;

                // Try to get the handler for the task type
                if (handlers.TryGetValue(taskToProcess.Type, out var handler))
                {
                    taskToProcess.Status = ETaskStatus.RUNNING;
                    var result = await handler.HandleAsync(taskToProcess.Payload ?? "");

                    if (result == null) // If null the payload wasn't in the correct format 
                    {
                        taskToProcess.Status = ETaskStatus.FAILED;
                        continue;
                    }
                    else
                    {
                        taskToProcess.Result = JsonSerializer.Serialize(result);
                        taskToProcess.Status = ETaskStatus.SUCCESS;
                        taskToProcess.FinishedAt = DateTime.Now;
                    }

                    // TODO: here we can do something with the result if needed
                    Console.WriteLine("Result: " + result.ToString() + "\n");
                }
                else
                {
                    Console.WriteLine($"The handler for the type: {taskToProcess.Type} was not found");
                    break;
                }
            }
            else
                break; // there are no more tasks in queue
            --n;
        }
        _repository.SaveAll();
    }

    /// <summary>
    /// Processes up to 'n' tasks from the queue in parallel.
    /// </summary>
    /// <param name="n">The amount of eTasks that are going to be process</param>
    public async Task HandleTasksAsync(int n)
    {
        if (n <= 0)
        { 
            Console.WriteLine("Number of tasks to process must be positive");
            return; 
        }

        // List to hold the running tasks for parallel execution
        List<Task> runningTasks = new List<Task>();

        Console.WriteLine($"Doing {n} tasks\n");
        // If there is a task to process and it's not deleted or null
        while (n > 0 && DeQueue(out ETask? taskToProcess))
        {
            if (taskToProcess == null || taskToProcess.IsDeleted) continue;

            // Try to get the handler for the task type
            if (handlers.TryGetValue(taskToProcess.Type, out var handler))
                runningTasks.Add(ProcessSingleTaskAsync(taskToProcess, handler));
            else
                Console.WriteLine($"The handler for the type: {taskToProcess.Type} was not found");
            --n;
        }

        // Execute all tasks in parallel
        await Task.WhenAll(runningTasks);

        _repository.SaveAll();
    }

    /// <summary>
    /// Here we process a single task and mark the status accordingly
    /// </summary>
    /// <param name="task"> the task to process</param>
    /// <param name="handler">the handler that is gonnar process the task</param>
    /// <returns></returns>
    private async Task ProcessSingleTaskAsync(ETask task, IHandler handler)
    {
        task.Status = ETaskStatus.RUNNING;

        // here we wait just fot this single handler to process the task but other tasks can run in parallel
        var result = await handler.HandleAsync(task.Payload ?? "");

        if (result == null) //If null the payload wasn't in the correct format 
        {   
            task.Status = ETaskStatus.FAILED;
            task.FinishedAt = DateTime.Now;
            Console.WriteLine($"Task {task.Id} Result: -FAILED-\n");
        }
        else
        {
            task.Result = JsonSerializer.Serialize(result);
            task.Status = ETaskStatus.SUCCESS;
            task.FinishedAt = DateTime.Now;
            Console.WriteLine($"Task {task.Id} completed Result: {result.ToString()} \n");
        }
        // TODO: here we can do something with the result if needed like a Log to store the history
    }


    /// <summary>
    /// Adds the specified task to the queue if it is not null.
    /// </summary>
    /// <param name="taskToAdd">The task to be added to the queue.</param>
    /// <returns>True if the task was added; otherwise, false.</returns>
    public bool EnQueue(ETask taskToAdd)
    {
        if (taskToAdd == null)
        {
            Console.WriteLine("Task invalid cannot be added");
            return false;
        }

        TaskQueue.Enqueue(taskToAdd);
        return true;
    }

    /// <summary>
    /// Attempts to dequeue a task from the queue.
    /// </summary>
    /// <param name="taskToProcess">When this method returns, contains the dequeued task if available; otherwise, null.</param>
    /// <returns>true if a task was dequeued; otherwise, false.</returns>
    public bool DeQueue(out ETask? taskToProcess)
    {
        if (TaskQueue.Count == 0)
        {
            taskToProcess = null;
            Console.WriteLine("Queue of Tasks empty");
            return false;
        }
        taskToProcess = TaskQueue.Dequeue();
        return true;
    }

    public IHandler? GetHandler(ETaskType type)
    {
        if (handlers.TryGetValue(type, out var handler))
            return handler;
        return null;
    }
}
