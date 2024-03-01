namespace TodoMicroSaas.Domain.Entities;

public class Todo(
    string description,
    Guid ownerId) : Entity
{
    public string Description { get; private set; } = description;
    public bool Done { get; private set; }
    public Guid OwnerId { get; private set; } = ownerId;

    public User? Owner { get; private set; }
}