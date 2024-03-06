using Microsoft.EntityFrameworkCore;
using TodoMicroSaas.Domain.Entities;
using TodoMicroSaas.Domain.Repositories;

namespace TodoMicroSaas.Infrastructure.Data.Repositories;

public class TodoRepository(TodoMicroSaasContext context) : ITodoRepository
{
    public async Task<IEnumerable<Todo>> GetAllByOwnerId(Guid ownerId)
        => await context.Todos.Where(x => x.OwnerId == ownerId).ToListAsync();

    public async Task<Todo?> GetById(Guid id)
        => await context.Todos.FirstOrDefaultAsync(x => x.Id == id);

    public async Task Create(Todo todo)
    {
        context.Todos.Add(todo);
        await context.SaveChangesAsync();
    }

    public async Task Update(Todo todo)
    {
        context.Todos.Update(todo);
        await context.SaveChangesAsync();
    }
}