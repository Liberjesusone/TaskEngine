using TaskEngine.Application.Interfaces;
using TaskEngine.Application.Services;
using TaskEngine.Controller;
using TaskEngine.Infrastructure.JSONFactory;
using TaskEngine.Infrastructure.Repositories;

// Service and repository setup
DataJSONFactory _dataFactory = new DataJSONFactory();
ETaskRepository _repository = new ETaskRepository(_dataFactory);
ETaskEngine _engine = new ETaskEngine(_repository);

MenuController menu = new MenuController(_repository, _engine);
await menu.Show();    


