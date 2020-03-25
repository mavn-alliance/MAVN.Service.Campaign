using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker.Publisher;
using Lykke.Service.Campaign.Contract.Enums;
using Lykke.Service.Campaign.Contract.Events;
using Lykke.Service.Campaign.Domain.Enums;
using Lykke.Service.Campaign.Domain.Exceptions;
using Lykke.Service.Campaign.Domain.Models;
using Lykke.Service.Campaign.Domain.Models.BurnRules;
using Lykke.Service.Campaign.Domain.Repositories;
using Lykke.Service.Campaign.Domain.Services;

namespace Lykke.Service.Campaign.DomainServices.Services
{
    public class BurnRuleService : IBurnRuleService
    {
        private readonly IBurnRuleRepository _burnRuleRepository;
        private readonly IBurnRuleContentRepository _burnRuleContentRepository;
        private readonly IRabbitPublisher<SpendRuleChangedEvent> _spendRuleChangeEventPublisher;
        private readonly ILog _log;
        private readonly IFileService _fileService;
        private readonly IRuleContentValidationService _burnRuleContentValidation;
        private readonly IBurnRulePartnerRepository _burnRulePartnerRepository;
        private readonly string _assetName;

        public BurnRuleService(
            string assetName,
            IBurnRuleRepository burnRuleRepository,
            IBurnRuleContentRepository burnRuleContentRepository,
            IRabbitPublisher<SpendRuleChangedEvent> spendRuleChangeEventPublisher,
            ILogFactory logFactory,
            IFileService fileService,
            IRuleContentValidationService burnRuleContentValidation,
            IBurnRulePartnerRepository burnRulePartnerRepository)
        {
            _burnRuleRepository = burnRuleRepository ??
                                  throw new ArgumentException(nameof(burnRuleRepository));
            _burnRuleContentRepository = burnRuleContentRepository ??
                                         throw new ArgumentNullException(nameof(burnRuleContentRepository));
            _spendRuleChangeEventPublisher = spendRuleChangeEventPublisher;
            _fileService = fileService ??
                           throw new ArgumentNullException(nameof(fileService));
            _burnRuleContentValidation = burnRuleContentValidation ??
                                         throw new ArgumentNullException(nameof(burnRuleContentValidation));
            _burnRulePartnerRepository = burnRulePartnerRepository;
            _log = logFactory.CreateLog(this);
            _assetName = assetName ?? 
                         throw new ArgumentNullException(nameof(assetName));
        }

        public async Task<Guid> InsertAsync(BurnRuleModel burnRuleModel)
        {
            burnRuleModel.CreationDate = DateTime.UtcNow;

            if (burnRuleModel.UsePartnerCurrencyRate)
            {
                burnRuleModel.AmountInTokens = null;
                burnRuleModel.AmountInCurrency = null;
            }

            var earnRuleId = await _burnRuleRepository.InsertAsync(burnRuleModel);
                
            await PublishSpendRuleChangeEvent(burnRuleModel, ActionType.Created);

            return earnRuleId;
        }

        public async Task UpdateAsync(BurnRuleModel burnRuleModel)
        {
            var oldEntity = await _burnRuleRepository.GetAsync(burnRuleModel.Id);

            if (oldEntity == null)
            {
                throw new EntityNotFoundException($"Burn rule with id {burnRuleModel.Id} does not exist.");
            }

            var response = _burnRuleContentValidation.
                ValidateHaveInvalidOrEmptyIds(burnRuleModel.BurnRuleContents.Select(c => c.Id).ToList(),
                    oldEntity.BurnRuleContents.Select(c => c.Id).ToList());

            if (!response.IsValid)
            {
                throw new EntityNotValidException(string.Join(Environment.NewLine, response.ValidationMessages));
            }

            burnRuleModel.CreationDate = oldEntity.CreationDate;
            burnRuleModel.CreatedBy = oldEntity.CreatedBy;

            if (burnRuleModel.UsePartnerCurrencyRate)
            {
                burnRuleModel.AmountInTokens = null;
                burnRuleModel.AmountInCurrency = null;
            }
            
            var contentsToRemove = oldEntity.BurnRuleContents.Where(c1 => burnRuleModel.BurnRuleContents.All(c2 => c1.Id != c2.Id)).ToList();
            
            await _burnRuleContentRepository.DeleteAsync(contentsToRemove);

            foreach (var content in contentsToRemove)
            {
                if (content.RuleContentType == RuleContentType.UrlForPicture)
                {
                    await _fileService.DeleteAsync(content.Id);
                }
            }

            var partnersToRemove = oldEntity.PartnerIds.Where(c1 => burnRuleModel.PartnerIds.All(c2 => c1 != c2)).ToList();
            var partnersToAdd = burnRuleModel.PartnerIds.Where(c1 => oldEntity.PartnerIds.All(c2 => c1 != c2)).ToList();

            await _burnRulePartnerRepository.DeleteAsync(partnersToRemove, burnRuleModel.Id);
            await _burnRulePartnerRepository.InsertAsync(partnersToAdd, burnRuleModel.Id);

            await _burnRuleRepository.UpdateAsync(burnRuleModel);

            await PublishSpendRuleChangeEvent(burnRuleModel, ActionType.Edited);
        }

        public async Task DeleteAsync(Guid earnRuleId)
        {
            var rule = await _burnRuleRepository.GetAsync(earnRuleId);

            if (rule == null)
            {
                _log.Info($"Burn rule with id {earnRuleId} does not exists", context: earnRuleId, process: nameof(DeleteAsync));
                return;
            }

            await _burnRuleRepository.DeleteAsync(rule);

            foreach (var content in rule.BurnRuleContents)
            {
                if (content.RuleContentType == RuleContentType.UrlForPicture)
                {
                    await _fileService.DeleteAsync(content.Id);
                }
            }

            _log.Info($"Burn rule with id {earnRuleId} was deleted", context: earnRuleId, process: nameof(DeleteAsync));

            await PublishSpendRuleChangeEvent(rule, ActionType.Deleted);
        }

        public async Task<BurnRuleModel> GetAsync(Guid burnRuleId)
        {
            var rule = await _burnRuleRepository.GetAsync(burnRuleId);

            if (rule == null)
            {
                throw new EntityNotFoundException($"Burn rule with id {burnRuleId} does not exist.");
            }

            foreach (var content in rule.BurnRuleContents)
            {
                if (content.RuleContentType == RuleContentType.UrlForPicture)
                {
                    content.Image = await _fileService.GetAsync(content.Id);
                }
            }

            return rule;
        }

        public Task<IReadOnlyList<BurnRuleModel>> GetAsync(IReadOnlyList<Guid> identifiers)
        {
            return _burnRuleRepository.GetByIdentifiersAsync(identifiers);
        }

        public async Task<PaginatedBurnRuleList> GetPagedAsync(BurnRuleListRequestModel paginationModel)
        {
            return await _burnRuleRepository.GetPagedAsync(paginationModel);
        }

        public async Task<PaginatedBurnRuleListModel> GetLocalizedPagedAsync(Localization language, PaginationModel paginationModel)
        {
            var spendRulesPage = await _burnRuleRepository.GetPagedAsync(
                new BurnRuleListRequestModel { CurrentPage = paginationModel.CurrentPage, PageSize = paginationModel.PageSize });

            return new PaginatedBurnRuleListModel
            {
                BurnRules = spendRulesPage.BurnRules
                    .Select(o => new BurnRuleLocalizedModel
                    {
                        Id = o.Id,
                        Title = o.GetContent(RuleContentType.Title, language)?.Value,
                        AmountInTokens = o.AmountInTokens,
                        AmountInCurrency = o.AmountInCurrency,
                        UsePartnerCurrencyRate = o.UsePartnerCurrencyRate,
                        Description = o.GetContent(RuleContentType.Description, language)?.Value,
                        ImageUrl = o.GetContent(RuleContentType.UrlForPicture, language)?.Value,
                        PartnerIds = o.PartnerIds.ToArray(),
                        Vertical = o.Vertical,
                        Price = o.Price,
                        CurrencyName = _assetName,
                        CreationDate = o.CreationDate,
                        Order = o.Order
                    })
                    .ToList(),
                CurrentPage = spendRulesPage.CurrentPage,
                PageSize = spendRulesPage.PageSize,
                TotalCount = spendRulesPage.TotalCount,
            };
        }

        public async Task<BurnRuleLocalizedModel> GetAsync(Guid id, Localization language, bool includeDeleted = false)
        {
            var rule = await _burnRuleRepository.GetAsync(id, includeDeleted);

            if (rule == null)
                return null;

            return new BurnRuleLocalizedModel
            {
                Id = rule.Id,
                Title = rule.GetContent(RuleContentType.Title, language)?.Value,
                AmountInTokens = rule.AmountInTokens,
                AmountInCurrency = rule.AmountInCurrency,
                UsePartnerCurrencyRate = rule.UsePartnerCurrencyRate,
                Description = rule.GetContent(RuleContentType.Description, language)?.Value,
                ImageUrl = rule.GetContent(RuleContentType.UrlForPicture, language)?.Value,
                PartnerIds = rule.PartnerIds.ToArray(),
                Vertical = rule.Vertical,
                Price = rule.Price,
                CurrencyName = _assetName,
                CreationDate = rule.CreationDate,
                Order = rule.Order
            };
        }

        public async Task SaveBurnRuleContentImage(FileModel file)
        {
            var earnRuleContent = await _burnRuleContentRepository.GetAsync(file.RuleContentId);

            if (earnRuleContent == null)
            {
                throw new EntityNotFoundException($"Burn Content type with id {file.RuleContentId} does not exist.");
            }

            if (earnRuleContent.RuleContentType is RuleContentType.UrlForPicture)
            {
                var url = await _fileService.SaveAsync(file);

                earnRuleContent.Value = url;

                await _burnRuleContentRepository.UpdateAsync(earnRuleContent);
            }
            else
            {
                throw new RuleConditionNotFileException($"Burn Content type with id {file.RuleContentId} is not image type");
            }
        }

        public async Task<BurnRuleModel> GetByIdAsync(Guid burnRuleId)
        {
            var burnRule = await _burnRuleRepository.GetByIdAsync(burnRuleId);

            if (burnRule == null)
                throw new EntityNotFoundException($"Burn rule with id {burnRuleId} does not exist.");

            foreach (var content in burnRule.BurnRuleContents)
            {
                if (content.RuleContentType == RuleContentType.UrlForPicture)
                {
                    content.Image = await _fileService.GetAsync(content.Id);
                }
            }

            return burnRule;
        }

        private async Task PublishSpendRuleChangeEvent(BurnRuleModel spendRule, ActionType actionType)
        {
            var @event = new SpendRuleChangedEvent
            {
                Id = Guid.NewGuid(),
                SpendRuleId = spendRule.Id,
                Title = spendRule.Title,
                Description = spendRule.Description,
                AmountInTokens = spendRule.AmountInTokens,
                AmountInCurrency = spendRule.AmountInCurrency,
                TimeStamp = DateTime.UtcNow,
                Action = actionType
            };

            _log.Info("Spend rule change event published", @event);

            await _spendRuleChangeEventPublisher.PublishAsync(@event);
        }
    }
}
