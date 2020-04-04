using System;

namespace MAVN.Service.Campaign.Domain.Exceptions
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
