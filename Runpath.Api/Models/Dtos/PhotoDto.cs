using System.Collections.Generic;

namespace Runpath.Api
{
    public class PhotoDto
    {
        public int PhotoId { get; set; }
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
    }
}