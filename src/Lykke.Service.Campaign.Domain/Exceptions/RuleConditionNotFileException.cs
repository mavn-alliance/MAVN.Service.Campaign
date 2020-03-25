using System;

namespace Lykke.Service.Campaign.Domain.Exceptions
{
    public class RuleConditionNotFileException : Exception
    {
    public RuleConditionNotFileException()
        : base("The passed rule content is not image type")
    {
    }

    public RuleConditionNotFileException(string message)
        : base(message)
    {
    }
    }
}
