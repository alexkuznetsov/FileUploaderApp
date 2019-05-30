using FileUploadApp.Domain;
using FileUploadApp.StreamAdapters;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FileUploadApp.Imaging
{
    public class ImageHelper
    {
        public static async Task<Upload> Resize(System.Drawing.Size size, Upload file)
        {
            using (var stream = new MemoryStream())
            {
                await file.Stream.CopyToAsync(stream);

                stream.Seek(0, SeekOrigin.Begin);

                using (var image = Image.Load(stream))
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
    }
}
