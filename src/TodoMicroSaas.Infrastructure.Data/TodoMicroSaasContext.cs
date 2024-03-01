using Microsoft.EntityFrameworkCore;
using TodoMicroSaas.Domain.Entities;

namespace TodoMicroSaas.Infrastructure.Data;

public class TodoMicroSaasContext(DbContextOptions<TodoMicroSaasContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Todo> Todos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var userBuilder = modelBuilder.Entity<User>();

        userBuilder.HasKey(u => u.Id);
        userBuilder.Property(x => x.Id).HasColumnName("id");
        userBuilder.Property(x => x.Name).HasColumnName("name").IsRequired();
        userBuilder.Property(x => x.Email).HasColumnName("email").IsRequired();
        userBuilder.HasIndex(x => x.Email);
        userBuilder.Property(x => x.CustomerId).HasColumnName("customer_id").IsRequired();
        userBuilder.Property(x => x.SubscriptionId).HasColumnName("subscription_id");
        userBuilder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        userBuilder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        userBuilder.HasMany(x => x.Todos)
            .WithOne(x => x.Owner)
            .HasForeignKey(x => x.OwnerId);
        userBuilder.ToTable("users");

        var todoBuilder = modelBuilder.Entity<Todo>();

        todoBuilder.HasKey(t => t.Id);
        todoBuilder.Property(x => x.Id).HasColumnName("id");
        todoBuilder.Property(x => x.Description).HasColumnName("description").IsRequired();
        todoBuilder.Property(x => x.Done).HasColumnName("done").HasDefaultValue(false);
        todoBuilder.Property(x => x.OwnerId).HasColumnName("owner_id").IsRequired();
        todoBuilder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        todoBuilder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        todoBuilder.ToTable("todos");
    }
}