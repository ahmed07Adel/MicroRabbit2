using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Models;
using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Domain.Models;
using MicroRabbit.Domain.Core.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Banking.Application.Services
{
    public class AccountService : IAccountService
    {
        private IAccountRepository accountRepository;
        private readonly IEventBus eventBus;

        public AccountService(IAccountRepository accountRepository, IEventBus eventBus)
        {
            this.accountRepository=accountRepository;
            this.eventBus=eventBus;
        }
        public IEnumerable<Account> GetAccounts()
        {
            return accountRepository.GetAccounts();   
        }

        public void Transfer(AccountTransfer accountTransfer)
        {
            var createTransferCommand = new CreateTransferCommand(
                accountTransfer.FromAccount,
                accountTransfer.ToAccount,
                accountTransfer.TransferAmount
                );
            eventBus.SendCommand(createTransferCommand);
        }
    }
}
