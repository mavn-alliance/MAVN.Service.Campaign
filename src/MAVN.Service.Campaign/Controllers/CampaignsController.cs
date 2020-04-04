using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common.Log;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Common.Log;
using MAVN.Service.Campaign.Client.Api;
using MAVN.Service.Campaign.Client.Models;
using MAVN.Service.Campaign.Client.Models.Campaign.Requests;
using MAVN.Service.Campaign.Client.Models.Campaign.Responses;
using MAVN.Service.Campaign.Client.Models.Enums;
using MAVN.Service.Campaign.Client.Models.Files.Requests;
using MAVN.Service.Campaign.Domain.Exceptions;
using MAVN.Service.Campaign.Domain.Models;
using MAVN.Service.Campaign.Domain.Services;
using MAVN.Service.Campaign.Strings;
using Microsoft.AspNetCore.Mvc;

namespace MAVN.Service.Campaign.Controllers
{
    [Route("api/campaigns")]
    [ApiController]
    public class CampaignsController : Controller, ICampaignsApi
    {
        private readonly ICampaignService _campaignService;
        private readonly IMapper _mapper;
        private readonly ILog _log;

        public CampaignsController(
            ICampaignService campaignService,
            IMapper mapper,
            ILogFactory logFactory)
        {
            _campaignService = campaignService;
            _log = logFactory.CreateLog(this);
            _mapper = mapper;
        }

        /// <inheritdoc/>
        /// <response code="200">A collection of campaigns.</response>
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedCampaignListResponseModel), (int) HttpStatusCode.OK)]
        public async Task<PaginatedCampaignListResponseModel> GetAsync(
            [FromQuery] CampaignsPaginationRequestModel campaignsPaginationRequestModel)
        {
            var campaignListRequestModel = _mapper.Map<CampaignListRequestModel>(campaignsPaginationRequestModel);

            var campaignsPaged = await _campaignService.GetPagedCampaignsAsync(campaignListRequestModel);

            var campaignModels = _mapper.Map<PaginatedCampaignListResponseModel>(campaignsPaged);

            return campaignModels;
        }

        /// <inheritdoc/>
        /// <response code="200">Campaign.</response>
        [HttpGet("{campaignId}")]
        [ProducesResponseType(typeof(CampaignDetailResponseModel), (int) HttpStatusCode.OK)]
        public async Task<CampaignDetailResponseModel> GetByIdAsync([Required] [FromRoute] string campaignId)
        {
            try
            {
                if (!Guid.TryParse(campaignId, out _))
                {
                    _log.Info(Phrases.InvalidIdentifier, process: nameof(GetByIdAsync), context: campaignId);

                    return new CampaignDetailResponseModel
                    {
                        ErrorCode = CampaignServiceErrorCodes.GuidCanNotBeParsed,
                        ErrorMessage = Phrases.InvalidIdentifier
                    };
                }

                var campaign = await _campaignService.GetCampaignAsync(campaignId);

                var campaignModel = _mapper.Map<CampaignDetailResponseModel>(campaign);

                return campaignModel;
            }
            catch (EntityNotFoundException e)
            {
                _log.Info(string.Format(Phrases.EntityWithIdNotFound, "Campaign", campaignId), context: campaignId);

                return new CampaignDetailResponseModel
                {
                    ErrorCode = CampaignServiceErrorCodes.EntityNotFound, ErrorMessage = e.Message
                };
            }
        }

        /// <inheritdoc/>
        /// <response code="200">CampaignsInfoListResponseModel.</response>
        [HttpGet("all")]
        [ProducesResponseType(typeof(CampaignsInfoListResponseModel), (int) HttpStatusCode.OK)]
        public async Task<CampaignsInfoListResponseModel> GetCampaignsByIds([FromQuery] Guid[] campaignsIds)
        {
            var campaigns = await _campaignService.GetCampaignsByIdsAsync(campaignsIds);

            return new CampaignsInfoListResponseModel
            {
                Campaigns = _mapper.Map<IReadOnlyCollection<CampaignInformationResponseModel>>(campaigns),
                ErrorCode = CampaignServiceErrorCodes.None
            };
        }

        /// <inheritdoc/>
        /// <response code="200">The campaign successfully created.</response>
        [HttpPost]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<CampaignCreateResponseModel> CreateCampaignAsync([FromBody] CampaignCreateModel model)
        {
            try
            {
                var campaign = _mapper.Map<CampaignDetails>(model);
                var campaignId = await _campaignService.InsertAsync(campaign);

                return new CampaignCreateResponseModel
                {
                    CampaignId = campaignId, ErrorCode = CampaignServiceErrorCodes.None
                };
            }
            catch (EntityNotValidException e)
            {
                _log.Info(Phrases.EntityNotValid, model);

                return new CampaignCreateResponseModel
                {
                    ErrorCode = CampaignServiceErrorCodes.EntityNotValid, ErrorMessage = e.Message
                };
            }
        }

        /// <inheritdoc/>
        /// <response code="200">The campaign successfully updated.</response>
        /// <response code="400">The server cannot or will not process the request due to an apparent client error.</response>
        [HttpPut]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<CampaignDetailResponseModel> UpdateAsync([FromBody] CampaignEditModel model)
        {
            try
            {
                var campaign = _mapper.Map<CampaignDetails>(model);

                if (!Guid.TryParse(model.Id, out _))
                {
                    _log.Info(Phrases.InvalidIdentifier, context: model.Id);

                    return new CampaignDetailResponseModel
                    {
                        ErrorCode = CampaignServiceErrorCodes.GuidCanNotBeParsed,
                        ErrorMessage = Phrases.InvalidIdentifier
                    };
                }

                await _campaignService.UpdateAsync(campaign);

                return new CampaignDetailResponseModel {ErrorCode = CampaignServiceErrorCodes.None};
            }
            catch (EntityNotFoundException e)
            {
                _log.Info(string.Format(Phrases.EntityWithIdNotFound, "Campaign", model.Id), context: model.Id);

                return new CampaignDetailResponseModel
                {
                    ErrorCode = CampaignServiceErrorCodes.EntityNotFound, ErrorMessage = e.Message
                };
            }
            catch (EntityNotValidException e)
            {
                _log.Info(Phrases.EntityNotValid, model);

                return new CampaignDetailResponseModel
                {
                    ErrorCode = CampaignServiceErrorCodes.EntityNotValid, ErrorMessage = e.Message
                };
            }
        }

        /// <inheritdoc/>
        /// <response code="200">The campaign successfully deleted.</response>
        [HttpDelete("{campaignId}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<CampaignDetailResponseModel> DeleteAsync([Required] [FromRoute] string campaignId)
        {
            if (!Guid.TryParse(campaignId, out _))
            {
                _log.Info(Phrases.InvalidIdentifier, context: campaignId);

                return new CampaignDetailResponseModel
                {
                    ErrorCode = CampaignServiceErrorCodes.GuidCanNotBeParsed,
                    ErrorMessage = Phrases.InvalidIdentifier
                };
            }

            await _campaignService.DeleteAsync(campaignId);

            return new CampaignDetailResponseModel {ErrorCode = CampaignServiceErrorCodes.None};
        }

        /// <inheritdoc/>
        /// <response code="200">The campaign's image is successfully created.</response>
        [HttpPost("image")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<CampaignServiceErrorResponseModel> AddImage(FileCreateRequest model)
        {
            try
            {
                var file = _mapper.Map<FileModel>(model);

                await _campaignService.SaveCampaignContentImage(file);

                return new CampaignServiceErrorResponseModel {ErrorCode = CampaignServiceErrorCodes.None};
            }
            catch (RuleConditionNotFileException ex)
            {
                _log.Info(string.Format(Phrases.InvalidContentType), model.RuleContentId);

                return new CampaignServiceErrorResponseModel
                {
                    ErrorCode = CampaignServiceErrorCodes.NotValidRuleContentType, ErrorMessage = ex.Message
                };
            }
            catch (EntityNotFoundException e)
            {
                _log.Info(string.Format(Phrases.EntityWithIdNotFound, "Campaign content rule", model.RuleContentId),
                    model.RuleContentId);

                return new CampaignServiceErrorResponseModel
                {
                    ErrorCode = CampaignServiceErrorCodes.EntityNotFound, ErrorMessage = e.Message
                };
            }
            catch (NotValidFormatFile ex)
            {
                _log.Info(string.Format(Phrases.NotValidFormatFile), context: model.Type);

                return new CampaignServiceErrorResponseModel
                {
                    ErrorCode = CampaignServiceErrorCodes.NotValidFileFormat, ErrorMessage = ex.Message
                };
            }
        }

        /// <inheritdoc/>
        /// <response code="200">The campaign's image is successfully updated.</response>
        /// <response code="400">The server cannot or will not process the request due to an apparent client error.</response>
        [HttpPut("image")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<CampaignServiceErrorResponseModel> UpdateImage(FileEditRequest model)
        {
            try
            {
                var file = _mapper.Map<FileModel>(model);

                await _campaignService.SaveCampaignContentImage(file);

                return new CampaignServiceErrorResponseModel {ErrorCode = CampaignServiceErrorCodes.None};
            }
            catch (RuleConditionNotFileException ex)
            {
                _log.Info(string.Format(Phrases.InvalidContentType), model.RuleContentId);

                return new CampaignServiceErrorResponseModel
                {
                    ErrorCode = CampaignServiceErrorCodes.NotValidRuleContentType, ErrorMessage = ex.Message
                };
            }
            catch (EntityNotFoundException e)
            {
                _log.Info(string.Format(Phrases.EntityWithIdNotFound, "Campaign content rule", model.RuleContentId),
                    model.RuleContentId);

                return new CampaignServiceErrorResponseModel
                {
                    ErrorCode = CampaignServiceErrorCodes.EntityNotFound, ErrorMessage = e.Message
                };
            }
            catch (NotValidFormatFile ex)
            {
                _log.Info(string.Format(Phrases.NotValidFormatFile), context: model.Type);

                return new CampaignServiceErrorResponseModel
                {
                    ErrorCode = CampaignServiceErrorCodes.NotValidFileFormat, ErrorMessage = ex.Message
                };
            }
        }
    }
}
