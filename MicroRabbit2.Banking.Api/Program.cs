using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Banking.Data.Repository;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.Bus;
using MicroRabbit.Infra.IOC;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMvc();

builder.Services.AddDbContext<BankingDbContext>(a => a.UseSqlServer(builder.Configuration
                                                                           .GetConnectionString("BankingDBConnection")));

// void ConfigureServices(IServiceCollection services)

//{
//    DependencyContainer.RegisterServices(services);
//}
//Domain Bus
builder.Services.AddScoped<IEventBus, RabbitMQBus>();

//Application Services
builder.Services.AddScoped<IAccountService, AccountService>();

//Data
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<BankingDbContext>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
