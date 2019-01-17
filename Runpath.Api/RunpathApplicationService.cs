using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Runpath.Api
{
    public class RunpathApplicationService : IRunpathApplicationService
    {
        private readonly IJsonPlaceholderClient jsonPlaceholderClient;

        public RunpathApplicationService(IJsonPlaceholderClient jsonPlaceholderClient)
        {
            this.jsonPlaceholderClient = jsonPlaceholderClient;
        }

        public async Task<IEnumerable<PhotoAlbumDto>> GetPhotoAlbumsAsync()
        {
            return await GetPhotoAlbumsCoreAsync(o => true);
        }

        public async Task<IEnumerable<PhotoAlbumDto>> GetPhotoAlbumsAsync(int userId)
        {
            return await GetPhotoAlbumsCoreAsync(o => o.UserId == userId);
        }

        private async Task<IEnumerable<PhotoAlbumDto>> GetPhotoAlbumsCoreAsync(Func<JsonPlaceholderAlbum, bool> predicate)
        {
            var jsonAlbumsTask = this.jsonPlaceholderClient.GetAlbumsAsync(); //kick off tasks in parallel

            var jsonPhotosTask = this.jsonPlaceholderClient.GetPhotosAsync(); //kick off tasks in parallel

            await Task.WhenAll(jsonAlbumsTask, jsonPhotosTask);
            
            //---------  all tasks completed ---------//
            
            var jsonAlbums = await jsonAlbumsTask;

            var jsonPhotos = await jsonPhotosTask;

            var albums = new List<PhotoAlbumDto>();

            //filter out for userId here to save unnessary creation of new PhotoAlbumDto's (ones that don't pertain to user) + photolookup.
            foreach (var jsonAlbum in jsonAlbums.Where(predicate))
            {
                var album = new PhotoAlbumDto
                {
                    AlbumId = jsonAlbum.Id,
                    UserId = jsonAlbum.UserId,
                    Title = jsonAlbum.Title,
                    Photos = jsonPhotos.Where(photo => photo.AlbumId == jsonAlbum.Id).Select(o => new PhotoDto
                    {
                        AlbumId = o.AlbumId,
                        PhotoId = o.Id,
                        ThumbnailUrl = o.ThumbnailUrl,
                        Title = o.Title,
                        Url = o.Url
                    }).ToList()
                };

                albums.Add(album);
            }

            return albums;
        }
    }
}
