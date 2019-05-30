using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.IO;

namespace FileUploadApp.Imaging
{
    public static class ImageHelper
    {
        public static byte[] Resize(System.Drawing.Size size
            , Stream fileStream)
        {
            using (var image = Image.Load(fileStream))
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

                    return s.GetBuffer();
                }
            }
        }
    }
}
