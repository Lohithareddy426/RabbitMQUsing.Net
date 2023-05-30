using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Banking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankingController : ControllerBase
    {
        private IAccountService _accountService;
        public BankingController(IAccountService accountService) {
            _accountService = accountService;
        }
        //Get api/banking
        [HttpGet]
        public ActionResult<IEnumerable<Account>> Get()
        {
            try
            {
                var accounts = _accountService.GetAccounts();
                if (accounts.Count() > 0)
                    return Ok(accounts);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
 
        }
    }
}

