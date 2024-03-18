using AutoMapper;
using OpenBanking_API_Service.Domain.Entities.Account;
using OpenBanking_API_Service.Domain.Enums;
using OpenBanking_API_Service.Dtos.AccountsDto.Requests;
using OpenBanking_API_Service.Dtos.AccountsDto.Responses;

namespace OpenBanking_API_Service.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BankAccountDto, BankAccount>();
            CreateMap<BankAccount, BankAccountDto>()
                .ForMember(dest => dest.MaritalStatus, opt => opt.MapFrom(src => src.MaritalStatus.GetDescription()))
                .ForMember(dest => dest.AccountType, opt => opt.MapFrom(src => src.AccountType.GetDescription()));


            CreateMap<BankDeposit, BankAccountDepositResponse>();
            CreateMap<BankAccountDepositResponse, BankDeposit>();

            CreateMap<CreateBankAccountDeposit, BankDeposit>();
            CreateMap<BankDeposit, BankAccountDepositResponse>();

            CreateMap<CreateBankAccountWithdrawal, BankWithdrawal>();
            CreateMap<BankWithdrawal, BankAccountWithdrawalResponse>().ReverseMap();

            CreateMap<CreateBankAccountTransfer, BankTransfer>();
            CreateMap<BankTransfer, BankAccountTransferResponse>();

            CreateMap<BankAccountForUpdateDto, BankAccount>().ReverseMap();
        }

    }
}
