using FileUploadApp.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApp.Handlers
{
    public class FileAsUriDataPayloadHandler : IFileDataPayloadHandler<string[]>
    {
        private readonly IDownloadHelper downloadHelper;

        public FileAsUriDataPayloadHandler(IDownloadHelper downloadHelper)
        {
            this.downloadHelper = downloadHelper;
        }

        public Task ApplyAsync(string[] links)
        {
            return LoadImagesFromRemoteAsync(links);
        }

        private Task LoadImagesFromRemoteAsync(string[] links)
        {
            var tasks = links.AsUriEnumerable()
                         .Select(downloadHelper.Download)
                         .ToList();

            return Task.WhenAll(tasks);
        }
    }
}
