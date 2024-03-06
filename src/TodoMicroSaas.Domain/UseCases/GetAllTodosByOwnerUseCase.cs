using TodoMicroSaas.Domain.Entities;
using TodoMicroSaas.Domain.Repositories;

namespace TodoMicroSaas.Domain.UseCases;

public record GetAllTodosByOwnerRequest(Guid OwnerId);

public class GetAllTodosByOwnerUseCase(ITodoRepository todoRepository)
{
    public async Task<IEnumerable<Todo>> Execute(GetAllTodosByOwnerRequest request)
    {
        var todos = await todoRepository.GetAllByOwnerId(request.OwnerId);

        return todos;
    }
}