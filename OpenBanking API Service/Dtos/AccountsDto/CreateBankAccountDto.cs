namespace OpenBanking_API_Service.Dtos.AccountsDto
{
    public class CreateBankAccountDto
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Middlename { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string StateOfOrigin { get; set; }
        public string MaritalStatus { get; set; }
        public int Pin { get; set; }

    }
}
