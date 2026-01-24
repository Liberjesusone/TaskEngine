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

    /// OJO in this method we can add some parallelism if needed in the future
    /// OJO in this method we save in memory just after processing all tasks, we can change this behavior if needed
    /// <summary>
    /// Processes up to 'n' tasks from the queue.
    /// </summary>
    /// <param name="n">The amount of eTasks that are going to be process</param>
    public async Task HandleTasksAsync(int n)
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

                    // OJO here we can do something with the result if needed
                    Console.WriteLine("Result: " + taskToProcess.Result + "\n");
                }
                else
                {
                    Console.WriteLine($"No se encontró un Handler para el tipo: {taskToProcess.Type}");
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
