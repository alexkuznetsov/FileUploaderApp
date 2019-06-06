using FileUploadApp.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FileUploadApp.Requests
{
    public class GetUploadedResultsQuery : IRequest<UploadResult>
    {
        public GetUploadedResultsQuery(IEnumerable<Upload> uploads)
        {
            Ids = uploads
                .Select(x => new Tuple<Guid, Guid>(x.Id, x.PreviewId))
                .ToArray();
        }

        public Tuple<Guid, Guid>[] Ids { get; }
    }
}
