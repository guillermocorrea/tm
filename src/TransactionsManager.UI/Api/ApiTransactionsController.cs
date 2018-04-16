using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionsManager.Domain;
using TransactionsManager.Services;
using TransactionsManager.Services.Interfaces;

namespace TransactionsManager.UI.Api
{
    [Produces("application/json")]
    [Route("api/transactions")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ApiTransactionsController : Controller
    {
        private readonly ITransactionsService _transactionsService;

        public ApiTransactionsController(ITransactionsService transactionsService)
        {
            _transactionsService = transactionsService;
        }

        public async Task<IActionResult> Post([FromBody]Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _transactionsService.Register(transaction);
            return Ok();
        }

        [Route("search")]
        [HttpGet]
        public async Task<IActionResult> Search(bool isFraud, string searchNameDest)
        {
            return Ok(await _transactionsService.Find(new TransactionsSearchCriteria()
            {
                IsFraud = isFraud,
                SearchNameDest = searchNameDest
            }, 1, 100));
        }
    }
}
