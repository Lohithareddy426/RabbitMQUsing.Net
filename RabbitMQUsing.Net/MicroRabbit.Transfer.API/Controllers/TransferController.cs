using MicroRabbit.Transfer.Application.Interfaces;
using MicroRabbit.Transfer.Domain.Models;

using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Transfer.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransferController : ControllerBase
    {

        private ITransferService _transferService;
        public TransferController(ITransferService transferService)
        {
            _transferService = transferService;
        }
        //Get api/banking
        [HttpGet]
        public ActionResult<IEnumerable<TransferLog>> Get()
        {
            try
            {
                var accounts = _transferService.GetTransferLogs();
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