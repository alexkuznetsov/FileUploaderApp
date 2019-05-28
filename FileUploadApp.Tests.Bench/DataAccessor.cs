namespace FileUploadApp.Tests.Bench
{
    internal static class DataAccessor
    {
        public static string ImageBase64()
        {
            using (var str = typeof(DataAccessor).Assembly.GetManifestResourceStream("FileUploadApp.Tests.Bench.IMG_0162.JPG.txt"))
            using (var reader = new System.IO.StreamReader(str))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
