﻿using FileUploadApp.Core.Mvc;
using FileUploadApp.Features.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : BaseApiController
    {
        public FileController(IMediator mediator) : base(mediator)
        {

        }

        [HttpGet("{id}")]
        [ResponseCache(Duration = 5, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken = default)
        {
            var response = await SendAsync(new DownloadUploadById.Query(id), cancellationToken);

            if (response == null)
                return NotFound();

            return File(response.Stream.Stream, response.ContentType);
        }
    }
}
