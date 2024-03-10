

using System.ComponentModel;

namespace OpenBanking_API_Service.Domain.Enums
{
    public enum MaritalStatus
    {
        [Description("Single")]
        OptionOne = 1,

        [Description("Married")]
        OptionTwo = 2,

        [Description("Divorced")]
        OptionThree = 3,


    }

}
