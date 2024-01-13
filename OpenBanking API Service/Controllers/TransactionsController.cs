using Microsoft.AspNetCore.Mvc;
using OpenBanking_API_Service.Dtos.AccountsDto.Requests;
using OpenBanking_API_Service.RequestFeatures;
using OpenBanking_API_Service.Service.Interface;
using System.Net;
using System.Text.Json;

namespace OpenBanking_API_Service.Controllers
{
    [Route("api/accounts/{accountId}")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public TransactionsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        //[HttpGet("{accountId:guid}/deposits")]
        [HttpGet("deposits")]
        public async Task<IActionResult> GetBankDepositsForAccount(Guid accountId, [FromQuery] AccountTransactionParameters accountTransactionParameters)
        {
            var pagedResult = await _serviceManager.TransactionService.GetBankAccountDepositsAsync(accountId, accountTransactionParameters, trackChanges: false);
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));
            return pagedResult.Item1.StatusCode == HttpStatusCode.OK ? Ok(pagedResult.Item1) : StatusCode((int)pagedResult.Item1.StatusCode, pagedResult.Item1);
        }

        [HttpGet("deposits/{id:guid}", Name = "GetBankDepositForAccount")]
        public async Task<IActionResult> GetBankDepositForAccount(Guid accountId, Guid id)
        {
            var deposit = await _serviceManager.TransactionService.GetBankAccountDepositAsync(accountId, id, trackChanges: false);
            return deposit.StatusCode == HttpStatusCode.OK ? Ok(deposit) : StatusCode((int)deposit.StatusCode, deposit);
        }

        [HttpPost("deposits")]
        public async Task<IActionResult> CreateBankDeposit(CreateBankAccountDeposit bankAccountDeposit)
        {
            var deposit = await _serviceManager.TransactionService.CreateBankAccountDepositAsync(bankAccountDeposit, trackChanges: true);
            return deposit.StatusCode == HttpStatusCode.Created ? CreatedAtRoute("GetBankDepositForAccount", new { id = deposit.Data.Id, accountId = deposit.Data.AccountId }, deposit) : StatusCode((int)deposit.StatusCode, deposit);
        }


        [HttpGet("withdrawals")]
        public async Task<IActionResult> GetBankWithdrawalsForAccount(Guid accountId, [FromQuery] AccountTransactionParameters accountTransactionParameters)
        {
            var pagedResult = await _serviceManager.TransactionService.GetBankAccountWithdrawalsAsync(accountId, accountTransactionParameters, trackChanges: false);
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));
            return pagedResult.Item1.StatusCode == HttpStatusCode.OK ? Ok(pagedResult.Item1) : StatusCode((int)pagedResult.Item1.StatusCode, pagedResult.Item1);
        }

        [HttpGet("withdrawals/{id:guid}")]
        public async Task<IActionResult> GetBankWithdrawalForAccount(Guid accountId, Guid id)
        {
            var withdrawal = await _serviceManager.TransactionService.GetBankAccountWithdrawalAsync(accountId, id, trackChanges: false);
            return withdrawal.StatusCode == HttpStatusCode.OK ? Ok(withdrawal) : StatusCode((int)withdrawal.StatusCode, withdrawal);
        }

        [HttpPost("withdrawals")]
        public async Task<IActionResult> CreateBankWithdrawal(CreateBankAccountWithdrawal bankAccountWithdrawal)
        {
            var withdrawal = await _serviceManager.TransactionService.CreateBankAccountWithdrawalAsync(bankAccountWithdrawal, trackChanges: true);
            return withdrawal.StatusCode == HttpStatusCode.Created ? Ok(withdrawal) : StatusCode((int)withdrawal.StatusCode, withdrawal);
        }


        [HttpGet("transfers")]
        public async Task<IActionResult> GetBankTransfersForAccount(Guid accountId, [FromQuery] AccountTransactionParameters accountTransactionParameters)
        {
            var pagedResult = await _serviceManager.TransactionService.GetBankAccountTransfersAsync(accountId, accountTransactionParameters, trackChanges: false);
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));
            return pagedResult.Item1.StatusCode == HttpStatusCode.OK ? Ok(pagedResult.Item1) : StatusCode((int)pagedResult.Item1.StatusCode, pagedResult.Item1);
        }

        [HttpGet("transfers/{id:guid}")]
        public async Task<IActionResult> GetBankTransferForAccount(Guid accountId, Guid id)
        {
            var transfer = await _serviceManager.TransactionService.GetBankAccountTransferAsync(accountId, id, trackChanges: false);
            return transfer.StatusCode == HttpStatusCode.OK ? Ok(transfer) : StatusCode((int)transfer.StatusCode, transfer);
        }

        [HttpPost("transfers")]
        public async Task<IActionResult> CreateBankTransfer(CreateBankAccountTransfer bankAccountTransfer)
        {
            var transfer = await _serviceManager.TransactionService.CreateBankAccountTransferAsync(bankAccountTransfer, trackChanges: true);
            return transfer.StatusCode == HttpStatusCode.Created ? Ok(transfer) : StatusCode((int)transfer.StatusCode, transfer);
        }
    }
}
