using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OpenBanking_API_Service.Dtos.AccountsDto.Requests;
using OpenBanking_API_Service.Dtos.AccountsDto.Responses;
using OpenBanking_API_Service.RequestFeatures;
using OpenBanking_API_Service.Routes;
using OpenBanking_API_Service.Service.Interface;
using System.Net;
using System.Text.Json;

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
        /// <summary>
        /// Gets the list of all registered accounts
        /// </summary>
        /// <param name="accountParameters"></param>
        /// <returns>The registered accounts list</returns>
        /// <response code="200">Returns the list of all accounts</response>
        /// <response code="400">If something went wrong</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BankAccountDto>), 200, "application/json")]
        [ProducesResponseType(400)]
        //[Authorize]
        public async Task<IActionResult> GetBankAccounts([FromQuery] AccountParameters accountParameters)
        {
            var pagedResult = await _accountService.GetAllBankAccountsAsync(accountParameters, trackChanges: false);

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));
            return pagedResult.accounts.StatusCode == HttpStatusCode.OK ? Ok(pagedResult.accounts) : StatusCode((int)pagedResult.accounts.StatusCode, pagedResult.accounts);

        }

        /// <summary>
        /// Gets a single bank account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>The single bank account</returns>
        /// <response code="200">Returns the bank account</response>
        /// <response code="400">If bank account does not exist</response>
        /// <response code="500">If there was a server error</response>     
        [ProducesResponseType(typeof(BankAccountDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpGet("{accountId:guid}")]
        public async Task<IActionResult> GetBankAccount(Guid accountId)
        {
            var account = await _accountService.GetBankAccountAsync(accountId, trackChanges: false);
            //return account.StatusCode == HttpStatusCode.OK ? Ok(account) : BadRequest(account);
            return account.StatusCode == HttpStatusCode.OK ? Ok(account) : StatusCode((int)account.StatusCode, account);
        }


        /// <summary>
        /// creates a new bank account
        /// </summary>
        /// <param name="createBankAccountDto"></param>
        /// <returns>The newly created account</returns>
        /// <remarks>
        /// Sample request:
        ///     
        ///         POST /create-account
        ///         {
        ///             "firstName": "Clark",
        ///             "lastName": "Kent",
        ///             "middleName": "Claire",
        ///             "dateOfBirth": "2000-01-11T12:45:57.671Z",
        ///             "gender": "Female",
        ///             "residentCountry": "Nigeria",
        ///             "residentAddress": "Ikeja, Lagos",
        ///             "residentPostalCode": "100213",
        ///             "city": "Lagos",
        ///             "state": "Lagos",
        ///             "birthCountry": "Niger",
        ///             "maritalStatus": 1,
        ///             "accountType": 1,
        ///             "pin": 1234
        ///         }
        /// </remarks>
        /// <response code="201">Returns the newly created bank account</response>
        /// <response code="400">If the model is invalid</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="500">If there was a server error</response>     
        [ProducesResponseType(typeof(BankAccountDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [Authorize]
        [HttpPost]
        [Route(AccountRoutes.CreateAccount)]
        public async Task<IActionResult> CreateBankAccount(CreateBankAccount createBankAccountDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(APIResponse<ModelStateDictionary>.Create(HttpStatusCode.BadRequest, ModelState, "Validation Failed"));
            }
            var response = await _accountService.CreateBankAccountAsync(createBankAccountDto);
            return response.StatusCode == HttpStatusCode.OK ? Ok(response) : StatusCode((int)response.StatusCode, response);
        }
        /// <summary>
        /// Update user account details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns>Returns no content</returns>
        [HttpPatch("update-account/{id:guid}")]
        [Authorize]
        public async Task<IActionResult> PartiallyUpdateBankAccountDetail(Guid id, [FromBody] JsonPatchDocument<BankAccountForUpdateDto> patchDoc)
        {
            if (patchDoc is null)
            {
                return BadRequest(APIResponse<ModelStateDictionary>.Create(HttpStatusCode.BadRequest, ModelState, "Validation Failed"));
            }
            var result = await _accountService.GetBankAccountForPatch(id, trackChanges: true);
            patchDoc.ApplyTo(result.bankAccountToPatch);
            _accountService.SaveChangesForPatch(result.bankAccountToPatch, result.bankAccountEntity);

            return NoContent();

        }

        /// <summary>
        /// Delete a bank account
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns no content</returns>
        /// <response code="204">Account was deleted successful</response>
        /// <response code="404">If bank account does not exist</response>
        /// <response code="500">If there was a server error</response>
        [HttpDelete("delete-account/{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteBankAccount(Guid id)
        {
            var result = await _accountService.DeleteBankAccountAsync(id, trackChanges: false);
            return result.StatusCode == HttpStatusCode.OK ? Ok(result) : StatusCode((int)result.StatusCode, result);
        }

    }
}
