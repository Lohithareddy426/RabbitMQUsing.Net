using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Models;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Banking.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Banking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankingController : ControllerBase
    {
        private IAccountService _accountService;
        public BankingController(IAccountService accountService)
        {
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

        //[HttpPost()]
        //public ActionResult<IEnumerable<Account>> Post([FromBody] Account account)
        //{
        //    try
        //    {
        //       var res =  _accountService.SaveAccount(account);
        //        if(res == "Account saved successfully")
        //        {
        //            var accounts = _accountService.GetAccounts();
        //            if (accounts.Count() > 0)
        //                return Ok(accounts);
        //            else
        //                return NotFound();
        //        }
        //        return BadRequest("Account saving has issue : "+ res);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }

        //}

        [HttpPost]
        public ActionResult<IEnumerable<Account>> Post([FromBody] AccountTransfer accountTransfer)
        {
            _accountService.Transfer(accountTransfer);
            return Ok(accountTransfer);
        }
    }
}

