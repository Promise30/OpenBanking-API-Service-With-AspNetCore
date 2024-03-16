using AutoMapper;
using OpenBanking_API_Service.Domain.Entities.Account;
using OpenBanking_API_Service.Dtos.AccountsDto.Requests;
using OpenBanking_API_Service.Dtos.AccountsDto.Responses;
using OpenBanking_API_Service.Infrastructures.Interface;
using OpenBanking_API_Service.RequestFeatures;
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
        public async Task<(APIResponse<IEnumerable<BankAccountDto>> accounts, MetaData metaData)> GetAllBankAccountsAsync(AccountParameters accountParameters, bool trackChanges)
        {
            try
            {
                if (!accountParameters.ValidAmountRange)
                {
                    throw new Exception("Max amount cannot be less than minimum amount.");
                }
                var accountsWithMetaData = await _repositoryManager.Account.GetAllAccountsAsync(accountParameters, trackChanges);

                var accountsDto = _mapper.Map<IEnumerable<BankAccountDto>>(accountsWithMetaData);

                return (accounts: APIResponse<IEnumerable<BankAccountDto>>.Create(HttpStatusCode.OK, accountsDto, null), metaData: accountsWithMetaData.MetaData);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetAllBankAccountsAsync)} service method {ex}");
                return (APIResponse<IEnumerable<BankAccountDto>>.Create(HttpStatusCode.BadRequest, null, "Request unsuccessful"), metaData: null);
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
                    return APIResponse<BankAccountDto>.Create(HttpStatusCode.OK, accountDto, null);
                }
                return APIResponse<BankAccountDto>.Create(HttpStatusCode.NotFound, null, "Bank Account does not exist.");

            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong in the {nameof(GetBankAccountAsync)} service method {ex}");
                return APIResponse<BankAccountDto>.Create(HttpStatusCode.InternalServerError, null, "Internal Server error");
            }

        }


        public async Task<APIResponse<BankAccountDto>> CreateBankAccountAsync(CreateBankAccount createBankAccountDto)
        {
            try
            {
                if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    return APIResponse<BankAccountDto>.Create(HttpStatusCode.Unauthorized, null, "User not authenticated");

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
                    AccountType = createBankAccountDto.AccountType,
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
                return APIResponse<BankAccountDto>.Create(HttpStatusCode.Created, accountToReturn, null);
            }

            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(CreateBankAccountAsync)} service method {ex}");
                return APIResponse<BankAccountDto>.Create(HttpStatusCode.InternalServerError, null, "Internal Server error");

            }
        }

        public async Task<APIResponse<object>> DeleteBankAccountAsync(Guid accountId, bool trackChanges)
        {
            try
            {
                var account = await _repositoryManager.Account.GetBankAccountAsync(accountId, trackChanges);
                if (account == null)
                {
                    return APIResponse<object>.Create(HttpStatusCode.NotFound, "Account does not exist", null);
                }
                _repositoryManager.Account.DeleteBankAccount(account);
                await _repositoryManager.SaveAsync();
                return APIResponse<object>.Create(HttpStatusCode.OK, "Operation successful", null);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong in the {nameof(DeleteBankAccountAsync)} service method {ex}");
                return APIResponse<object>.Create(HttpStatusCode.InternalServerError, "Internal Server error", null);
            }

        }

        public async Task<(BankAccountForUpdateDto bankAccountToPatch, BankAccount bankAccountEntity)> GetBankAccountForPatch(Guid accountId, bool trackChanges)
        {
            try
            {
                var account = await _repositoryManager.Account.GetBankAccountAsync(accountId, trackChanges);
                if (account is null)
                {
                    throw new BadHttpRequestException("Account does not exist", 404);
                }
                var bankAccountToPatch = _mapper.Map<BankAccountForUpdateDto>(account);
                return (bankAccountToPatch, account);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong in the {nameof(GetBankAccountForPatch)} service method {ex}");
                throw;
            }

        }

        public void SaveChangesForPatch(BankAccountForUpdateDto bankAccountToPatch, BankAccount bankAccount)
        {
            _mapper.Map(bankAccountToPatch, bankAccount);
            _repositoryManager.SaveAsync();
        }




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
        private int CalculateAge(DateTime dateOfBirth)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - dateOfBirth.Year;

            // Adjust age if the birthday hasn't occurred yet this year
            if (dateOfBirth.Date > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }

        #endregion
    }
}
