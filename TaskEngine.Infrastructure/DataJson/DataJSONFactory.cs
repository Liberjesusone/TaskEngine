using System.Text.Json;
using TaskEngine.Domain;

namespace TaskEngine.Infrastructure.JSONFactory
{
    // The DataJSONFactory class is responsible for managing the storage and retrieval
    public class DataJSONFactory
    {
        // This is where data comes from, a folder that is copied to the output directory
        // and in the project is located in the TaskEngine.Infrastructure.Data folder
        private readonly string _filePath = "";
        private readonly string _directory = "";

        public DataJSONFactory()
        {
            // We define the rute: Data/tasks.json    to save the tasks
            // AppDomain.CurrentDomain.BaseDirectory works on any PC
            _directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            _filePath = Path.Combine(_directory, "tasks.json");

            EnsureFileExists();
        }

        // Creates the data directory and file if they do not exist
        private void EnsureFileExists()
        {
            if (!Directory.Exists(_directory))
                Directory.CreateDirectory(_directory);

            // We create a file if it doesn't exist with an empty list
            if (!File.Exists(_filePath))
                File.WriteAllText(_filePath, "[]");
        }

        /// <summary>
        /// Gets all tasks from the JSON file asynchronously.
        /// </summary>
        /// <returns> A list of ETask objects representing all tasks stored in the JSON file</returns>
        public async Task<List<ETask>> GetAllInternalAsync()
        {
            EnsureFileExists();

            var json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<ETask>>(json) ?? new List<ETask>();
        }

        /// <summary>
        /// Saves all tasks to the JSON file asynchronously.
        /// </summary>
        /// <param name="tasks">A list of ETask objects to be saved to the JSON file.</param>
        /// <returns>
        /// <c>true</c> if the tasks were saved successfully; otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> SaveAllAsync(List<ETask> tasks)
        {
            // If tasks is null, that means the list haven't been created yet
            if (tasks is null)
            {
                Console.WriteLine("List of tasks has not been created");
                return false;
            }

            EnsureFileExists();

            var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(tasks, jsonOptions);

            // Create a temp file in the same directory to ensure Move is atomic on same volume.
            var tempFileName = Path.Combine(_directory, Path.GetRandomFileName());
            try
            {
                await File.WriteAllTextAsync(tempFileName, json);

                // Move temp file to destination, overwrite if exists 
                File.Move(tempFileName, _filePath, overwrite: true);
            }
            catch (Exception ex) 
            {
                // Attempt to clean up temp file if it exists afteher failing to overwrite, ignore any errors during cleanup.
                try
                {
                    if (File.Exists(tempFileName))
                        File.Delete(tempFileName);
                }
                catch (Exception cleanupEx)
                {
                    Console.Error.WriteLine($"Warning: failed to delete temp file '{tempFileName}' during catch cleanup: {cleanupEx}");
                }

                Console.Error.WriteLine($"Error saving tasks to '{_filePath}': {ex}");
                return false;
            }
            finally
            {
                // Attempt to clean up temp file after overwrite, ignore any errors during cleanup.
                try
                {
                    if (File.Exists(tempFileName))
                        File.Delete(tempFileName);
                }
                catch (Exception cleanupEx)
                {
                    Console.Error.WriteLine($"Warning: failed to delete temp file '{tempFileName}' in finally: {cleanupEx}");
                }
            }
            return true;
        }

    }

}

