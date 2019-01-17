using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Runpath.Api
{
    public interface IRunpathApplicationService
    {
        Task<IEnumerable<PhotoAlbumDto>> GetPhotoAlbumsAsync();

        Task<IEnumerable<PhotoAlbumDto>> GetPhotoAlbumsAsync(int userId);
    }
}