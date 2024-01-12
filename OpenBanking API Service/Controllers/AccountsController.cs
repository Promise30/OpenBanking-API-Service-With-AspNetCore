using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OpenBanking_API_Service.Dtos.AccountsDto.Requests;
using OpenBanking_API_Service.Service.Interface;
using System.Net;

namespace OpenBanking_API_Service.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IBankAccountService _accountService;
        public AccountsController(IBankAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetBankAccounts()
        {
            var accounts = await _accountService.GetAllBankAccountsAsync(trackChanges: false);
            return accounts.StatusCode == HttpStatusCode.OK ? Ok(accounts) : StatusCode((int)accounts.StatusCode, accounts);
        }

        [HttpGet("{accountId:guid}")]
        public async Task<IActionResult> GetBankAccount(Guid accountId)
        {
            var account = await _accountService.GetBankAccountAsync(accountId, trackChanges: false);
            //return account.StatusCode == HttpStatusCode.OK ? Ok(account) : BadRequest(account);
            return account.StatusCode == HttpStatusCode.OK ? Ok(account) : StatusCode((int)account.StatusCode, account);
        }



        [Authorize]
        [HttpPost("create-account")]
        public async Task<IActionResult> CreateBankAccount(CreateBankAccount createBankAccountDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(APIResponse<ModelStateDictionary>.Create(HttpStatusCode.BadRequest, "Validation Failed", ModelState));
            }
            var response = await _accountService.CreateBankAccountAsync(createBankAccountDto);
            return response.StatusCode == HttpStatusCode.OK ? Ok(response) : StatusCode((int)response.StatusCode, response);
        }
    }
}
