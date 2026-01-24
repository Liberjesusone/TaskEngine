namespace TaskEngine.Application.Interfaces
{
    public interface IHandler
    {
        /// <summary>
        /// Deserialize the JSON payload to an object with the parameters that the handler needs
        /// </summary>
        /// <param name="payload"></param>
        /// <returns>The data turned into an object</returns>
        public object? Deserialize(string payload);

        
        /// <summary>
        /// Execute the task with the given payload
        /// </summary>
        /// <param name="payload"></param>
        /// <returns>Returns the result of the task execution</returns>
        public Task<Object?> HandleAsync(string payload);


        /// <summary>
        /// Get the payload from the user input 
        /// </summary>
        /// <returns>Returns the json object already serialized</returns>
        string GetPayloadFromUser();
    }
}
