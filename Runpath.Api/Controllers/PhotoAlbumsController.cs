using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Runpath.Api.Controllers
{
    [Route("api/[controller]")]
    public class PhotoAlbumsController : ControllerBase
    {
        private readonly IRunpathApplicationService runpathSvc;

        public PhotoAlbumsController(IRunpathApplicationService runpathSvc)
        {
            this.runpathSvc = runpathSvc;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int? userId)
        {
            try
            {
                IEnumerable<PhotoAlbumDto> response;

                if (userId.HasValue)
                    response = await this.runpathSvc.GetPhotoAlbumsAsync(userId.Value);
                else
                    response = await this.runpathSvc.GetPhotoAlbumsAsync();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
