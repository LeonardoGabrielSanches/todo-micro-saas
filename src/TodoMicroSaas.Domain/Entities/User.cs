namespace TodoMicroSaas.Domain.Entities;

public class User(
    string name,
    string email,
    string customerId) : Entity
{
    private readonly List<Todo> _todos = [];

    public string Name { get; private set; } = name;
    public string Email { get; private set; } = email;
    public string CustomerId { get; private set; } = customerId;
    public string? SubscriptionId { get; private set; }

    public IEnumerable<Todo> Todos => _todos;

    public void Subscribe(string? subscriptionId)
        => SubscriptionId = subscriptionId;

    public void AddTodo(Todo todo)
        => _todos.Add(todo);
}