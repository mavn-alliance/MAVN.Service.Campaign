using AutoFixture;
using Lykke.Logs;
using MAVN.Service.Campaign.Domain.Enums;
using MAVN.Service.Campaign.Domain.Exceptions;
using MAVN.Service.Campaign.Domain.Models;
using MAVN.Service.Campaign.Domain.Models.BurnRules;
using MAVN.Service.Campaign.Domain.Repositories;
using MAVN.Service.Campaign.Domain.Services;
using MAVN.Service.Campaign.DomainServices.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.RabbitMqBroker.Publisher;
using MAVN.Service.Campaign.Contract.Events;
using Xunit;

namespace MAVN.Service.Campaign.Tests.DomainServices
{
    public class BurnRuleServiceTests
    {
        private readonly Mock<IBurnRuleRepository> _burnRuleRepositoryMock;
        private readonly Mock<IBurnRuleContentRepository> _burnRuleContentRepositoryMock;
        private readonly Mock<IRabbitPublisher<SpendRuleChangedEvent>> _spendRuleChangeEventPublisher;
        private readonly Mock<IFileService> _fileServiceMock;
        private readonly Mock<IRuleContentValidationService> _burnRuleContentValidationMock;
        private readonly Mock<IBurnRulePartnerRepository> _burnRulePartnerRepositoryMock;
        private const string AssetName = "AED";

        public BurnRuleServiceTests()
        {
            MapperHelper.CreateAutoMapper();

            _burnRuleContentRepositoryMock = new Mock<IBurnRuleContentRepository>();
            _burnRuleRepositoryMock = new Mock<IBurnRuleRepository>();
            _spendRuleChangeEventPublisher = new Mock<IRabbitPublisher<SpendRuleChangedEvent>>();
            _burnRuleContentValidationMock = new Mock<IRuleContentValidationService>();
            _burnRulePartnerRepositoryMock = new Mock<IBurnRulePartnerRepository>();
            _fileServiceMock = new Mock<IFileService>();
        }

        //InsertAsync
        [Fact]
        public async Task Should_CallBurnRuleRepositoryInsertAsync_When_CallingInsertAsync()
        {
            var fixture = new Fixture();
            var burnModel = fixture.Create<BurnRuleModel>();

            _burnRuleRepositoryMock.Setup(b => b.InsertAsync(It.IsAny<BurnRuleModel>()))
                .ReturnsAsync(Guid.NewGuid);

            var service = new BurnRuleService(
                AssetName,
                _burnRuleRepositoryMock.Object,
                _burnRuleContentRepositoryMock.Object,
                _spendRuleChangeEventPublisher.Object,
                EmptyLogFactory.Instance,
                _fileServiceMock.Object,
                _burnRuleContentValidationMock.Object,
                _burnRulePartnerRepositoryMock.Object);

            var result = await service.InsertAsync(burnModel);

            _burnRuleRepositoryMock
                .Verify(b => b.InsertAsync(burnModel), Times.Once);
            Assert.IsType<Guid>(result);
        }

        //UpdateAsync
        [Fact]
        public async Task Should_ThrowEntityNotFoundException_When_UpdatingNotExistingBurnRule()
        {
            var fixture = new Fixture();
            var burnModel = fixture.Build<BurnRuleModel>()
                .With(c => c.Id, Guid.Parse("26a88452-48ab-48eb-b9df-a22b281d1583"))
                .Create();

            _burnRuleRepositoryMock.Setup(b => b.GetAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync((BurnRuleModel)null);

            var service = new BurnRuleService(
                AssetName,
                _burnRuleRepositoryMock.Object,
                _burnRuleContentRepositoryMock.Object,
                _spendRuleChangeEventPublisher.Object,
                EmptyLogFactory.Instance,
                _fileServiceMock.Object,
                _burnRuleContentValidationMock.Object,
                _burnRulePartnerRepositoryMock.Object);

            var exception = await Record.ExceptionAsync(() => service.UpdateAsync(burnModel));

            Assert.NotNull(exception);
            Assert.IsType<EntityNotFoundException>(exception);
            Assert.Equal("Burn rule with id 26a88452-48ab-48eb-b9df-a22b281d1583 does not exist.", exception.Message);
        }

        [Fact]
        public async Task Should_ThrowEntityNotValidException_When_UpdatingBurnRuleWithWrongContentsIds()
        {
            var fixture = new Fixture();

            var contentOld = fixture.Build<BurnRuleContentModel>()
                .With(c => c.Id, Guid.Parse("26a88452-48ab-48eb-b9df-a22b281d1584"))
                .Create();

            var burnModelNew = fixture.Build<BurnRuleModel>()
                .With(c => c.Id, Guid.Parse("26a88452-48ab-48eb-b9df-a22b281d1581"))
                .Create();

            var burnModelOld = fixture.Build<BurnRuleModel>()
                .With(c => c.BurnRuleContents, new List<BurnRuleContentModel>() { contentOld })
                .Create();

            var validationResult = new ValidationResult();
            validationResult.Add("Rule does not have any contents with id: 26a88452-48ab-48eb-b9df-a22b281d1583");

            _burnRuleContentValidationMock.Setup(c =>
                    c.ValidateHaveInvalidOrEmptyIds(It.IsAny<List<Guid>>(), It.IsAny<List<Guid>>()))
                .Returns(validationResult);

            _burnRuleRepositoryMock.Setup(b => b.GetAsync(Guid.Parse("26a88452-48ab-48eb-b9df-a22b281d1581"), false))
                .ReturnsAsync(burnModelOld);

            var service = new BurnRuleService(
                AssetName,
                _burnRuleRepositoryMock.Object,
                _burnRuleContentRepositoryMock.Object,
                _spendRuleChangeEventPublisher.Object,
                EmptyLogFactory.Instance,
                _fileServiceMock.Object,
                _burnRuleContentValidationMock.Object,
                _burnRulePartnerRepositoryMock.Object);

            var exception = await Record.ExceptionAsync(() => service.UpdateAsync(burnModelNew));

            Assert.NotNull(exception);
            Assert.IsType<EntityNotValidException>(exception);
            Assert.Equal("Rule does not have any contents with id: 26a88452-48ab-48eb-b9df-a22b281d1583", exception.Message);
        }

        [Fact]
        public async Task Should_RemoveOldContentsAdDeleteImagesIfTypeIsPictureUrl_When_UpdatingBurnRuleNewContentsIds()
        {
            var fixture = new Fixture();

            var contentNew = fixture.Build<BurnRuleContentModel>()
                .With(c => c.Id, Guid.Parse("26a88452-48ab-48eb-b9df-a22b281d1583"))
                .Create();

            var contentOld = fixture.Build<BurnRuleContentModel>()
                .With(c => c.Id, Guid.Parse("26a88452-48ab-48eb-b9df-a22b281d1584"))
                .With(c => c.RuleContentType, RuleContentType.UrlForPicture)
                .Create();

            var burnModelNew = fixture.Build<BurnRuleModel>()
                .With(c => c.Id, Guid.Parse("26a88452-48ab-48eb-b9df-a22b281d1581"))
                .With(c => c.BurnRuleContents, new List<BurnRuleContentModel>() { contentNew })
                .Create();

            var burnModelOld = fixture.Build<BurnRuleModel>()
                .With(c => c.BurnRuleContents, new List<BurnRuleContentModel>() { contentOld })
                .Create();

            //Valid entity
            var validationResult = new ValidationResult();

            _burnRuleContentValidationMock.Setup(c =>
                    c.ValidateHaveInvalidOrEmptyIds(It.IsAny<List<Guid>>(), It.IsAny<List<Guid>>()))
                .Returns(validationResult);

            _burnRuleRepositoryMock.Setup(b => b.GetAsync(Guid.Parse("26a88452-48ab-48eb-b9df-a22b281d1581"), false))
                .ReturnsAsync(burnModelOld);

            _burnRuleContentRepositoryMock
                .Setup(c => c.DeleteAsync(It.IsAny<List<BurnRuleContentModel>>()))
                .Returns(Task.CompletedTask);

            _fileServiceMock.Setup(c => c.DeleteAsync(It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);

            _burnRuleRepositoryMock.Setup(br => br.UpdateAsync(It.IsAny<BurnRuleModel>()))
                .Returns(Task.CompletedTask);

            var service = new BurnRuleService(
                AssetName,
                _burnRuleRepositoryMock.Object,
                _burnRuleContentRepositoryMock.Object,
                _spendRuleChangeEventPublisher.Object,
                EmptyLogFactory.Instance,
                _fileServiceMock.Object,
                _burnRuleContentValidationMock.Object,
                _burnRulePartnerRepositoryMock.Object);

            var exception = await Record.ExceptionAsync(() => service.UpdateAsync(burnModelNew));

            Assert.Null(exception);

            _burnRuleRepositoryMock.
                Verify(r => r.GetAsync(It.IsAny<Guid>(), It.IsAny<bool>()), Times.Once);

            _fileServiceMock
                .Verify(c => c.DeleteAsync(Guid.Parse("26a88452-48ab-48eb-b9df-a22b281d1584")), Times.Once);

            _burnRuleContentRepositoryMock
                .Verify(r => r.DeleteAsync(It.IsAny<List<BurnRuleContentModel>>()), Times.Once);

            _burnRuleRepositoryMock
                .Verify(expression: br => br.UpdateAsync(burnModelNew), Times.Once);
        }

        //DeleteAsync
        [Fact]
        public async Task Should_RemoveOldContentsAndDeleteImagesIfTypeIsPictureUrl_When_DeletingBurnRule()
        {
            var fixture = new Fixture();

            var contentOld = fixture.Build<BurnRuleContentModel>()
                .With(c => c.Id, Guid.Parse("26a88452-48ab-48eb-b9df-a22b281d1584"))
                .With(c => c.RuleContentType, RuleContentType.UrlForPicture)
                .Create();

            var contentOld1 = fixture.Build<BurnRuleContentModel>()
                .With(c => c.Id, Guid.Parse("26a88452-48ab-48eb-b9df-a22b281d1584"))
                .With(c => c.RuleContentType, RuleContentType.Description)
                .Create();

            var burnModelOld = fixture.Build<BurnRuleModel>()
                .With(c => c.BurnRuleContents,
                    new List<BurnRuleContentModel>()
                    {
                        contentOld,
                        contentOld1
                    })
                .Create();

            _burnRuleRepositoryMock.Setup(b => b.GetAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
          .ReturnsAsync(burnModelOld);

            _fileServiceMock.Setup(c => c.DeleteAsync(It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);

            _burnRuleRepositoryMock.Setup(br => br.DeleteAsync(It.IsAny<BurnRuleModel>()))
                .Returns(Task.CompletedTask);

            var service = new BurnRuleService(
                AssetName,
                _burnRuleRepositoryMock.Object,
                _burnRuleContentRepositoryMock.Object,
                _spendRuleChangeEventPublisher.Object,
                EmptyLogFactory.Instance,
                _fileServiceMock.Object,
                _burnRuleContentValidationMock.Object,
                _burnRulePartnerRepositoryMock.Object);

            var exception = await Record.ExceptionAsync(() => service.DeleteAsync(burnModelOld.Id));

            Assert.Null(exception);

            _burnRuleRepositoryMock.
                Verify(r => r.GetAsync(It.IsAny<Guid>(), It.IsAny<bool>()), Times.Once);

            _fileServiceMock
                .Verify(c => c.DeleteAsync(Guid.Parse("26a88452-48ab-48eb-b9df-a22b281d1584")), Times.Once);

            _burnRuleRepositoryMock
                .Verify(expression: br => br.DeleteAsync(burnModelOld), Times.Once);
        }

        [Fact]
        public async Task Should_ContinueWhenEntityAlreadyDeleted_When_DeletingBurnRule()
        {
            _burnRuleRepositoryMock.Setup(b => b.GetAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
          .ReturnsAsync(() => null);

            _fileServiceMock.Setup(c => c.DeleteAsync(It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);

            _burnRuleRepositoryMock.Setup(br => br.DeleteAsync(It.IsAny<BurnRuleModel>()))
                .Returns(Task.CompletedTask);

            var service = new BurnRuleService(
                AssetName,
                _burnRuleRepositoryMock.Object,
                _burnRuleContentRepositoryMock.Object,
                _spendRuleChangeEventPublisher.Object,
                EmptyLogFactory.Instance,
                _fileServiceMock.Object,
                _burnRuleContentValidationMock.Object,
                _burnRulePartnerRepositoryMock.Object);

            var exception = await Record.ExceptionAsync(() => service.DeleteAsync(Guid.NewGuid()));

            Assert.Null(exception);

            _burnRuleRepositoryMock.
                Verify(r => r.GetAsync(It.IsAny<Guid>(), It.IsAny<bool>()), Times.Once);

            _fileServiceMock
                .Verify(c => c.DeleteAsync(It.IsAny<Guid>()), Times.Never);

            _burnRuleRepositoryMock
                .Verify(expression: br => br.DeleteAsync(It.IsAny<BurnRuleModel>()), Times.Never);
        }

        //GetAsync
        [Fact]
        public async Task Should_CallFileServiceGetBlob_When_GetAsyncWithContentTypeUrlForPicture()
        {
            var fixture = new Fixture();

            var content = fixture.Build<BurnRuleContentModel>()
                .With(c => c.Id, Guid.Parse("26a88452-48ab-48eb-b9df-a22b281d1584"))
                .With(c => c.RuleContentType, RuleContentType.UrlForPicture)
                .Create();

            var content1 = fixture.Build<BurnRuleContentModel>()
                .With(c => c.Id, Guid.Parse("26a88452-48ab-48eb-b9df-a22b281d1589"))
                .With(c => c.RuleContentType, RuleContentType.Description)
                .Create();

            var burnModel = fixture.Build<BurnRuleModel>()
                .With(c => c.BurnRuleContents,
                    new List<BurnRuleContentModel>()
                    {
                        content,
                        content1
                    })
                .Create();

            _burnRuleRepositoryMock.Setup(b => b.GetAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
          .ReturnsAsync(burnModel);

            _fileServiceMock.Setup(c => c.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new FileResponseModel());

            var service = new BurnRuleService(
                AssetName,
                _burnRuleRepositoryMock.Object,
                _burnRuleContentRepositoryMock.Object,
                _spendRuleChangeEventPublisher.Object,
                EmptyLogFactory.Instance,
                _fileServiceMock.Object,
                _burnRuleContentValidationMock.Object,
                _burnRulePartnerRepositoryMock.Object);

            var exception = await Record.ExceptionAsync(() => service.GetAsync(burnModel.Id));

            Assert.Null(exception);

            _burnRuleRepositoryMock.
                Verify(r => r.GetAsync(It.IsAny<Guid>(), It.IsAny<bool>()), Times.Once);

            _fileServiceMock
                .Verify(c => c.GetAsync(Guid.Parse("26a88452-48ab-48eb-b9df-a22b281d1584")), Times.Once);
        }

        [Fact]
        public async Task Should_ThrowEntityNotFoundException_When_GetAsyncWithContentTypeUrlForPicture()
        {
            _burnRuleRepositoryMock.Setup(b => b.GetAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync(() => null);

            var service = new BurnRuleService(
                AssetName,
                _burnRuleRepositoryMock.Object,
                _burnRuleContentRepositoryMock.Object,
                _spendRuleChangeEventPublisher.Object,
                EmptyLogFactory.Instance,
                _fileServiceMock.Object,
                _burnRuleContentValidationMock.Object,
                _burnRulePartnerRepositoryMock.Object);

            var exception = await Record.ExceptionAsync(() => service.GetAsync(Guid.Parse("26a88452-48ab-48eb-b9df-a22b281d1584")));

            Assert.NotNull(exception);
            Assert.IsType<EntityNotFoundException>(exception);
            Assert.Equal("Burn rule with id 26a88452-48ab-48eb-b9df-a22b281d1584 does not exist.", exception.Message);
        }

        //SaveBurnRuleContentImage
        [Fact]
        public async Task Should_ThrowEntityNotFoundException_When_SaveBurnRuleContentImageWithNotExistingContentCalled()
        {
            _burnRuleRepositoryMock.Setup(b => b.GetAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync(() => null);

            var service = new BurnRuleService(
                AssetName,
                _burnRuleRepositoryMock.Object,
                _burnRuleContentRepositoryMock.Object,
                _spendRuleChangeEventPublisher.Object,
                EmptyLogFactory.Instance,
                _fileServiceMock.Object,
                _burnRuleContentValidationMock.Object,
                _burnRulePartnerRepositoryMock.Object);

            var exception = await Record.ExceptionAsync(() => service.SaveBurnRuleContentImage(new FileModel()
            {
                RuleContentId = Guid.Parse("26a88452-48ab-48eb-b9df-a22b281d1584")
            }));

            Assert.NotNull(exception);
            Assert.IsType<EntityNotFoundException>(exception);
            Assert.Equal("Burn Content type with id 26a88452-48ab-48eb-b9df-a22b281d1584 does not exist.", exception.Message);
        }

        [Fact]
        public async Task Should_ThrowRuleConditionNotFileException_When_SaveBurnRuleContentImageWithContentTypeNotPicture()
        {
            var fixture = new Fixture();

            var content = fixture.Build<BurnRuleContentModel>()
                .With(c => c.Id, Guid.Parse("26a88452-48ab-48eb-b9df-a22b281d1584"))
                .With(c => c.RuleContentType, RuleContentType.Description)
                .Create();

            _burnRuleContentRepositoryMock.Setup(b => b.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => content);

            var service = new BurnRuleService(
                AssetName,
                _burnRuleRepositoryMock.Object,
                _burnRuleContentRepositoryMock.Object,
                _spendRuleChangeEventPublisher.Object,
                EmptyLogFactory.Instance,
                _fileServiceMock.Object,
                _burnRuleContentValidationMock.Object,
                _burnRulePartnerRepositoryMock.Object);

            var exception = await Record.ExceptionAsync(() => service.SaveBurnRuleContentImage(new FileModel()
            {
                RuleContentId = Guid.Parse("26a88452-48ab-48eb-b9df-a22b281d1584")
            }));

            Assert.NotNull(exception);
            Assert.IsType<RuleConditionNotFileException>(exception);
            Assert.Equal("Burn Content type with id 26a88452-48ab-48eb-b9df-a22b281d1584 is not image type", exception.Message);
        }

        [Fact]
        public async Task Should_ThrowNotValidFormatFile_When_SaveBurnRuleContentImageWithInvalidFileType()
        {
            var fixture = new Fixture();

            var content = fixture.Build<BurnRuleContentModel>()
                .With(c => c.Id, Guid.Parse("26a88452-48ab-48eb-b9df-a22b281d1584"))
                .With(c => c.RuleContentType, RuleContentType.UrlForPicture)
                .Create();

            _burnRuleContentRepositoryMock.Setup(b => b.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => content);

            _fileServiceMock.Setup(f => f.SaveAsync(It.IsAny<FileModel>()))
                .ThrowsAsync(new NotValidFormatFile());

            var service = new BurnRuleService(
                AssetName,
                _burnRuleRepositoryMock.Object,
                _burnRuleContentRepositoryMock.Object,
                _spendRuleChangeEventPublisher.Object,
                EmptyLogFactory.Instance,
                _fileServiceMock.Object,
                _burnRuleContentValidationMock.Object,
                _burnRulePartnerRepositoryMock.Object);

            var exception = await Record.ExceptionAsync(() => service.SaveBurnRuleContentImage(new FileModel()
            {
                RuleContentId = Guid.Parse("26a88452-48ab-48eb-b9df-a22b281d1584"),
                Type = "wrong"
            }));

            Assert.NotNull(exception);
            Assert.IsType<NotValidFormatFile>(exception);
        }
    }
}
