using System.Collections.Generic;

namespace FileUploadApp.Domain
{
    public class UploadedFilesContext
    {
        private readonly Queue<UploadedFile> queue = new Queue<UploadedFile>();

        public void Add(uint number, string name, string contentType, StreamWrapper streamWrapper)
        {
            queue.Enqueue(new UploadedFile(
                        num: number,
                        name: name,
                        contentType: contentType,
                        height: 0,
                        width: 0,
                        streamWrapper: streamWrapper));
        }

        public IEnumerable<UploadedFile> GetList()
        {
            while (queue.Count > 0)
            {
                yield return queue.Dequeue();
            }
        }
    }
}
