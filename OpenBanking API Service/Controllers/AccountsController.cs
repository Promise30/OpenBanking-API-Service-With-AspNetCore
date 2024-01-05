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

        [Authorize]
        [HttpPost("account-deposit")]
        public async Task<IActionResult> AccountDeposit(string accountNumber, CreateBankAccountDepositDto bankAcountDepositDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(APIResponse<ModelStateDictionary>.Create(HttpStatusCode.BadRequest, "Validation Failed", ModelState));
            }
            var transfer = await _accountService.BankAccountDeposit(accountNumber, bankAcountDepositDto);
            if (transfer.Data != null)
            {
                return StatusCode((int)transfer.StatusCode, transfer.Data);
            }
            return StatusCode((int)transfer.StatusCode, transfer.StatusMessage);
        }

        [Authorize]
        [HttpPost("account-withdrawal")]
        public async Task<IActionResult> AccountWithdrawal(CreateBankAccountWithdrawal bankAccountWithdrawal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(APIResponse<ModelStateDictionary>.Create(HttpStatusCode.BadRequest, "Validation Failed", ModelState));
            }
            var result = await _accountService.BankAccountWithdrawal(bankAccountWithdrawal);
            return result.StatusCode == HttpStatusCode.OK ? Ok(result) : BadRequest(result);
        }

        [Authorize]
        [HttpPost("account-transfer")]
        public async Task<IActionResult> AccountTransfer(CreateAccountTransfer amountTransfer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(APIResponse<ModelStateDictionary>.Create(HttpStatusCode.BadRequest, "Validation Failed", ModelState));
            }
            var result = await _accountService.BankAccountTransfer(amountTransfer);
            return result.StatusCode == HttpStatusCode.OK ? Ok(result) : BadRequest(result);
        }

        [HttpGet("transactions-history")]
        public async Task<IActionResult> TransactionsHistory(Guid accountId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(APIResponse<ModelStateDictionary>.Create(HttpStatusCode.BadRequest, "Validation Failed", ModelState));
            }
            var result = await _accountService.BankAccountTransactionHistory(accountId);
            return result.StatusCode == HttpStatusCode.OK ? Ok(result) : BadRequest(result);
        }
    }
}
