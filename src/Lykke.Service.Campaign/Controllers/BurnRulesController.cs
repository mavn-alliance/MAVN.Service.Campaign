using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common.Log;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Common.Log;
using Lykke.Service.Campaign.Client.Api;
using Lykke.Service.Campaign.Client.Models;
using Lykke.Service.Campaign.Client.Models.BurnRule.Requests;
using Lykke.Service.Campaign.Client.Models.BurnRule.Responses;
using Lykke.Service.Campaign.Client.Models.Enums;
using Lykke.Service.Campaign.Client.Models.Files.Requests;
using Lykke.Service.Campaign.Domain.Exceptions;
using Lykke.Service.Campaign.Domain.Models;
using Lykke.Service.Campaign.Domain.Models.BurnRules;
using Lykke.Service.Campaign.Domain.Services;
using Lykke.Service.Campaign.Strings;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.Campaign.Controllers
{
    [Route("api/burn-rules")]
    [ApiController]
    public class BurnRulesController : Controller, IBurnRulesApi
    {
        private readonly ILog _log;
        private readonly IMapper _mapper;
        private readonly IBurnRuleService _burnRuleService;

        public BurnRulesController(
            IBurnRuleService burnRuleService,
            IMapper mapper,
            ILogFactory logFactory)
        {
            _burnRuleService = burnRuleService ??
                               throw new ArgumentNullException(nameof(burnRuleService));
            _mapper = mapper;
            _log = logFactory.CreateLog(this);
        }

        /// <inheritdoc/>
        /// <returns code="200">A paginated collection of burn rules</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedBurnRuleListResponse), (int)HttpStatusCode.OK)]
        public async Task<PaginatedBurnRuleListResponse> GetAsync([FromQuery]  BurnRulePaginationRequest burnRulesPaginationRequest)
        {
            var burnRulePaginationRequestModel = _mapper.Map<BurnRuleListRequestModel>(burnRulesPaginationRequest);

            var paginatedResult = await _burnRuleService.GetPagedAsync(burnRulePaginationRequestModel);

            var mappedPagination = _mapper.Map<PaginatedBurnRuleListResponse>(paginatedResult);

            return mappedPagination;
        }

        /// <inheritdoc/>
        /// <response code="200">Burn rule response</response>
        [HttpGet("{burnRuleId}")]
        [ProducesResponseType(typeof(BurnRuleResponse), (int)HttpStatusCode.OK)]
        public async Task<BurnRuleResponse> GetByIdAsync(Guid burnRuleId)
        {
            try
            {
                var rule = await _burnRuleService.GetAsync(burnRuleId);

                return _mapper.Map<BurnRuleResponse>(rule);
            }
            catch (EntityNotFoundException e)
            {
                _log.Info(string.Format(Phrases.EntityWithIdNotFound, "Burn rule", burnRuleId), context: burnRuleId);

                return new BurnRuleResponse()
                {
                    ErrorCode = CampaignServiceErrorCodes.EntityNotFound,
                    ErrorMessage = e.Message
                };
            }
        }

        /// <inheritdoc/>
        /// <response code="200">The burn rule is successfully created.</response>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<BurnRuleCreateResponse> CreateAsync([FromBody] BurnRuleCreateRequest model)
        {
            var ruleToBeInserted = _mapper.Map<BurnRuleModel>(model);

            var createdId = await _burnRuleService.InsertAsync(ruleToBeInserted);

            return new BurnRuleCreateResponse()
            {
                BurnRuleId = createdId,
                ErrorCode = CampaignServiceErrorCodes.None
            };
        }

        /// <inheritdoc/>
        /// <response code="200">The burn rule is successfully updated.</response>
        /// <response code="400">The server cannot or will not process the request due to an apparent client error.</response>
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<BurnRuleResponse> UpdateAsync([FromBody] BurnRuleEditRequest request)
        {
            try
            {
                var burnRuleToUpdate = _mapper.Map<BurnRuleModel>(request);

                await _burnRuleService.UpdateAsync(burnRuleToUpdate);

                return new BurnRuleResponse()
                {
                    ErrorCode = CampaignServiceErrorCodes.None,
                    Id = burnRuleToUpdate.Id
                };
            }
            catch (EntityNotValidException e)
            {

                _log.Info(string.Format(Phrases.EntityNotValid, "Burn rule", request.Id), context: request.Id);

                return new BurnRuleResponse()
                {
                    ErrorCode = CampaignServiceErrorCodes.EntityNotValid,
                    ErrorMessage = e.Message
                };
            }
            catch (EntityNotFoundException e)
            {
                _log.Info(string.Format(Phrases.EntityWithIdNotFound, "Burn rule", request.Id), context: request.Id);

                return new BurnRuleResponse()
                {
                    ErrorCode = CampaignServiceErrorCodes.EntityNotFound,
                    ErrorMessage = e.Message
                };
            }
        }

        /// <inheritdoc/>
        /// <response code="200">The burn rule is successfully deleted.</response>
        [HttpDelete("{burnRuleId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<BurnRuleResponse> DeleteAsync([Required][FromRoute] Guid burnRuleId)
        {
            await _burnRuleService.DeleteAsync(burnRuleId);

            return new BurnRuleResponse()
            {
                ErrorCode = CampaignServiceErrorCodes.None
            };
        }

        /// <inheritdoc/>
        /// <response code="200">The burn rule's image is successfully created.</response>
        [HttpPost("image")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<CampaignServiceErrorResponseModel> AddImage(FileCreateRequest model)
        {
            try
            {
                var file = _mapper.Map<FileModel>(model);

                await _burnRuleService.SaveBurnRuleContentImage(file);

                return new CampaignServiceErrorResponseModel()
                {
                    ErrorCode = CampaignServiceErrorCodes.None
                };
            }
            catch (RuleConditionNotFileException ex)
            {
                _log.Info(string.Format(Phrases.InvalidContentType), context: model.RuleContentId);

                return new CampaignServiceErrorResponseModel()
                {
                    ErrorCode = CampaignServiceErrorCodes.NotValidRuleContentType,
                    ErrorMessage = ex.Message
                };
            }
            catch (EntityNotFoundException e)
            {
                _log.Info(string.Format(Phrases.EntityWithIdNotFound, "Burn rule", model.RuleContentId), context: model.RuleContentId);

                return new CampaignServiceErrorResponseModel()
                {
                    ErrorCode = CampaignServiceErrorCodes.EntityNotFound,
                    ErrorMessage = e.Message
                };
            }
            catch (NotValidFormatFile ex)
            {
                _log.Info(string.Format(Phrases.NotValidFormatFile), context: model.Type);

                return new CampaignServiceErrorResponseModel()
                {
                    ErrorCode = CampaignServiceErrorCodes.NotValidFileFormat,
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <inheritdoc/>
        /// <response code="200">The burn rule's image is successfully updated.</response>
        /// <response code="400">The server cannot or will not process the request due to an apparent client error.</response>
        [HttpPut("image")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<CampaignServiceErrorResponseModel> UpdateImage(FileEditRequest model)
        {
            try
            {
                var file = _mapper.Map<FileModel>(model);

                await _burnRuleService.SaveBurnRuleContentImage(file);

                return new CampaignServiceErrorResponseModel() { ErrorCode = CampaignServiceErrorCodes.None };
            }
            catch (RuleConditionNotFileException ex)
            {
                _log.Info(string.Format(Phrases.InvalidContentType), context: model.RuleContentId);

                return new CampaignServiceErrorResponseModel()
                {
                    ErrorCode = CampaignServiceErrorCodes.NotValidRuleContentType,
                    ErrorMessage = ex.Message
                };
            }
            catch (EntityNotFoundException e)
            {
                _log.Info(string.Format(Phrases.EntityWithIdNotFound, "Burn rule", model.RuleContentId), context: model.RuleContentId);

                return new CampaignServiceErrorResponseModel()
                {
                    ErrorCode = CampaignServiceErrorCodes.EntityNotFound,
                    ErrorMessage = e.Message
                };
            }
            catch (NotValidFormatFile ex)
            {
                _log.Info(string.Format(Phrases.NotValidFormatFile), context: model.Type);

                return new CampaignServiceErrorResponseModel()
                {
                    ErrorCode = CampaignServiceErrorCodes.NotValidFileFormat,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
