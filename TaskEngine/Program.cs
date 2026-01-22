using TaskEngine.Infrastructure;
using TaskEngine.Infrastructure.JSONFactory;
using TaskEngine.Infrastructure.Repositories;



// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

DataJSONFactory _dataFactory = new DataJSONFactory();
ETaskRepository _repository = new ETaskRepository(_dataFactory);


