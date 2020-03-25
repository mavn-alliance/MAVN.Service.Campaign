using System;
using Lykke.Service.Campaign.Domain.Enums;

namespace Lykke.Service.Campaign.Domain.Models.BurnRules
{
    public class BurnRuleContentModel
    {
        public Guid Id { get; set; }

        public Guid BurnRuleId { get; set; }

        public RuleContentType RuleContentType { get; set; }

        public Localization Localization { get; set; }

        public string Value { get; set; }

        public FileResponseModel Image { get; set; }
    }
}
