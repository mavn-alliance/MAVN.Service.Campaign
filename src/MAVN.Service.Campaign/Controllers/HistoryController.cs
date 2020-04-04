using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using MAVN.Service.Campaign.Client.Api;
using MAVN.Service.Campaign.Client.Models.BurnRule.Responses;
using MAVN.Service.Campaign.Client.Models.Campaign.Responses;
using MAVN.Service.Campaign.Client.Models.Enums;
using MAVN.Service.Campaign.Domain.Exceptions;
using MAVN.Service.Campaign.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace MAVN.Service.Campaign.Controllers
{
    [ApiController]
    [Route("api/history")]
    public class HistoryController : ControllerBase, IHistoryApi
    {
        private readonly ICampaignService _campaignService;
        private readonly IBurnRuleService _burnRuleService;
        private readonly IMapper _mapper;

        public HistoryController(
            ICampaignService campaignService,
            IBurnRuleService burnRuleService,
            IMapper mapper)
        {
            _campaignService = campaignService;
            _burnRuleService = burnRuleService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns earn rules by identifiers.
        /// </summary>
        /// <param name="identifiers">The collection of earn rules identifiers.</param>
        /// <returns>A collection of earn rules.</returns>
        /// <response code="200">A collection of earn rules.</response>
        [HttpGet("earnRules")]
        [ProducesResponseType(typeof(IReadOnlyList<CampaignResponse>), (int) HttpStatusCode.OK)]
        public async Task<IReadOnlyList<CampaignInformationResponseModel>> GetEarnRulesAsync(
            [FromQuery] Guid[] identifiers)
        {
            var campaigns = await _campaignService.GetCampaignsByIdsAsync(identifiers);

            return _mapper.Map<IReadOnlyList<CampaignInformationResponseModel>>(campaigns);
        }

        /// <summary>
        /// Returns earn rule by identifier.
        /// </summary>
        /// <param name="earnRuleId">The earn rule identifier.</param>
        /// <returns>The earn rule response.</returns>
        /// <remarks> 
        /// Error codes:
        /// - **EntityNotFound**
        /// </remarks>
        /// <response code="200">The earn rule response.</response>
        [HttpGet("earnRules/{earnRuleId}")]
        [ProducesResponseType(typeof(CampaignDetailResponseModel), (int) HttpStatusCode.OK)]
        public async Task<CampaignDetailResponseModel> GetEarnRuleByIdAsync(Guid earnRuleId)
        {
            try
            {
                var campaign = await _campaignService.GetByIdAsync(earnRuleId);

                return _mapper.Map<CampaignDetailResponseModel>(campaign);
            }
            catch (EntityNotFoundException exception)
            {
                return new CampaignDetailResponseModel
                {
                    ErrorCode = CampaignServiceErrorCodes.EntityNotFound,
                    ErrorMessage = exception.Message
                };
            }
        }

        /// <summary>
        /// Returns burn rules by identifiers.
        /// </summary>
        /// <param name="identifiers">The collection of burn rules identifiers.</param>
        /// <returns>A collection of burn rules.</returns>
        /// <response code="200">A collection of burn rules.</response>
        [HttpGet("burnRules")]
        [ProducesResponseType(typeof(IReadOnlyList<CampaignResponse>), (int) HttpStatusCode.OK)]
        public async Task<IReadOnlyList<BurnRuleInfoResponse>> GetBurnRulesAsync(Guid[] identifiers)
        {
            var burnRules = await _burnRuleService.GetAsync(identifiers);

            return _mapper.Map<IReadOnlyList<BurnRuleInfoResponse>>(burnRules);
        }

        /// <summary>
        /// Returns burn rule by identifier.
        /// </summary>
        /// <param name="burnRuleId">The burn rule identifier.</param>
        /// <returns>The burn rule response.</returns>
        /// <remarks> 
        /// Error codes:
        /// - **EntityNotFound**
        /// </remarks>
        /// <response code="200">The burn rule response.</response>
        [HttpGet("burnRules/{burnRuleId}")]
        [ProducesResponseType(typeof(BurnRuleResponse), (int) HttpStatusCode.OK)]
        public async Task<BurnRuleResponse> GetBurnRuleByIdAsync(Guid burnRuleId)
        {
            try
            {
                var burnRule = await _burnRuleService.GetByIdAsync(burnRuleId);

                return _mapper.Map<BurnRuleResponse>(burnRule);
            }
            catch (EntityNotFoundException exception)
            {
                return new BurnRuleResponse
                {
                    ErrorCode = CampaignServiceErrorCodes.EntityNotFound,
                    ErrorMessage = exception.Message
                };
            }
        }

        /// <summary>
        /// Returns a localized earn rule model even if it's deleted.
        /// </summary>
        /// <param name="earnRuleId">EarnRule's id</param>
        /// <param name="language">The language of content.</param>
        /// <returns code="200">A earn rule model.</returns>
        [HttpGet("earnRulesMobile/{earnRuleId}")]
        [ProducesResponseType(typeof(EarnRuleLocalizedResponse), (int)HttpStatusCode.OK)]
        public async Task<EarnRuleLocalizedResponse> GetEarnRuleMobileAsync(Guid earnRuleId, Localization language)
        {
            var earnRule = await _campaignService
                .GetHistoryAsync(earnRuleId, Enum.Parse<Domain.Enums.Localization>(language.ToString(), true));

            return _mapper.Map<EarnRuleLocalizedResponse>(earnRule);
        }
    }
}
