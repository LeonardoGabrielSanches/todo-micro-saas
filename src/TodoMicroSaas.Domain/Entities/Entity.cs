namespace TodoMicroSaas.Domain.Entities;

public abstract class Entity
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; private set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; private set; }
}