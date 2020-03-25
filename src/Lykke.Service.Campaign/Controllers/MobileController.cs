using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.Campaign.Client.Api;
using Lykke.Service.Campaign.Client.Models;
using Lykke.Service.Campaign.Client.Models.BurnRule.Responses;
using Lykke.Service.Campaign.Client.Models.Campaign.Responses;
using Lykke.Service.Campaign.Client.Models.Enums;
using Lykke.Service.Campaign.Domain.Models;
using Lykke.Service.Campaign.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.Campaign.Controllers
{
    [ApiController]
    [Route("api/mobile")]
    public class MobileController : ControllerBase, IMobileApi
    {
        private readonly IBurnRuleService _burnRuleService;
        private readonly ICampaignService _campaignService;
        private readonly IMapper _mapper;

        public MobileController(
            IBurnRuleService burnRuleService,
            ICampaignService campaignService,
            IMapper mapper)
        {
            _burnRuleService = burnRuleService;
            _campaignService = campaignService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns a collection of localized spend rules.
        /// </summary>
        /// <param name="language">The language of content.</param>
        /// <param name="pagination">Pagination</param>
        /// <returns code="200">A collection of spend rules.</returns>
        [HttpGet("burn-rules")]
        [ProducesResponseType(typeof(BurnRulePaginatedResponseModel), (int) HttpStatusCode.OK)]
        public async Task<BurnRulePaginatedResponseModel> GetSpendRulesAsync(Localization language, BasePaginationRequestModel pagination)
        {
            var spendRules = await _burnRuleService.GetLocalizedPagedAsync(
                Enum.Parse<Domain.Enums.Localization>(language.ToString(), true),
                _mapper.Map<PaginationModel>(pagination));

            return _mapper.Map<BurnRulePaginatedResponseModel>(spendRules);
        }

        /// <summary>
        /// Returns a collection of localized spend rules.
        /// </summary>
        /// <param name="language">The language of content.</param>
        /// <param name="burnRuleId">SpendRule's id</param>
        /// <param name="includeDeleted">Indicates that the deleted spend rules should be returned.</param>
        /// <returns code="200">A collection of spend rules.</returns>
        [HttpGet("burn-rules/{burnRuleId}")]
        [ProducesResponseType(typeof(BurnRuleLocalizedResponse), (int)HttpStatusCode.OK)]
        public async Task<BurnRuleLocalizedResponse> GetSpendRuleAsync(Guid burnRuleId, Localization language,
            bool includeDeleted)
        {
            var spendRule = await _burnRuleService
                .GetAsync(burnRuleId, Enum.Parse<Domain.Enums.Localization>(language.ToString(), true), includeDeleted);

            return _mapper.Map<BurnRuleLocalizedResponse>(spendRule);
        }

        /// <summary>
        /// Returns a collection of localized earn rules.
        /// </summary>
        /// <param name="language">The language of content.</param>
        /// <param name="statuses">Filter by earn rule's status</param>
        /// <param name="pagination">Pagination</param>
        /// <returns code="200">A collection of earn rules.</returns>
        [HttpGet("earn-rules")]
        [ProducesResponseType(typeof(EarnRulePaginatedResponseModel), (int)HttpStatusCode.OK)]
        public async Task<EarnRulePaginatedResponseModel> GetEarnRulesAsync(
            Localization language,
            [FromQuery] CampaignStatus[] statuses,
            [FromQuery] BasePaginationRequestModel pagination)
        {
            var earnRules = await _campaignService
                .GetEarnRulesPagedAsync(
                    Enum.Parse<Domain.Enums.Localization>(language.ToString(), true),
                    _mapper.Map<List<Domain.Enums.CampaignStatus>>(statuses),
                    _mapper.Map<PaginationModel>(pagination));

            return _mapper.Map<EarnRulePaginatedResponseModel>(earnRules);
        }

        /// <summary>
        /// Returns a localized earn rule model.
        /// </summary>
        /// <param name="earnRuleId">EarnRule's id</param>
        /// <param name="language">The language of content.</param>
        /// <returns code="200">A earn rule model.</returns>
        [HttpGet("earn-rules/{earnRuleId}")]
        [ProducesResponseType(typeof(EarnRuleLocalizedResponse), (int)HttpStatusCode.OK)]
        public async Task<EarnRuleLocalizedResponse> GetEarnRuleAsync(Guid earnRuleId, Localization language)
        {
            var earnRule = await _campaignService
                .GetAsync(earnRuleId, Enum.Parse<Domain.Enums.Localization>(language.ToString(), true));

            return _mapper.Map<EarnRuleLocalizedResponse>(earnRule);
        }
    }
}
