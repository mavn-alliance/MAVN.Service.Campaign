using System;
using System.Threading.Tasks;
using AutoMapper;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Service.Campaign.Client.Api;
using Lykke.Service.Campaign.Client.Models.Condition;
using Lykke.Service.Campaign.Client.Models.Enums;
using Lykke.Service.Campaign.Domain.Exceptions;
using Lykke.Service.Campaign.Domain.Services;
using Lykke.Service.Campaign.Strings;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.Campaign.Controllers
{
    [Route("api/conditions")]
    [ApiController]
    public class ConditionsController : Controller, IConditionsApi
    {
        private readonly IConditionService _conditionService;
        private readonly IMapper _mapper;
        private readonly ILog _log;

        public ConditionsController(
            IConditionService conditionService,
            IMapper mapper,
            ILogFactory logFactory)
        {
            _conditionService = conditionService;
            _mapper = mapper;
            _log = logFactory.CreateLog(this);
        }

        /// <inheritdoc/>
        /// <response code="200">ConditionDetailsResponseModel.</response>
        [HttpGet("{conditionId}")]
        public async Task<ConditionDetailsResponseModel> GetByIdAsync(string conditionId)
        {
            try
            {
                if (!Guid.TryParse(conditionId, out var conditionGuid))
                {
                    _log.Info(Phrases.InvalidIdentifier, process: nameof(GetByIdAsync), context: conditionId);

                    return new ConditionDetailsResponseModel
                    {
                        ErrorCode = CampaignServiceErrorCodes.GuidCanNotBeParsed,
                        ErrorMessage = Phrases.InvalidIdentifier
                    };
                }

                var condition = await _conditionService.GetConditionByIdAsync(conditionGuid);

                return new ConditionDetailsResponseModel
                {
                    Condition = _mapper.Map<ConditionModel>(condition),
                    ErrorCode = CampaignServiceErrorCodes.None
                };
            }
            catch (EntityNotFoundException e)
            {
                _log.Info(string.Format(Phrases.EntityWithIdNotFound, "Condition", conditionId), context: conditionId);

                return new ConditionDetailsResponseModel
                {
                    ErrorCode = CampaignServiceErrorCodes.EntityNotFound,
                    ErrorMessage = e.Message
                };
            }
        }
    }
}
