using Microsoft.EntityFrameworkCore;
using TodoMicroSaas.Domain.Interfaces;
using TodoMicroSaas.Domain.UseCases;
using TodoMicroSaas.Infrastructure.CrossCutting.Payments;
using TodoMicroSaas.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TodoMicroSaasContext>(options =>
    options.UseNpgsql(builder.Configuration["ConnectionString:DefaultConnection"]));

builder.Services.AddScoped<CreateUserUseCase>();
builder.Services.AddScoped<CreateTodoUseCase>();
builder.Services.AddScoped<IPaymentService, StripeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();