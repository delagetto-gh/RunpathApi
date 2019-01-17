using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Runpath.Api
{
    public class JsonPlaceholderClient : BaseHttpClient, IJsonPlaceholderClient
    {
        public JsonPlaceholderClient() : base("http://jsonplaceholder.typicode.com")
        {
        }

        public async Task<IEnumerable<JsonPlaceholderPhoto>> GetPhotosAsync()
        {
            IEnumerable<JsonPlaceholderPhoto> responseDto = null;

            var response = await this.Get("photos");

            responseDto = await response.Content.ReadAsAsync<IEnumerable<JsonPlaceholderPhoto>>();

            return responseDto;
        }

        public async Task<IEnumerable<JsonPlaceholderAlbum>> GetAlbumsAsync()
        {
            IEnumerable<JsonPlaceholderAlbum> responseDto = null;

            var response = await this.Get("albums");

            responseDto = await response.Content.ReadAsAsync<IEnumerable<JsonPlaceholderAlbum>>();

            return responseDto;
        }
    }
}
