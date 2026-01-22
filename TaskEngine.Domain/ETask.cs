namespace TaskEngine.Domain;

// The tasks doesn't store the result, so it just process the payload and change the status.
// result can be showed in logs or other systems.
public class ETask
{
    public int? Id { get; set; }

    public ETaskType Type { get; set; }

    public string? Payload { get; set; }

    public ETaskStatus Status { get; set; } = ETaskStatus.PENDING;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime? FinishedAt { get; set; }

    public bool IsDeleted { get; set; } = false;


    public ETask()
    {

    }
}
