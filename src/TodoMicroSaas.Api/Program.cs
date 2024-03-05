using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Stripe;
using TodoMicroSaas.Domain.Interfaces;
using TodoMicroSaas.Domain.Repositories;
using TodoMicroSaas.Domain.UseCases;
using TodoMicroSaas.Infrastructure.CrossCutting.Payments;
using TodoMicroSaas.Infrastructure.Data;
using TodoMicroSaas.Infrastructure.Data.Repositories;
using CreateCheckoutSessionRequest = TodoMicroSaas.Domain.UseCases.CreateCheckoutSessionRequest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TodoMicroSaasContext>(options =>
    options.UseNpgsql(builder.Configuration["ConnectionString:DefaultConnection"]));

builder.Services.AddScoped<CreateUserUseCase>();
builder.Services.AddScoped<CreateCheckoutSessionUseCase>();
// builder.Services.AddScoped<CreateTodoUseCase>();
builder.Services.AddScoped<IPaymentService, StripeService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UpdateSubscriptionUseCase>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/users", async (CreateUserRequest request, CreateUserUseCase createUserUseCase) =>
{
    var response = await createUserUseCase.Execute(request);

    return Results.Created("", response);
});

app.MapPost("/checkout", async (HttpContext context, CreateCheckoutSessionUseCase createCheckoutSessionUseCase) =>
{
    var userId = context.Request.Headers["x-user-id"].ToString();

    var response = await createCheckoutSessionUseCase.Execute(new CreateCheckoutSessionRequest(Guid.Parse(userId)));

    return Results.Ok(new { checkoutUrl = response });
});

app.MapGet("/success",
    (IConfiguration configuration) => Results.Ok(new
        { response = $"Checkout successfully created {configuration["Stripe:EndpointSecretWebhook"]}" }));

app.MapPost("/webhook",
    async (HttpContext context, IConfiguration configuration, UpdateSubscriptionUseCase updateSubscriptionUseCase) =>
    {
        try
        {
            var json = await new StreamReader(context.Request.Body).ReadToEndAsync();

            var stripeEvent = EventUtility.ConstructEvent(json,
                context.Request.Headers["Stripe-Signature"], configuration["Stripe:EndpointSecretWebhook"]);

            if (stripeEvent.Data.RawObject.status.ToString() is not "active")
                return Results.Ok();

            switch (stripeEvent.Type)
            {
                case Events.CustomerSubscriptionCreated:
                case Events.CustomerSubscriptionUpdated:
                    await updateSubscriptionUseCase.Execute(
                        new UpdateSubscriptionRequest(stripeEvent.Data.RawObject.customer.ToString(),
                            stripeEvent.Data.RawObject.id.ToString()));
                    break;
            }

            return Results.Ok();
        }
        catch (StripeException e)
        {
            return Results.BadRequest(e.Message);
        }
    });

app.Run();