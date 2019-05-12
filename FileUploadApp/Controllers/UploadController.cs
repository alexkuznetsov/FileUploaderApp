using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileUploadApp.Interfaces;
using FileUploadApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileUploadApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IUploadService uploadService;

        public UploadController(IUploadService uploadService)
        {
            this.uploadService = uploadService;
        }

        [HttpPost("")]
        public IActionResult Post()
        {
            var allFileNames = uploadService.UploadedFiles.Select(x => x.Name)
                .ToArray();

            return Ok(allFileNames);
        }
    }
}
