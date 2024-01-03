using Microsoft.EntityFrameworkCore;
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
                        AccountId = bankAccount.BankAccountId,
                        Amount = bankAccountDepositDto.Amount,
                        Balance = bankAccount.AccountBalance,
                        AccountNumber = accountNumber,
                        TransactionDate = DateTimeOffset.UtcNow
                    };
                    await _dbContext.BankDeposits.AddAsync(depositTransaction);
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
                return new APIResponse<CreateBankDepositResponse>(HttpStatusCode.NotFound, "Account with the provided account number does not exist.", null);
            }
            catch (Exception ex)
            {
                return new APIResponse<CreateBankDepositResponse>(HttpStatusCode.BadRequest, "Transaction unsuccessful.", null);
            }
        }
        public async Task<APIResponse<AccountWithdrawalResponse>> BankAccountWithdrawal(CreateBankAccountWithdrawal bankAccountWithdrawal)
        {
            string currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Retrieve the user's bank account
            var bankAccount = _dbContext.BankAccounts.SingleOrDefault(b => b.UserId == currentUserId);
            if (bankAccount != null)
            {
                // check if the pin matches
                if (bankAccount.Pin == bankAccountWithdrawal.Pin)
                {
                    if (bankAccount.AccountBalance > bankAccountWithdrawal.Amount)
                    {
                        bankAccount.AccountBalance -= bankAccountWithdrawal.Amount;
                        var withDrawalTransaction = new BankWithdrawal
                        {
                            Amount = bankAccountWithdrawal.Amount,
                            AccountId = bankAccount.BankAccountId,
                            AccountNumber = bankAccount.AccountNumber,
                            TransactionDate = DateTimeOffset.UtcNow,
                            Balance = bankAccount.AccountBalance

                        };
                        await _dbContext.BankWithdrawals.AddAsync(withDrawalTransaction);
                        await _dbContext.SaveChangesAsync();
                        var withDrawalResponse = new AccountWithdrawalResponse
                        {
                            AccountNumber = bankAccount.AccountNumber,
                            DebitAmount = bankAccountWithdrawal.Amount,
                            Balance = bankAccount.AccountBalance,
                            TransactionDate = withDrawalTransaction.TransactionDate
                        };
                        return new APIResponse<AccountWithdrawalResponse>(HttpStatusCode.OK, "Amount has been withdrawn successfully.", withDrawalResponse);
                    }
                    return new APIResponse<AccountWithdrawalResponse>(HttpStatusCode.BadRequest, "Insufficient funds for withdrawal operation.", null);
                }
                return new APIResponse<AccountWithdrawalResponse>(HttpStatusCode.BadRequest, "Invalid PIN.", null);
            }
            return new APIResponse<AccountWithdrawalResponse>(HttpStatusCode.NotFound, "Bank account not found.", null);
        }

        public async Task<APIResponse<CreateAccountTransferResponse>> BankAccountTransfer(CreateAccountTransfer bankAccountTransfer)
        {
            string currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // Retrieve the sender's and receiver's account number
            var senderAccount = _dbContext.BankAccounts.SingleOrDefault(a => a.UserId == currentUserId);
            var receiverAccount = _dbContext.BankAccounts.SingleOrDefault(a => a.AccountNumber == bankAccountTransfer.DestinationAccount);

            if (senderAccount == null)
                return APIResponse<CreateAccountTransferResponse>.Create(HttpStatusCode.NotFound, "Sender's bank account not found.", null);

            // Check if the receiver's account is not found
            if (receiverAccount == null)
                return APIResponse<CreateAccountTransferResponse>.Create(HttpStatusCode.NotFound, "Receiver's bank account not found.", null);


            // Check if the sender has insufficient funds
            if (senderAccount.AccountBalance < bankAccountTransfer.Amount)
                return APIResponse<CreateAccountTransferResponse>.Create(HttpStatusCode.BadRequest, "Insufficient funds for transfer.", null);

            // Update the sender's and receiver's account balances
            senderAccount.AccountBalance -= bankAccountTransfer.Amount;
            receiverAccount.AccountBalance += bankAccountTransfer.Amount;
            // Create a transfer transaction record
            var transferTransaction = new BankTransfer
            {
                Amount = bankAccountTransfer.Amount,
                TransactionDate = DateTimeOffset.UtcNow,
                SourceAccount = senderAccount.AccountNumber,
                Balance = senderAccount.AccountBalance,
                DestinationAccount = bankAccountTransfer.DestinationAccount,
                Narration = bankAccountTransfer.Narration,
                AccountId = senderAccount.BankAccountId

            };
            await _dbContext.BankTransfers.AddAsync(transferTransaction);
            await _dbContext.SaveChangesAsync();

            var transferResponse = new CreateAccountTransferResponse
            {
                SourceAccount = bankAccountTransfer.SourceAccount,
                DestinationAccount = bankAccountTransfer.DestinationAccount,
                Balance = transferTransaction.Balance,
                Amount = bankAccountTransfer.Amount,
                Narration = bankAccountTransfer.Narration,
                TransactionDate = DateTimeOffset.UtcNow,
            };
            return APIResponse<CreateAccountTransferResponse>.Create(HttpStatusCode.OK, "Transfer successful.", transferResponse);
        }
        public async Task<APIResponse<TransactionHistoryDto>> BankAccountTransactionHistory(Guid accountId)
        {
            var bankAccount = _dbContext.BankAccounts
                                            .Include(a => a.BankTransfers)
                                            .Include(a => a.BankWithdrawals)
                                            .Include(a => a.BankDeposits)
                                            .FirstOrDefault(a => a.BankAccountId == accountId);

            if (bankAccount == null)
            {
                return APIResponse<TransactionHistoryDto>.Create(HttpStatusCode.NotFound, $"Account not found.", null);
            }

            var transactionHistory = new TransactionHistoryDto
            {
                BankTransfers = bankAccount.BankTransfers.Select(transfer => new CreateAccountTransferResponse
                {
                    SourceAccount = transfer.SourceAccount,
                    Amount = transfer.Amount,
                    Balance = transfer.Balance,
                    Narration = transfer.Narration,
                    DestinationAccount = transfer.DestinationAccount,
                    TransactionDate = transfer.TransactionDate
                }),
                BankWithdrawals = bankAccount.BankWithdrawals.Select(withdrawal => new AccountWithdrawalResponse
                {
                    AccountNumber = withdrawal.AccountNumber,
                    DebitAmount = withdrawal.Amount,
                    Balance = withdrawal.Balance,
                    TransactionDate = withdrawal.TransactionDate
                }),
                BankDeposits = bankAccount.BankDeposits.Select(deposit => new CreateBankDepositResponse
                {
                    AccountNumber = deposit.AccountNumber,
                    Deposit = deposit.Amount,
                    Balance = deposit.Balance,
                    TransactionDate = deposit.TransactionDate
                })
            };

            return APIResponse<TransactionHistoryDto>.Create(HttpStatusCode.OK, "Bank Transactions History", transactionHistory);
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
