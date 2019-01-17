using System.Collections.Generic;

namespace Runpath.Api
{
    public class PhotoAlbumDto
    {
        public int AlbumId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public List<PhotoDto> Photos { get; set; }
    }
}