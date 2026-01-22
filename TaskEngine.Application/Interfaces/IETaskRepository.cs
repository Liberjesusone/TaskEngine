using TaskEngine.Domain;
namespace TaskEngine.Application.Interfaces;

public interface IETaskRepository
{
    public Task<ETask> AddAsync(ETask task);
    public Task<ETask> EditAsync(ETask task);
    public Task<bool> DeleteAsync(int id);
    public ETask? GetById(int id);
}
