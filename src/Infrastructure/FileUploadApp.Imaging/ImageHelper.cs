using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;

namespace FileUploadApp.Imaging
{
    public static class ImageHelper
    {
        public static MemoryStream Resize(System.Drawing.Size size
            , Stream fileStream
            , string previewContentType)
        {
            using var image = Image.Load(fileStream);
            image.Mutate(x => x
                .Resize(new ResizeOptions
                {
                    Size = new Size(size.Width, size.Height),
                    Mode = ResizeMode.Pad
                }));

            var s = new MemoryStream();

            var saveMethod = ResolveSaveMethod(previewContentType, image);

            saveMethod(s);

            return s;
        }


        private static Action<MemoryStream> ResolveSaveMethod(string contentType, Image image)
        {
            return contentType switch
            {
                "image/png" => image.SaveAsPng,
                "image/jpg" => image.SaveAsJpeg,
                "image/webp" => image.SaveAsWebp,
                _ => image.SaveAsJpeg
            };
        }
    }
}
