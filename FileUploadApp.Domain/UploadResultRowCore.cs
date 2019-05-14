namespace FileUploadApp.Domain
{
    public class UploadResultRowCore
    {
        public string Id { get; set; }

        public string Path { get; set; }

        public string ContentType { get; set; }

        public uint Width { get; set; }

        public uint Height { get; set; }
    }
}
