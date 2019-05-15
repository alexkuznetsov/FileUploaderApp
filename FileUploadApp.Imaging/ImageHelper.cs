using FileUploadApp.Domain;
using FileUploadApp.StreamAdapters;
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

        public static async Task<Image<Rgba32>> CreateImageAsync(Upload file)
        {
            var bytes = await file.Stream.AsRawBytesAsync()
                .ConfigureAwait(false);

            var image = Image.Load(bytes.ToArray());

            file.SetSize(
                height: (uint)image.Height,
                width: (uint)image.Width);

            return image;
        }

        public static Upload Resize(Upload file, Image<Rgba32> original, System.Drawing.Size size, string mime)
        {
            original.Mutate(x => x
                .Resize(new ResizeOptions
                {
                    Size = new SixLabors.Primitives.Size(size.Width, size.Height),
                    Mode = ResizeMode.Pad
                }));

            using (var s = new MemoryStream())
            {
                var newHeight = (uint)original.Height;
                var newWidth = (uint)original.Width;

                original.SaveAsJpeg(s);

                return new Upload(
                    num: file.Number,
                    name: PreviewPrefix + file.Name,
                    contentType: mime,
                    width: newWidth,
                    height: newHeight,
                    streamAdapter: new ByteaStreamAdapter(s.ToArray()));
            }

        }
    }
}
