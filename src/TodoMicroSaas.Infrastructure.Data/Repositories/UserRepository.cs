using Microsoft.EntityFrameworkCore;
using TodoMicroSaas.Domain.Entities;
using TodoMicroSaas.Domain.Repositories;

namespace TodoMicroSaas.Infrastructure.Data.Repositories;

public class UserRepository(TodoMicroSaasContext context) : IUserRepository
{
    public async Task<User?> GetById(Guid id)
        => await context.Users.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<User?> GetByCustomerId(string customerId)
        => await context.Users.FirstOrDefaultAsync(x => x.CustomerId == customerId);

    public async Task Create(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
    }

    public async Task Update(User user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }
}