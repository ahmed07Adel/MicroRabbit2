using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Banking.Data.Repository;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.Bus;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Infra.IOC
{
    public class DependencyContainer
    {
        //register dependecy Container
        public static void RegisterServices(IServiceCollection services)
        {
            //Domain Bus
            services.AddScoped<IEventBus, RabbitMQBus>();

            //Application Services
            services.AddScoped<IAccountService, AccountService>();

            //Data
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<BankingDbContext>();
        }
    }
}
