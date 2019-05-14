using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileUploadApp.Commands;
using FileUploadApp.Domain;
using FileUploadApp.Interfaces;
using FileUploadApp.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileUploadApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly UploadedFilesContext uploadedFilesContext;
        private readonly IMediator mediator;

        public UploadController(UploadedFilesContext uploadedFilesContext, IMediator mediator)
        {
            this.uploadedFilesContext = uploadedFilesContext;
            this.mediator = mediator;
        }

        [HttpPost("")]
        public async Task<IActionResult> Post()
        {
            var saveFilesResult = await mediator.Send(new UploadFilesCommand(uploadedFilesContext.GetList()));
            var allFileNames = saveFilesResult.Result.Select(x => 
                new { UploadedFileId = x.Id, UploadedFilePreviewId = x.Preview.Id  })
                .ToArray();

            return Ok(allFileNames);
        }
    }
}
