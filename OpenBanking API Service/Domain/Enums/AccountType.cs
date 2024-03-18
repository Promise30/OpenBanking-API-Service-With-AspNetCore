using System.ComponentModel;

namespace OpenBanking_API_Service.Domain.Enums
{
    public enum AccountType
    {
        [Description("Savings")]
        OptionOne = 1,

        [Description("Current")]
        OptionTwo = 2,

        [Description("FixedDeposit")]

        OptionThree = 3,

    }
}
