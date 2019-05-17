using System.Collections.Generic;

namespace FileUploadApp.Domain
{
    public class UploadsContext
    {
        private readonly Queue<Upload> queue = new Queue<Upload>();

        public void Add(uint number, string name, string contentType, StreamAdapter streamAdapter)
        {
            Add(new Upload(
                        num: number,
                        name: name,
                        contentType: contentType,
                        height: 0,
                        width: 0,
                        streamAdapter: streamAdapter));
        }

        public void Add(Upload upload)
        {
            queue.Enqueue(upload);
        }

        public IEnumerable<Upload> YieldAll()
        {
            while (queue.Count > 0)
            {
                yield return queue.Dequeue();
            }
        }
    }
}
