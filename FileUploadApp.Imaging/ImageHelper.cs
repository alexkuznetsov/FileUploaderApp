using FileUploadApp.Domain;
using FileUploadApp.StreamAdapters;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Imaging
{
    public class ImageHelper : IDisposable
    {
        private bool disposed;

        private readonly Upload file;
        private readonly Image<Rgba32> image;

        public static async Task<ImageHelper> FromUploadAsync(Upload file, CancellationToken cancellationToken = default)
        {
            var bytes = await file.Stream.AsRawBytesAsync(cancellationToken)
                .ConfigureAwait(false);

            var image = Image.Load(bytes.ToArray());

            return new ImageHelper(file, image);
        }

        private ImageHelper(Upload file, Image<Rgba32> image)
        {
            this.file = file;
            this.image = image;
        }

        ~ImageHelper()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                if (image != null)
                    image.Dispose();

                disposed = true;
            }
        }

        public Upload Resize(System.Drawing.Size size)
        {
            image.Mutate(x => x
                .Resize(new ResizeOptions
                {
                    Size = new SixLabors.Primitives.Size(size.Width, size.Height),
                    Mode = ResizeMode.Pad
                }));

            using (var s = new MemoryStream())
            {
                image.SaveAsJpeg(s);

                return new Upload(
                        id: file.PreviewId
                      , previewId: Guid.Empty
                      , num: file.Number
                      , name: Upload.PreviewPrefix + file.Name
                      , contentType: file.ContentType
                      , streamAdapter: new ByteaStreamAdapter(s.ToArray()));
            }

        }
    }
}
