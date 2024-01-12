using AutoMapper;
using OpenBanking_API_Service.Domain.Entities.Account;
using OpenBanking_API_Service.Dtos.AccountsDto.Requests;
using OpenBanking_API_Service.Dtos.AccountsDto.Responses;

namespace OpenBanking_API_Service.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BankAccountDto, BankAccount>();
            CreateMap<BankAccount, BankAccountDto>();

            CreateMap<BankDeposit, BankAccountDepositResponse>();
            CreateMap<BankAccountDepositResponse, BankDeposit>();

            CreateMap<CreateBankAccountDeposit, BankDeposit>();
            CreateMap<BankDeposit, BankAccountDepositResponse>();

            CreateMap<CreateBankAccountWithdrawal, BankWithdrawal>();
            CreateMap<BankWithdrawal, BankAccountWithdrawalResponse>();

            CreateMap<CreateBankAccountTransfer, BankTransfer>();
            CreateMap<BankTransfer, BankAccountTransferResponse>();

        }
    }
}
