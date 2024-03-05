using TodoMicroSaas.Domain.Entities;

namespace TodoMicroSaas.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetById(Guid id);
    Task<User?> GetByCustomerId(string customerId);
    Task Create(User user);
    Task Update(User user);
}