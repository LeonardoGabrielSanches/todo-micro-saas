using TodoMicroSaas.Domain.Entities;

namespace TodoMicroSaas.Domain.Repositories;

public interface ITodoRepository
{
    Task<IEnumerable<Todo>> GetAllByOwnerId(Guid ownerId);
    Task<Todo?> GetById(Guid id);
    Task Create(Todo todo);
    Task Update(Todo todo);
}