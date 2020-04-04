using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common.Log;
using Lykke.Common.Log;
using MAVN.Service.Campaign.Client.Api;
using MAVN.Service.Campaign.Client.Models.BonusType;
using MAVN.Service.Campaign.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace MAVN.Service.Campaign.Controllers
{
    [Route("api/bonusTypes")]
    [ApiController]
    public class BonusTypesController : Controller, IBonusTypesApi
    {
        private readonly IBonusTypeService _bonusTypeService;
        private readonly IMapper _mapper;
        private readonly ILog _log;

        public BonusTypesController(
            IBonusTypeService bonusTypeService,
            IMapper mapper,
            ILogFactory logFactory)
        {
            _bonusTypeService = bonusTypeService;
            _mapper = mapper;
            _log = logFactory.CreateLog(this);
        }

        /// <inheritdoc />
        /// <response code="200">A collection of all Bonus Types.</response>
        [HttpGet]
        [ProducesResponseType(typeof(BonusTypeListResponseModel), (int)HttpStatusCode.OK)]
        public async Task<BonusTypeListResponseModel> GetAsync()
        {
            var bonusTypes = await _bonusTypeService.GetBonusTypesAsync();

            return new BonusTypeListResponseModel
            {
                BonusTypes = _mapper.Map<IReadOnlyCollection<BonusTypeModel>>(bonusTypes)
            };
        }

        /// <inheritdoc />
        /// <response code="200">A BonusTypeModel</response>
        /// <response code="204">No content if bonus type does not exists</response>
        [HttpGet("{type}")]
        [ProducesResponseType(typeof(BonusTypeModel), (int)HttpStatusCode.OK)]
        public async Task<BonusTypeModel> GetByTypeAsync(string type)
        {
            var bonusType = await _bonusTypeService.GetAsync(type);

            return _mapper.Map<BonusTypeModel>(bonusType);
        }

        /// <inheritdoc />
        /// <response code="200">A collection of active Bonus Types.</response>
        [HttpGet("active")]
        [ProducesResponseType(typeof(BonusTypeListResponseModel), (int)HttpStatusCode.OK)]
        public async Task<BonusTypeListResponseModel> GetActiveAsync()
        {
            var bonusTypes = await _bonusTypeService.GetActiveBonusTypesAsync();

            return new BonusTypeListResponseModel
            {
                BonusTypes = _mapper.Map<IReadOnlyCollection<BonusTypeModel>>(bonusTypes)
            };
        }
    }
}
