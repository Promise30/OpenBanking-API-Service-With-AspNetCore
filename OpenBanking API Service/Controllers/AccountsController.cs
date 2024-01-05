using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OpenBanking_API_Service.Dtos.AccountsDto;
using OpenBanking_API_Service.Service.Interface;
using OpenBanking_API_Service_Common.Library.Models;
using System.Net;

namespace OpenBanking_API_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [Authorize]
        [HttpPost("create-account")]
        public async Task<IActionResult> CreateBankAccount(CreateBankAccountDto createBankAccountDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(APIResponse<ModelStateDictionary>.Create(HttpStatusCode.BadRequest, "Validation Failed", ModelState));
            }
            var response = await _accountService.CreateBankAccount(createBankAccountDto);
            return StatusCode((int)response.StatusCode, response.Data);
        }
    }
}
