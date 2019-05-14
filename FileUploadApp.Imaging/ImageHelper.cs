using FileUploadApp.StreamWrappers;
using FileUploadApp.Domain;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IO;
using System.Threading.Tasks;

namespace FileUploadApp.Imaging
{
    public class ImageHelper
    {
        private static readonly string PreviewPrefix = "p_";

        public static async Task<Image<Rgba32>> CreateImageAsync(UploadedFile file)
        {
            var bytes = await file.Stream.AsRawBytesAsync()
                .ConfigureAwait(false);

            var image = Image.Load(bytes);

            file.SetSize(
                height: (uint)image.Height,
                width: (uint)image.Width);

            return image;
        }

        public static UploadedFile Resize(UploadedFile file, Image<Rgba32> original, int width, int height, string mime)
        {
            original.Mutate(x => x
                .Resize(new ResizeOptions
                {
                    Size = new SixLabors.Primitives.Size(width, height),
                    Mode = ResizeMode.Pad
                }));

            using (var s = new MemoryStream())
            {
                var newHeight = (uint)original.Height;
                var newWidth = (uint)original.Width;

                original.SaveAsJpeg(s);

                return new UploadedFile(
                    num: file.Number,
                    name: PreviewPrefix + file.Name,
                    contentType: mime,
                    width: newWidth,
                    height: newHeight,
                    streamWrapper: new ByteaStreamWrapper(s.ToArray()));
            }

        }
    }
}
