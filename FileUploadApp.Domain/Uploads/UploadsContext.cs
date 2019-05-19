using System;
using System.Collections.Generic;

namespace FileUploadApp.Domain
{
    public class UploadsContext
    {
        private readonly Queue<Upload> queue = new Queue<Upload>();

        public void Add(Guid id, Guid previewId, uint number, string name, string contentType, StreamAdapter streamAdapter)
        {
            Add(new Upload(
                        id: id,
                        previewId: previewId,
                        num: number,
                        name: name,
                        contentType: contentType,
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
