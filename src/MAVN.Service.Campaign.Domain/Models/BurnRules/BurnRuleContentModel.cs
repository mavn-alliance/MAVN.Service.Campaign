using System;
using MAVN.Service.Campaign.Domain.Enums;

namespace MAVN.Service.Campaign.Domain.Models.BurnRules
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
