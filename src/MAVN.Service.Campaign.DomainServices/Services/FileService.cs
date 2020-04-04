using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using MAVN.Service.Campaign.Domain.Exceptions;
using MAVN.Service.Campaign.Domain.Models;
using MAVN.Service.Campaign.Domain.Repositories;
using MAVN.Service.Campaign.Domain.Services;
using MoreLinq;

namespace MAVN.Service.Campaign.DomainServices.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IFileInfoRepository _fileInfoRepository;
        private readonly ILog _log;

        private const string PngContentType = "image/png";
        private const string JpegContentType = "image/jpeg";
        private const string JpgContentType = "image/jpg";

        private static readonly string[] ImageContentTypes = { PngContentType, JpegContentType, JpgContentType };

        public FileService(IFileRepository fileRepository,
            IFileInfoRepository fileInfoRepository,
            ILogFactory logFactory)
        {
            _fileRepository = fileRepository ??
                throw new ArgumentNullException(nameof(fileRepository));
            _fileInfoRepository = fileInfoRepository ??
                throw new ArgumentNullException(nameof(fileInfoRepository));
            _log = logFactory.CreateLog(this);
        }

        public async Task<FileResponseModel> GetAsync(Guid ruleContentId)
        {
            var file = await _fileInfoRepository.GetAsync(ruleContentId.ToString());

            if (file == null)
            {
                return null;
            }

            var fileBlobUrl = await _fileRepository.GetBlobUrl(file.Name);

            return new FileResponseModel()
            {
                Id = file.Id,
                BlobUrl = fileBlobUrl,
                Type = file.Type,
                Name = file.Name,
                RuleContentId = file.RuleContentId
            };
        }

        public async Task<string> SaveAsync(FileModel file)
        {
            var shouldBeSaved = ImageContentTypes.Contains(file.Type);

            if (shouldBeSaved)
            {
                var info = await _fileInfoRepository.GetAsync(file.RuleContentId.ToString());

                file.Name = GenerateRuleContentImageFileName(file.Type);

                if (info != null)
                {
                    await _fileRepository.DeleteAsync(info.Name);
                    await _fileInfoRepository.DeleteAsync(info.RuleContentId.ToString());
                }

                await _fileInfoRepository.InsertAsync(file);

                await _fileRepository.InsertAsync(file.Content, file.Name);
            }
            else
            {
                _log.Warning($"{file.Type} is not supported file type");

                throw new NotValidFormatFile($"{file.Type} is not supported file type");
            }

            return await _fileRepository.GetBlobUrl(file.Name);
        }

        public async Task DeleteAsync(Guid ruleContentId)
        {
            var file = await _fileInfoRepository.GetAsync(ruleContentId.ToString());

            if (file != null)
            {
                await _fileInfoRepository.DeleteAsync(ruleContentId.ToString());
                await _fileRepository.DeleteAsync(file.Name);
            }
        }

        private string GenerateRuleContentImageFileName(string contentType)
        {
            string type = null;

            switch (contentType)
            {
                case PngContentType:
                    type = ".png";
                    break;
                case JpegContentType:
                    type = ".jpeg";
                    break;
                case JpgContentType:
                    type = ".jpg";
                    break;
            }

            var id = Guid.NewGuid().ToString();

            return $"{id}{type}";
        }
    }
}
