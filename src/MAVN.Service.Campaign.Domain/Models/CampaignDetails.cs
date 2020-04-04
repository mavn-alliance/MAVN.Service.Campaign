using System.Collections.Generic;
using System.Linq;
using MAVN.Service.Campaign.Domain.Enums;
using MAVN.Service.Campaign.Domain.Models.EarnRules;

namespace MAVN.Service.Campaign.Domain.Models
{
    public class CampaignDetails : Campaign
    {
        public IReadOnlyList<EarnRuleContentModel> Contents { get; set; }

        public EarnRuleContentModel GetContent(RuleContentType contentType, Localization language)
        {
            if (Contents == null)
                return null;

            var content = Contents
                .FirstOrDefault(o => o.RuleContentType == contentType && o.Localization == language);

            if (content != null)
                return content;

            return Contents
                .FirstOrDefault(o => o.RuleContentType == contentType && o.Localization == Localization.En);
        }
    }
}
