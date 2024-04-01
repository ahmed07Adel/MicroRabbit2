using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Models;
using MicroRabbit.Banking.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit2.Banking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankingController : ControllerBase
    {
        private IAccountService accountService;

        public BankingController(IAccountService accountService)
        {
            this.accountService=accountService;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Account>> Get()
        {
            return Ok(accountService.GetAccounts());
        }
            
        [HttpPost]
        public ActionResult Post([FromBody] AccountTransfer accountTransfer)
        {
            return Ok(accountTransfer);
        }
    }
}
