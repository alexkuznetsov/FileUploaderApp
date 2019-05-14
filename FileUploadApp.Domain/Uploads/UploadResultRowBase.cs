namespace FileUploadApp.Domain
{
    public class UploadResultRowBase
    {
        public string Id { get; set; }

        public uint Number { get; set; }

        public string Name { get; set; }

        public string ContentType { get; set; }

        public uint Width { get; set; }

        public uint Height { get; set; }
    }
}
