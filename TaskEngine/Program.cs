using TaskEngine.Controller;
using TaskEngine.Infrastructure.JSONFactory;
using TaskEngine.Infrastructure.Repositories;

DataJSONFactory _dataFactory = new DataJSONFactory();
ETaskRepository _repository = new ETaskRepository(_dataFactory);

MenuController menu = new MenuController(_repository);
await menu.Show();    


