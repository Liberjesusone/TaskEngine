# TaskEngine

## E-Task  Engine

An asynchronous ETask processing engine developed in C#. This system allows managing and running tasks in parallel (such as text processing or mathematical calculations) without blocking the user interface, maintaining a persistent record in JSON format.

### Main characteristics 
- **ETask:** the representation of a task with a payload(data) and a type, represented as EngineTask -> ETask 
- **Asynchronous engine:** Non-blocking processing using Task.WhenAll and background flows, works as a queue of ETasks, in order of creation .
- **Parallel Processing:** Ability to run multiple tasks simultaneously (ideal for tasks with latency or delays).
- **Extendable Handlers:** Interface-based system (IHandler) to add new functions easily.
- **Resilience (Fault Tolerance):** Robust error handling against corrupt or malformed JSON payloads.
- **Persistence:** Automatic saving of task status in "...\Data\tasks.json" file.

### 
- **Runtime:** .NET 9.0 SDK o higher.
- **IDE (Opcional):** Visual Studio 2022 *(Used in this project)*, VS Code o JetBrains Rider.


### Installation and compilation
If you want to clone the repository and compile it yourself, follow these steps:

- clone the repository:

Bash
git clone https://github.com/tu-usuario/e-task-manager.git
cd e-task-manager

- Restore dependencies:

Bash
dotnet restore
compile the project:


Bash
dotnet build --configuration Release
Execution
From the source code
Once compiled, you can start the application from the project root:


Bash
dotnet run --project TaskEngine/TaskEngine.csproj
From binary files (bin folder)
If you already have the generated executable (located in bin/Release/net9.0/):

Windows: Ejecuta TuProyecto.exe.
Linux/macOS: Ejecuta dotnet TuProyecto.dll.

#### If you have VS 2022
You only have to clone the repository in your prefered folder, then open the solution .sln file with vs2022 and run the compilation in the prefered mode (Debug/Release), then you will find the binaries in the destination folder "...\TaskEngine\TaskEngine\bin\Release\net9.0\TaskEngine.exe" or "\Debug\net9.0\TaskEngine.exe" 



---
### How does it works? 
The system uses 4 main states for tasks:

- **PENDING:** An ETask thats has been created and EnQueued.
- **RUNNING:** ETasks that are been processed right now, (you can go back to the Running Tasks Menu while this is happening).
- **SUCCESS / FAILED:** Final result after processing.

#### Use case:
By selecting "Process All Tasks", the engine will launch all pending tasks in background threads. You will be able to navigate to the Running Tasks menu and see how they are completed in real time, even if they have long or diferent waiting times.

#### Tests
The Engine has a "...\Data\tasks.json" file in destination folder where has all current ETasks, you can edit this file by copy-pasting all the ETasks from the other files in the same folder into the "tasks.json" file that is the "DataBase" file 

The engine has been tested with: 
- Wrong data types.
- JSON with wrong syntax.
- Missing properties.

### Architecture
This project was designed following the principles of Clean Code and asynchronously. With a scalable structure in order to create new types of tasks easily, just by creating new handlers

#### Folders 
````text
├───README.md
│
├───TaskEngine
│   │   Program.cs
│   │   TaskEngine.csproj
│   │   TaskEngine.sln
│   │
│   ├───bin
│   │   ├───Debug
│   │   │   └───net9.0
│   │   │       │   TaskEngine.exe
│   │   │       │
│   │   │       └───Data
│   │   │               81_TASKS.json
│   │   │               GET_MEAN_TASKS.json
│   │   │               tasks.json
│   │   │               TASKS_TO_FAIL.json
│   │   │               TO_LOWER_TASKS.json
│   │   │               TO_UPPER_TASKS.json
│   │   │
│   │   └───Release
│   │       └───net9.0
│   │           │   TaskEngine.exe
│   │           │
│   │           └───Data
│   │                   81_TASKS.json
│   │                   GET_MEAN_TASKS.json
│   │                   tasks.json
│   │                   TASKS_TO_FAIL.json
│   │                   TO_LOWER_TASKS.json
│   │                   TO_UPPER_TASKS.json
│   │
│   └───Controller
│           Controller.cs
│           CreateController.cs
│           FailedController.cs
│           MenuController.cs
│           PendingController.cs
│           RunningController.cs
│           SuccessController.cs
│
├───TaskEngine.Application
│   │
│   ├───DTOs
│   ├───Handlers
│   │       GET_MEAN_H.cs
│   │       TO_LOWER_H.cs
│   │       TO_UPPER_H.cs
│   │
│   ├───Interfaces
│   │       IETaskRepository.cs
│   │       IHandler.cs
│   │
│   └───Services
│           ETaskEngine.cs
│
├───TaskEngine.Domain
│       ETask.cs
│       ETaskStatus.cs
│       ETaskType.cs
│       TaskEngine.Domain.csproj
│
└───TaskEngine.Infrastructure
    │   TaskEngine.Infrastructure.csproj
    │
    ├───Data
    │       81_TASKS.json
    │       GET_MEAN_TASKS.json
    │       tasks.json
    │       TASKS_TO_FAIL.json
    │       TO_LOWER_TASKS.json
    │       TO_UPPER_TASKS.json
    │
    ├───DataJson
    │       DataJSONFactory.cs
    │
    └───Repositories
            ETaskRepository.cs
````

##### .TaskEngine
This project is meant to be the front end app, it can be a Web API, a web application or a desktop app if its needed, you can find there the controllers, that are meant to be the pages of the menu  
##### .Application 
- This project is business logic layer, where the DTO(Data Transfers Objects) are placed, but at this moment there aren't any.
- Here there is also the ETaskEngine, the service created to store the Queue of ETasks that are going to be processed when needed and it also manages the parallelism and priority of the tasks (In creation order -> FIFO - First In, First Out).
It also has a dictionary to map the ETask type, with its corresponding handler
- In this project you will find the interfaces and services, but most important, the handlers, this objects are the ones that process the Types of ETask, if a ETaskType (TO_UPPER_H -> a task that has a payload that only the handler: TO_UPPER_H.cs can process) has to be process, the engine finds the correct handler with its Type, and delegates the processing work to the handler, so every Type of ETask has to have its own Handler, thats why the ETaskType are named after them. 
They also have a method to ask the user in a controlled way to enter data for the payload
##### .Domain
In this project you will find the entities of the project ready to be stored 
##### .Infrastructure
In this project there is the persistent data layer where you find the Repository to store ETasks in memory and jsons, it also has a "\Data" folder with a backing of the jsons 


### To do (things to improve or implement in the future)
This comments and things to improved are marked in code with the comment "  // TODO:  " for you to find them:
- At the moment there are only 3 types of ETasks with its handlers:
    - TO_UPPER_H : turns the text to uppercase   (25 sec)
    - TO_LOWER_H : turns the text to lowercase   (15 sec)
    - GET_MEAN_H : gets the mean or average of the list of doubles (5 sec)

    - as those tasks are so fast for the processor, there are implemented some intentional Task.Delay() to wait some seconds for them to complete, that way you can experiment parallelism 

- In this method from "ETaskEngine.cs" we save in memory just after processing all tasks, we can change this behavior if needed. 
    - Processes up to 'n' tasks from the queue.
    - n = The amount of eTasks that are going to be process
    - public async Task HandleTasksAsync(int n);
    - So if we change the data persistent implementation to a DataBase, we will better change this to save tasks as they are finishing 

- After finishing every ETask no matter the result, we can store a log in a data base or json to manage the history of interactions 

- In "ETasksRepository.cs" at the moment if the SaveAllAsync() method *(That saves the Repository of ETasks in jsons files)* fails we just show the error in console, but a better implementation would be to store a task to try to save it again later, because it could be store in memory but not in jsons if somethings goes wrong.   

- At the moment we are only looking for the greater ID in the ETaskRepository to create the new one, for a better implementation would be to store a Id counter in a settings.json file

- In the CreateController.cs, and SuccessController.cs when we are showing the payload that the user is editing or the ETask has, we show the Json object deserialized, but we can improve it visually to compare in proper way the payload with the result. 

- In the RunningController.cs when we list all running ETasks, we only show 1 sec remaining for every task (its hardcoded) so a better implementation would be to have a progress score or bar controlled by the corresponding Handler

If you have another recommendation or note to make about this project do not hesitate to tell me.