using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.RateLimiting;
using OpenBanking_API_Service.Dtos.AccountsDto.Requests;
using OpenBanking_API_Service.Dtos.AccountsDto.Responses;
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

        /// <summary>
        /// get all deposits for a single account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>A list of a bank account deposits</returns>
        /// <response code="200">List of bank account deposit</response>
        /// <response code="404">If bank account id does not exist</response>
        /// <response code="500">If there is a server error</response>
        //[HttpGet("{accountId:guid}/deposits")]
        [ProducesResponseType(typeof(IEnumerable<BankAccountDepositResponse>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpGet("deposits")]
        [OutputCache(Duration = 60)]
        [EnableRateLimiting("SpecificPolicy")]
        public async Task<IActionResult> GetBankDepositsForAccount(Guid accountId, [FromQuery] AccountTransactionParameters accountTransactionParameters)
        {
            var pagedResult = await _serviceManager.TransactionService.GetBankAccountDepositsAsync(accountId, accountTransactionParameters, trackChanges: false);
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));
            var etag = $"\"{Guid.NewGuid():n}\"";
            HttpContext.Response.Headers.ETag = etag;
            return pagedResult.Item1.StatusCode == HttpStatusCode.OK ? Ok(pagedResult.Item1) : StatusCode((int)pagedResult.Item1.StatusCode, pagedResult.Item1);
        }
        /// <summary>
        /// gets a single deposit for a single bank account
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="id"></param>
        /// <returns>A single bank account deposit</returns>
        /// <response code="200">Bank account deposit was found</response>
        /// <response code="404">If bank account does not exist</response>
        /// <response code="500">If something went wrong with the server</response>
        [HttpGet("deposits/{id:guid}", Name = "GetBankDepositForAccount")]
        [ProducesResponseType(typeof(BankAccountDepositResponse), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetBankDepositForAccount(Guid accountId, Guid id)
        {
            var deposit = await _serviceManager.TransactionService.GetBankAccountDepositAsync(accountId, id, trackChanges: false);
            return deposit.StatusCode == HttpStatusCode.OK ? Ok(deposit) : StatusCode((int)deposit.StatusCode, deposit);
        }
        /// <summary>
        /// create a new bank deposit for a bank account
        /// </summary>
        /// <param name="bankAccountDeposit"></param>
        /// <returns>A newly created bank account deposit</returns>
        /// <response code="201">Bank account deposit was successful</response>
        /// <response code="400">If model is invalid</response>
        /// <response code="404">If bank account does not exist</response>
        /// <response code="500">If something went wrong with the server</response>
        [ProducesResponseType(typeof(BankAccountDepositResponse), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpPost("deposits")]
        public async Task<IActionResult> CreateBankDeposit(CreateBankAccountDeposit bankAccountDeposit)
        {
            var deposit = await _serviceManager.TransactionService.CreateBankAccountDepositAsync(bankAccountDeposit, trackChanges: true);
            return deposit.StatusCode == HttpStatusCode.Created ? CreatedAtRoute("GetBankDepositForAccount", new { id = deposit.Data.Id, accountId = deposit.Data.AccountId }, deposit) : StatusCode((int)deposit.StatusCode, deposit);
        }

        /// <summary>
        /// gets a list of all withdrawals for a bank account
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="accountTransactionParameters"></param>
        /// <returns>A list of all bank account withdrawals</returns>
        /// <response code="200">List of bank account withdrawals</response>
        /// <response code="404">If bank account id does not exist</response>
        /// <response code="500">If there is a server error</response>
        [HttpGet("withdrawals")]
        [ProducesResponseType(typeof(IEnumerable<BankAccountWithdrawalResponse>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [OutputCache(Duration = 60)]
        //[OutputCache(PolicyName = "120SecondsDuration")]
        public async Task<IActionResult> GetBankWithdrawalsForAccount(Guid accountId, [FromQuery] AccountTransactionParameters accountTransactionParameters)
        {
            var pagedResult = await _serviceManager.TransactionService.GetBankAccountWithdrawalsAsync(accountId, accountTransactionParameters, trackChanges: false);
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));
            return pagedResult.Item1.StatusCode == HttpStatusCode.OK ? Ok(pagedResult.Item1) : StatusCode((int)pagedResult.Item1.StatusCode, pagedResult.Item1);
        }
        /// <summary>
        /// gets a single withdrawal detail for a bank account
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="id"></param>
        /// <returns>A single bank account withdrawal</returns>
        /// <response code="200">Bank account withdrawal was found</response>
        /// <response code="404">If bank account does not exist</response>
        /// <response code="500">If something went wrong with the server</response>
        [HttpGet("withdrawals/{id:guid}")]
        [ProducesResponseType(typeof(BankAccountWithdrawalResponse), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetBankWithdrawalForAccount(Guid accountId, Guid id)
        {
            var withdrawal = await _serviceManager.TransactionService.GetBankAccountWithdrawalAsync(accountId, id, trackChanges: false);
            return withdrawal.StatusCode == HttpStatusCode.OK ? Ok(withdrawal) : StatusCode((int)withdrawal.StatusCode, withdrawal);
        }

        /// <summary>
        /// create a bank withdrawal for a bank account
        /// </summary>
        /// <param name="bankAccountWithdrawal"></param>
        /// <returns>A newly created bank account withdrawal</returns>
        /// <response code="201">Bank account withdrawal was successful</response>
        /// <response code="400">If model is invalid</response>
        /// <response code="404">If bank account does not exist</response>
        /// <response code="500">If something went wrong with the server</response>
        [HttpPost("withdrawals")]
        //[AutoValidateAntiforgeryToken]
        [ProducesResponseType(typeof(BankAccountWithdrawalResponse), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateBankWithdrawal(CreateBankAccountWithdrawal bankAccountWithdrawal)
        {
            var withdrawal = await _serviceManager.TransactionService.CreateBankAccountWithdrawalAsync(bankAccountWithdrawal, trackChanges: true);
            return withdrawal.StatusCode == HttpStatusCode.Created ? Ok(withdrawal) : StatusCode((int)withdrawal.StatusCode, withdrawal);
        }

        /// <summary>
        /// gets a list of all bank transfers for a single bank account
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="accountTransactionParameters"></param>
        /// <returns>A list of all bank account transfers</returns>
        /// <response code="200">List of bank account transfers</response>
        /// <response code="404">If bank account id does not exist</response>
        /// <response code="500">If there is a server error</response>
        [HttpGet("transfers")]
        [ProducesResponseType(typeof(IEnumerable<BankAccountTransferResponse>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [OutputCache(PolicyName = "120SecondsDuration")]
        public async Task<IActionResult> GetBankTransfersForAccount(Guid accountId, [FromQuery] AccountTransactionParameters accountTransactionParameters)
        {
            var pagedResult = await _serviceManager.TransactionService.GetBankAccountTransfersAsync(accountId, accountTransactionParameters, trackChanges: false);
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));
            return pagedResult.Item1.StatusCode == HttpStatusCode.OK ? Ok(pagedResult.Item1) : StatusCode((int)pagedResult.Item1.StatusCode, pagedResult.Item1);
        }

        /// <summary>
        /// gets a single bank transfer detail for a bank account
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="id"></param>
        /// <returns>A single bank account transfer detail</returns>
        /// <response code="200">Bank account transfer was found</response>
        /// <response code="404">If bank account does not exist</response>
        /// <response code="500">If something went wrong with the server</response>
        [HttpGet("transfers/{id:guid}")]
        [ProducesResponseType(typeof(BankAccountTransferResponse), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetBankTransferForAccount(Guid accountId, Guid id)
        {
            var transfer = await _serviceManager.TransactionService.GetBankAccountTransferAsync(accountId, id, trackChanges: false);
            return transfer.StatusCode == HttpStatusCode.OK ? Ok(transfer) : StatusCode((int)transfer.StatusCode, transfer);
        }

        /// <summary>
        /// create a new bank transfer for a bank account
        /// </summary>
        /// <param name="bankAccountTransfer"></param>
        /// <returns>A newly created bank account transfer</returns>
        /// <remarks>
        /// Sample request:
        ///     
        ///     POST  /transfers
        ///     {
        ///         "sourceAccount": "12345678901",
        ///         "amount": 1000,
        ///         "narration": "Test transfer",
        ///         "destinationAccount": "1234567890",
        ///         "pin": 1112
        ///     }
        /// </remarks>

        /// <response code="201">Bank account transfer was successful</response>
        /// <response code="400">If model is invalid</response>
        /// <response code="404">If bank account does not exist</response>
        /// <response code="500">If something went wrong with the server</response>
        [HttpPost("transfers")]
        [ProducesResponseType(typeof(BankAccountTransferResponse), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateBankTransfer(CreateBankAccountTransfer bankAccountTransfer)
        {
            var transfer = await _serviceManager.TransactionService.CreateBankAccountTransferAsync(bankAccountTransfer, trackChanges: true);
            return transfer.StatusCode == HttpStatusCode.Created ? Ok(transfer) : StatusCode((int)transfer.StatusCode, transfer);
        }
    }
}
