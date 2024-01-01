using OpenBanking_API_Service.Data;
using OpenBanking_API_Service.Dtos.AccountsDto;
using OpenBanking_API_Service.Service.Interface;
using OpenBanking_API_Service_Common.Library.Entities.Account;
using OpenBanking_API_Service_Common.Library.Models;
using System.Net;
using System.Security.Claims;

namespace OpenBanking_API_Service.Service.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _dbContext;
        public AccountService(IHttpContextAccessor httpContextAccessor,
                              ApplicationDbContext dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }
        public async Task<APIResponse<CreateAccountResponse>> CreateBankAccount(CreateBankAccountDto createBankAccountDto)
        {
            //string currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                string currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var bankAccount = new BankAccount
                {
                    Firstname = createBankAccountDto.Firstname,
                    Lastname = createBankAccountDto.Lastname,
                    Middlename = createBankAccountDto.Middlename,
                    Gender = createBankAccountDto.Gender,
                    MaritalStatus = createBankAccountDto.MaritalStatus,
                    Pin = createBankAccountDto.Pin,
                    Address = createBankAccountDto.Address,
                    StateOfOrigin = createBankAccountDto.StateOfOrigin,
                    DateOfBirth = createBankAccountDto.DateOfBirth,
                    AccountNumber = Generate11DigitAccountNumber(),
                    AccountOpeningDate = DateTimeOffset.UtcNow,
                    AccountBalance = 0.0,
                    UserId = currentUserId

                };

                await _dbContext.BankAccounts.AddAsync(bankAccount);
                await _dbContext.SaveChangesAsync();

                var bankAccountResponse = new CreateAccountResponse
                {
                    BankAccountId = bankAccount.BankAccountId,
                    FirstName = bankAccount.Firstname,
                    LastName = bankAccount.Lastname,
                    MiddleName = bankAccount?.Middlename,
                    Gender = bankAccount.Gender,
                    MaritalStatus = bankAccount.MaritalStatus,
                    Address = bankAccount.Address,
                    StateOfOrigin = bankAccount.StateOfOrigin,
                    DateOfBirth = bankAccount.DateOfBirth,
                    AccountNumber = bankAccount.AccountNumber,
                    AccountBalance = bankAccount.AccountBalance,
                    Pin = bankAccount.Pin,
                    AccountOpeningDate = bankAccount.AccountOpeningDate,
                    UserId = bankAccount.UserId
                };

                return new APIResponse<CreateAccountResponse>(HttpStatusCode.OK, "Account created successfully", bankAccountResponse);
            }
            return new APIResponse<CreateAccountResponse>(HttpStatusCode.Unauthorized, "User not authenticated", null);



        }

        public async Task<APIResponse<CreateBankDepositResponse>> BankAccountDeposit(string accountNumber, CreateBankAccountDepositDto bankAccountDepositDto)
        {
            try
            {
                var bankAccount = _dbContext.BankAccounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
                if (bankAccount != null)
                {
                    bankAccount.AccountBalance += bankAccountDepositDto.Amount;

                    var depositTransaction = new BankDeposit
                    {
                        BankAccountId = bankAccount.BankAccountId,
                        Amount = bankAccountDepositDto.Amount,
                        TransactionDate = DateTimeOffset.UtcNow
                    };

                    await _dbContext.SaveChangesAsync();
                    var depositResponse = new CreateBankDepositResponse
                    {
                        AccountNumber = bankAccount.AccountNumber,
                        Deposit = bankAccountDepositDto.Amount,
                        Balance = bankAccount.AccountBalance,
                        TransactionDate = depositTransaction.TransactionDate

                    };

                    return new APIResponse<CreateBankDepositResponse>(HttpStatusCode.OK, "Funds deposited successfully.", depositResponse);
                }
                return new APIResponse<CreateBankDepositResponse>(HttpStatusCode.NotFound, "Account with the provided account number does not exist", null);
            }
            catch (Exception ex)
            {

                return new APIResponse<CreateBankDepositResponse>(HttpStatusCode.BadRequest, "Transaction unsuccessful", null);
            }
        }

        #region PrivateMethods
        private static string Generate11DigitAccountNumber()
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
