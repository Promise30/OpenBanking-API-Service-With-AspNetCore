﻿using AutoMapper;
using OpenBanking_API_Service.Domain.Entities.Account;
using OpenBanking_API_Service.Dtos.AccountsDto.Requests;
using OpenBanking_API_Service.Dtos.AccountsDto.Responses;
using OpenBanking_API_Service.Infrastructures.Interface;
using OpenBanking_API_Service.RequestFeatures;
using OpenBanking_API_Service.Service.Interface;
using System.Net;

namespace OpenBanking_API_Service.Service.Implementation
{
    public class TransactionService : ITransactionService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILogger<TransactionService> _logger;
        private readonly IMapper _mapper;
        public TransactionService(IRepositoryManager repositoryManager, ILogger<TransactionService> logger, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<(APIResponse<IEnumerable<BankAccountDepositResponse>>, MetaData metaData)> GetBankAccountDepositsAsync(Guid accountId, AccountTransactionParameters accountTransactionParameters, bool trackChanges)
        {
            try
            {
                var bankAccount = await _repositoryManager.Account.GetBankAccountAsync(accountId, trackChanges);
                if (bankAccount == null)
                {
                    return (APIResponse<IEnumerable<BankAccountDepositResponse>>.Create(HttpStatusCode.NotFound, null, "Bank account does not exist."), metaData: new MetaData());
                }
                if (!accountTransactionParameters.ValidAmountRange)
                {
                    throw new Exception("Max amount cannot be less than min amount");
                }
                var depositsWithMetaData = await _repositoryManager.BankDeposit.GetBankAccountDepositsAsync(accountId, accountTransactionParameters, trackChanges);
                var depositsDto = _mapper.Map<IEnumerable<BankAccountDepositResponse>>(depositsWithMetaData);

                return (APIResponse<IEnumerable<BankAccountDepositResponse>>.Create(HttpStatusCode.OK, depositsDto, null), metaData: depositsWithMetaData.MetaData);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetBankAccountDepositsAsync)} service method {ex}");
                return (APIResponse<IEnumerable<BankAccountDepositResponse>>.Create(HttpStatusCode.InternalServerError, null, "Internal Server error"), metaData: new MetaData());

            }
        }
        public async Task<APIResponse<BankAccountDepositResponse>> GetBankAccountDepositAsync(Guid accountId, Guid id, bool trackChanges)
        {
            try
            {
                var account = await _repositoryManager.Account.GetBankAccountAsync(accountId, trackChanges);
                if (account == null)
                {
                    return APIResponse<BankAccountDepositResponse>.Create(HttpStatusCode.NotFound, null, "Account does not exist");
                }
                var bankDepositDb = await _repositoryManager.BankDeposit.GetBankAccountDepositAsync(accountId, id, trackChanges);
                if (bankDepositDb == null)
                {
                    return APIResponse<BankAccountDepositResponse>.Create(HttpStatusCode.NotFound, null, "Record does not exist");
                }

                var bankDeposit = _mapper.Map<BankAccountDepositResponse>(bankDepositDb);
                return APIResponse<BankAccountDepositResponse>.Create(HttpStatusCode.OK, bankDeposit, null);

            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong in the {nameof(GetBankAccountDepositAsync)} service method {ex}");
                return APIResponse<BankAccountDepositResponse>.Create(HttpStatusCode.InternalServerError, null, "Internal Server error");
            }
        }

        public async Task<APIResponse<BankAccountDepositResponse>> CreateBankAccountDepositAsync(CreateBankAccountDeposit createBankAccountDeposit, bool trackChanges)
        {
            try
            {
                var account = await _repositoryManager.Account.GetBankAccountByAccountNumberAsync(createBankAccountDeposit.AccountNumber, trackChanges: true);
                if (account == null)
                {
                    return APIResponse<BankAccountDepositResponse>.Create(HttpStatusCode.NotFound, null, "Account does not exist.");
                }
                if (account.Pin != createBankAccountDeposit.Pin)
                {
                    return APIResponse<BankAccountDepositResponse>.Create(HttpStatusCode.BadRequest, null, "Invalid Pin");
                }

                account.AccountBalance += createBankAccountDeposit.Amount;
                var deposit = new BankDeposit
                {
                    AccountId = account.BankAccountId,
                    AccountNumber = createBankAccountDeposit.AccountNumber,
                    Amount = createBankAccountDeposit.Amount,
                    TransactionDate = DateTimeOffset.Now,
                    AccountBalance = account.AccountBalance

                };
                _repositoryManager.BankDeposit.CreateBankDeposit(deposit);

                var response = _mapper.Map<BankAccountDepositResponse>(deposit);

                await _repositoryManager.SaveAsync();


                return APIResponse<BankAccountDepositResponse>.Create(HttpStatusCode.Created, response, null);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong in the {nameof(CreateBankAccountDepositAsync)} service method {ex}");
                return APIResponse<BankAccountDepositResponse>.Create(HttpStatusCode.InternalServerError, null, "Internal Server error");
            }
        }


        public async Task<(APIResponse<IEnumerable<BankAccountWithdrawalResponse>>, MetaData metaData)> GetBankAccountWithdrawalsAsync(Guid accountId, AccountTransactionParameters accountTransactionParameters, bool trackChanges)
        {
            try
            {
                var bankAccount = await _repositoryManager.Account.GetBankAccountAsync(accountId, trackChanges);
                if (bankAccount == null)
                {
                    return (APIResponse<IEnumerable<BankAccountWithdrawalResponse>>.Create(HttpStatusCode.NotFound, null, "Bank account does not exist."), metaData: new MetaData());
                }
                if (!accountTransactionParameters.ValidAmountRange)
                {
                    throw new Exception("Max amount cannot be less than min amount");
                }

                var withdrawalsWithMetaData = await _repositoryManager.BankWithdrawal.GetBankAccountWithdrawalsAsync(accountId, accountTransactionParameters, trackChanges);
                var withdrawalsDto = _mapper.Map<IEnumerable<BankAccountWithdrawalResponse>>(withdrawalsWithMetaData);

                return (APIResponse<IEnumerable<BankAccountWithdrawalResponse>>.Create(HttpStatusCode.OK, withdrawalsDto, null), metaData: withdrawalsWithMetaData.MetaData);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetBankAccountWithdrawalsAsync)} service method {ex}");
                return (APIResponse<IEnumerable<BankAccountWithdrawalResponse>>.Create(HttpStatusCode.InternalServerError, null, "Internal Server error"), metaData: new MetaData());

            }
        }

        public async Task<APIResponse<BankAccountWithdrawalResponse>> GetBankAccountWithdrawalAsync(Guid accountId, Guid id, bool trackChanges)
        {
            try
            {
                var account = await _repositoryManager.Account.GetBankAccountAsync(accountId, trackChanges);
                if (account == null)
                {
                    return APIResponse<BankAccountWithdrawalResponse>.Create(HttpStatusCode.NotFound, null, "Account does not exist");
                }
                var bankWithdrawalDb = await _repositoryManager.BankWithdrawal.GetBankAccountWithdrawalAsync(accountId, id, trackChanges);
                if (bankWithdrawalDb == null)
                {
                    return APIResponse<BankAccountWithdrawalResponse>.Create(HttpStatusCode.NotFound, null, "Record does not exist");
                }

                var bankWithdrawal = _mapper.Map<BankAccountWithdrawalResponse>(bankWithdrawalDb);
                return APIResponse<BankAccountWithdrawalResponse>.Create(HttpStatusCode.OK, bankWithdrawal, null);

            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong in the {nameof(GetBankAccountWithdrawalsAsync)} service method {ex}");
                return APIResponse<BankAccountWithdrawalResponse>.Create(HttpStatusCode.InternalServerError, null, "Internal Server error");
            }
        }

        public async Task<APIResponse<BankAccountWithdrawalResponse>> CreateBankAccountWithdrawalAsync(CreateBankAccountWithdrawal createBankAccountWithdrawal, bool trackChanges)
        {
            try
            {
                var account = await _repositoryManager.Account.GetBankAccountByAccountNumberAsync(createBankAccountWithdrawal.AccountNumber, trackChanges: true);
                if (account == null)
                {
                    return APIResponse<BankAccountWithdrawalResponse>.Create(HttpStatusCode.NotFound, null, "Account does not exist.");
                }
                if (account.Pin != createBankAccountWithdrawal.Pin)
                {
                    return APIResponse<BankAccountWithdrawalResponse>.Create(HttpStatusCode.BadRequest, null, "Invalid Pin");
                }
                if (account.AccountBalance < createBankAccountWithdrawal.Amount)
                {
                    return APIResponse<BankAccountWithdrawalResponse>.Create(HttpStatusCode.BadRequest, null, "Insufficient account balance");
                }

                account.AccountBalance -= createBankAccountWithdrawal.Amount;
                var withdrawal = new BankWithdrawal
                {
                    AccountId = account.BankAccountId,
                    AccountNumber = createBankAccountWithdrawal.AccountNumber,
                    Amount = createBankAccountWithdrawal.Amount,
                    AccountBalance = account.AccountBalance,
                    TransactionDate = DateTimeOffset.Now

                };
                _repositoryManager.BankWithdrawal.CreateBankWithdrawal(withdrawal);

                var response = _mapper.Map<BankAccountWithdrawalResponse>(withdrawal);

                await _repositoryManager.SaveAsync();


                return APIResponse<BankAccountWithdrawalResponse>.Create(HttpStatusCode.Created, response, null);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong in the {nameof(CreateBankAccountWithdrawalAsync)} service method {ex}");
                return APIResponse<BankAccountWithdrawalResponse>.Create(HttpStatusCode.InternalServerError, null, "Internal Server error");
            }
        }

        public async Task<(APIResponse<IEnumerable<BankAccountTransferResponse>>, MetaData metaData)> GetBankAccountTransfersAsync(Guid accountId, AccountTransactionParameters accountTransactionParameters, bool trackChanges)
        {
            try
            {
                var bankAccount = await _repositoryManager.Account.GetBankAccountAsync(accountId, trackChanges);
                if (bankAccount == null)
                {
                    return (APIResponse<IEnumerable<BankAccountTransferResponse>>.Create(HttpStatusCode.NotFound, null, "Bank account does not exist."), metaData: new MetaData());
                }
                if (!accountTransactionParameters.ValidAmountRange)
                {
                    throw new Exception("Max amount cannot be less than min amount");
                }
                var transfersWithMetaData = await _repositoryManager.BankTransfer.GetBankAccountTransfersAsync(accountId, accountTransactionParameters, trackChanges);
                var transfersDto = _mapper.Map<IEnumerable<BankAccountTransferResponse>>(transfersWithMetaData);

                return (APIResponse<IEnumerable<BankAccountTransferResponse>>.Create(HttpStatusCode.OK, transfersDto, null), metaData: transfersWithMetaData.MetaData);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetBankAccountTransfersAsync)} service method {ex}");
                return (APIResponse<IEnumerable<BankAccountTransferResponse>>.Create(HttpStatusCode.InternalServerError, null, "Internal Server error"), metaData: new MetaData());

            }
        }

        public async Task<APIResponse<BankAccountTransferResponse>> GetBankAccountTransferAsync(Guid accountId, Guid id, bool trackChanges)
        {
            try
            {
                var account = await _repositoryManager.Account.GetBankAccountAsync(accountId, trackChanges);
                if (account == null)
                {
                    return APIResponse<BankAccountTransferResponse>.Create(HttpStatusCode.NotFound, null, "Account does not exist");
                }
                var bankTransferDb = await _repositoryManager.BankTransfer.GetBankAccountTransferAsync(accountId, id, trackChanges);
                if (bankTransferDb == null)
                {
                    return APIResponse<BankAccountTransferResponse>.Create(HttpStatusCode.NotFound, null, "Record does not exist");
                }

                var bankTransfer = _mapper.Map<BankAccountTransferResponse>(bankTransferDb);
                return APIResponse<BankAccountTransferResponse>.Create(HttpStatusCode.OK, bankTransfer, null);

            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong in the {nameof(GetBankAccountTransferAsync)} service method {ex}");
                return APIResponse<BankAccountTransferResponse>.Create(HttpStatusCode.InternalServerError, null, "Internal Server error");
            }
        }

        public async Task<APIResponse<BankAccountTransferResponse>> CreateBankAccountTransferAsync(CreateBankAccountTransfer createBankAccountTransfer, bool trackChanges)
        {
            try
            {
                var sourceAccount = await _repositoryManager.Account.GetBankAccountByAccountNumberAsync(createBankAccountTransfer.SourceAccount, trackChanges: true);
                var destinationAccount = await _repositoryManager.Account.GetBankAccountByAccountNumberAsync(createBankAccountTransfer.DestinationAccount, trackChanges: true);
                if (sourceAccount == null || destinationAccount == null)
                {
                    return APIResponse<BankAccountTransferResponse>.Create(HttpStatusCode.NotFound, null, "Account does not exist.");
                }
                if (sourceAccount.Pin != createBankAccountTransfer.Pin)
                {
                    return APIResponse<BankAccountTransferResponse>.Create(HttpStatusCode.BadRequest, null, "Invalid Pin");
                }
                if (sourceAccount.AccountBalance < createBankAccountTransfer.Amount)
                {
                    return APIResponse<BankAccountTransferResponse>.Create(HttpStatusCode.BadRequest, null, "Insufficient account balance.");
                }

                sourceAccount.AccountBalance -= createBankAccountTransfer.Amount;
                destinationAccount.AccountBalance += createBankAccountTransfer.Amount;

                var transfer = new BankTransfer
                {
                    AccountId = sourceAccount.BankAccountId,
                    SourceAccount = createBankAccountTransfer.SourceAccount,
                    DestinationAccount = createBankAccountTransfer.DestinationAccount,
                    Amount = createBankAccountTransfer.Amount,
                    Narration = createBankAccountTransfer.Narration,
                    TransactionDate = DateTimeOffset.Now,
                    AccountBalance = sourceAccount.AccountBalance

                };



                var receipientTransfer = new BankTransfer
                {
                    AccountId = destinationAccount.BankAccountId,
                    SourceAccount = createBankAccountTransfer.SourceAccount,
                    DestinationAccount = createBankAccountTransfer.DestinationAccount,
                    Amount = createBankAccountTransfer.Amount,
                    AccountBalance = destinationAccount.AccountBalance,
                    Narration = createBankAccountTransfer.Narration,
                    TransactionDate = DateTimeOffset.Now,
                };
                _repositoryManager.BankTransfer.CreateBankTransfer(transfer);
                _repositoryManager.BankTransfer.CreateBankTransfer(receipientTransfer);

                var response = _mapper.Map<BankAccountTransferResponse>(transfer);


                await _repositoryManager.SaveAsync();


                return APIResponse<BankAccountTransferResponse>.Create(HttpStatusCode.Created, response, null);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong in the {nameof(CreateBankAccountTransferAsync)} service method {ex}");
                return APIResponse<BankAccountTransferResponse>.Create(HttpStatusCode.InternalServerError, null, "Internal Server error");
            }
        }
    }
}
