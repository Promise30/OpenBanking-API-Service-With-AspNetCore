using AutoMapper;
using OpenBanking_API_Service.Domain.Entities.Account;
using OpenBanking_API_Service.Dtos.AccountsDto.Requests;
using OpenBanking_API_Service.Dtos.AccountsDto.Responses;
using OpenBanking_API_Service.Infrastructures.Interface;
using OpenBanking_API_Service.Service.Interface;
using System.Net;
using System.Security.Claims;

namespace OpenBanking_API_Service.Service.Implementation
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<BankAccountService> _logger;
        private readonly IMapper _mapper;
        public BankAccountService(IHttpContextAccessor httpContextAccessor,
                                    IRepositoryManager repositoryManager,
                                    ILogger<BankAccountService> logger,
                                    IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _repositoryManager = repositoryManager;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<APIResponse<IEnumerable<BankAccountDto>>> GetAllBankAccountsAsync(bool trackChanges)
        {
            try
            {
                var accounts = await _repositoryManager.Account.GetAllAccountsAsync(trackChanges);

                var accountsDto = _mapper.Map<IEnumerable<BankAccountDto>>(accounts);

                return APIResponse<IEnumerable<BankAccountDto>>.Create(HttpStatusCode.OK, "Request successful", accountsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetAllBankAccountsAsync)} service method {ex}");
                return APIResponse<IEnumerable<BankAccountDto>>.Create(HttpStatusCode.BadRequest, "Request unsuccessful", null);
            }
        }

        public async Task<APIResponse<BankAccountDto>> GetBankAccountAsync(Guid accountId, bool trackChanges)
        {
            try
            {
                var bankAccount = await _repositoryManager.Account.GetBankAccountAsync(accountId, trackChanges);
                _logger.LogInformation($"Account corresponding to id {accountId} successfully retrieved.");
                if (bankAccount != null)
                {
                    var accountDto = _mapper.Map<BankAccountDto>(bankAccount);
                    return APIResponse<BankAccountDto>.Create(HttpStatusCode.OK, "Request successful", accountDto);
                }
                return APIResponse<BankAccountDto>.Create(HttpStatusCode.NotFound, "Bank Account does not exist.", null);

            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong in the {nameof(GetBankAccountAsync)} service method {ex}");
                return APIResponse<BankAccountDto>.Create(HttpStatusCode.InternalServerError, "Internal Server error", null);
            }

        }


        public async Task<APIResponse<BankAccountDto>> CreateBankAccountAsync(CreateBankAccount createBankAccountDto)
        {
            try
            {
                if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    return APIResponse<BankAccountDto>.Create(HttpStatusCode.Unauthorized, "User not authenticated", null);

                }
                var isAccountNumberExist = true;
                var accountNumber = string.Empty;

                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                do
                {
                    accountNumber = Generate11DigitAccountNumber();
                    isAccountNumberExist = _repositoryManager.Account.AccountExists(accountNumber, trackChanges: false);
                }
                while (isAccountNumberExist);

                var bankAccount = new BankAccount
                {
                    UserId = userId,
                    FirstName = createBankAccountDto.FirstName,
                    LastName = createBankAccountDto.LastName,
                    MiddleName = createBankAccountDto?.MiddleName,
                    Gender = createBankAccountDto.Gender,
                    MaritalStatus = createBankAccountDto.MaritalStatus,
                    ResidentCountry = createBankAccountDto.ResidentCountry,
                    ResidentAddress = createBankAccountDto.ResidentAddress,
                    ResidentPostalCode = createBankAccountDto.ResidentPostalCode,
                    City = createBankAccountDto.City,
                    State = createBankAccountDto.State,
                    BirthCountry = createBankAccountDto.BirthCountry,
                    DateOfBirth = createBankAccountDto.DateOfBirth,
                    AccountNumber = accountNumber,
                    Pin = createBankAccountDto.Pin,
                };
                _repositoryManager.Account.CreateBankAccount(bankAccount);
                await _repositoryManager.SaveAsync();

                var accountToReturn = _mapper.Map<BankAccountDto>(bankAccount);
                return APIResponse<BankAccountDto>.Create(HttpStatusCode.Created, "Account created successfully", accountToReturn);
            }

            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(CreateBankAccountAsync)} service method {ex}");
                return APIResponse<BankAccountDto>.Create(HttpStatusCode.InternalServerError, "Internal Server error", null);

            }
        }






        //public async Task<APIResponse<TransactionHistoryDto>> BankAccountTransactionHistory(Guid accountId)
        //{
        //    var bankAccount = _dbContext.BankAccounts
        //                                    .Include(a => a.BankTransfers)
        //                                    .Include(a => a.BankWithdrawals)
        //                                    .Include(a => a.BankDeposits)
        //                                    .FirstOrDefault(a => a.BankAccountId == accountId);

        //    if (bankAccount == null)
        //    {
        //        return APIResponse<TransactionHistoryDto>.Create(HttpStatusCode.NotFound, $"Account not found.", null);
        //    }

        //    var transactionHistory = new TransactionHistoryDto
        //    {
        //        BankTransfers = bankAccount.BankTransfers.Select(transfer => new BankAccountTransferResponse
        //        {
        //            SourceAccount = transfer.SourceAccount,
        //            Amount = transfer.Amount,
        //            Balance = transfer.AccountBalance,
        //            Narration = transfer.Narration,
        //            DestinationAccount = transfer.DestinationAccount,
        //            TransactionDate = transfer.TransactionDate
        //        }),
        //        BankWithdrawals = bankAccount.BankWithdrawals.Select(withdrawal => new BankAccountWithdrawalResponse
        //        {
        //            AccountNumber = withdrawal.AccountNumber,
        //            DebitAmount = withdrawal.Amount,
        //            Balance = withdrawal.AccountBalance,
        //            TransactionDate = withdrawal.TransactionDate
        //        }),
        //        BankDeposits = bankAccount.BankDeposits.Select(deposit => new BankAccountDepositResponse
        //        {
        //            AccountNumber = deposit.AccountNumber,
        //            Deposit = deposit.Amount,
        //            Balance = deposit.AccountBalance,
        //            TransactionDate = deposit.TransactionDate
        //        })
        //    };

        //    return APIResponse<TransactionHistoryDto>.Create(HttpStatusCode.OK, "Bank Transactions History", transactionHistory);
        //}



        #region PrivateMethods
        private string Generate11DigitAccountNumber()
        {
            // You can customize the logic for generating the 11-digit value based on your requirements.
            Random random = new Random();

            // Ensure the first two digits are 11
            long firstTwoDigits = 11;
            long restOfValue = random.Next(100000000, 999999999);

            // Concatenate the first two digits with the rest of the value
            long generatedLongValue = (firstTwoDigits * 1000000000) + restOfValue;

            // Convert the long value to string before returning
            return generatedLongValue.ToString();
        }

        #endregion
    }
}
