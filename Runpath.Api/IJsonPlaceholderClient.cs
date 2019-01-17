using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Runpath.Api
{
    public interface IJsonPlaceholderClient : IDisposable
    {
        Task<IEnumerable<JsonPlaceholderPhoto>> GetPhotosAsync();

        Task<IEnumerable<JsonPlaceholderAlbum>> GetAlbumsAsync();
    }
}