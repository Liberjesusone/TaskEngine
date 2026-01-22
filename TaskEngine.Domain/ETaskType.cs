namespace TaskEngine.Domain;


// The type of tasks available in the system. this is the key to find the appropriate handler to process the type task.
// _H = Handler
public enum ETaskType
{
    TO_UPPER_H,
    TO_LOWER_H,
    GET_MEAN_H
}
