using TodoMicroSaas.Domain.Entities;
using TodoMicroSaas.Domain.Repositories;

namespace TodoMicroSaas.Infrastructure.Data.Repositories;

public class TodoRepository(TodoMicroSaasContext context) : ITodoRepository
{
    public async Task Create(Todo todo)
    {
        context.Todos.Add(todo);
        await context.SaveChangesAsync();
    }
}