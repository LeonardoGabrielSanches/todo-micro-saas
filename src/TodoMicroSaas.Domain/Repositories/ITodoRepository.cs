using TodoMicroSaas.Domain.Entities;

namespace TodoMicroSaas.Domain.Repositories;

public interface ITodoRepository
{
    Task Create(Todo todo);
}