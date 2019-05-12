using FileUploadApp.Core.Streams;
using FileUploadApp.Domain;
using FileUploadApp.Interfaces;
using System;
using System.Threading.Tasks;

namespace FileUploadApp.Handlers
{
    public class FileAsBase64DataPayloadHadnler : IFileDataPayloadHandler<FileAsBase64Payload[]>
    {
        private readonly IContentTypeTestUtility contentTypeTestUtility;
        private readonly IUploadService uploadService;

        public FileAsBase64DataPayloadHadnler(IContentTypeTestUtility contentTypeTestUtility,
                                         IUploadService uploadService)
        {
            this.contentTypeTestUtility = contentTypeTestUtility;
            this.uploadService = uploadService;
        }

        public Task ApplyAsync(FileAsBase64Payload[] files)
        {
            LoadImagesFromBase64(files);

            return Task.FromResult(0);
        }

        private void LoadImagesFromBase64(FileAsBase64Payload[] files)
        {
            foreach (var f in files)
            {
                var contentType = contentTypeTestUtility.DetectContentType(f.Base64);

                if (contentTypeTestUtility.IsAllowed(contentType))
                {
                    var readOnlyMemory = new ReadOnlyMemory<byte>(Convert.FromBase64String(f.Base64));

                    uploadService.UploadedFiles.Add(new UploadedFile(
                        name: f.Name,
                        contentType: contentType,
                        height:0,
                        width:0,
                        streamWrapper: new ByteaStreamWrapper(readOnlyMemory)
                        )
                    );
                }

            }
        }
    }
}
