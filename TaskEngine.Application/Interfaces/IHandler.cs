namespace TaskEngine.Application.Interfaces
{
    public interface IHandler
    {
        /*
         * @brief Deserialize the JSON payload to an object with the parameters that the handler needs
         * 
         * @returns The data turned into an object
         */
        public object? Deserialize(string payload);

        /*
         * @brief Execute the task with the given payload
         * 
         * @brief returns the result of the task execution
         */
        public Task<Object?> HandleAsync(string payload);
    }
}
