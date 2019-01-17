using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Runpath.Api;
using Xunit;

namespace Runpath.Tests
{
    public class RunpathApiIntegrationTests : IDisposable
    {
        private readonly HttpClient sut;

        public RunpathApiIntegrationTests()
        {
            var webhost = WebHost.CreateDefaultBuilder()
                                    .UseStartup<Startup>();

            sut = new TestServer(webhost).CreateClient();
        }

        //  Based off doc's from 
        //  http://jsonplaceholder.typicode.com/

        //  Resources
        //  JSONPlaceholder comes with a set of 6 common resources:
        //  ...
        //  /albums	100 albums
        [Theory]
        [InlineData("/api/photoalbums")]
        public async Task ShouldReturn100PhotoAlbumsOnGET(string uri)
        {
            // Arrange
            var fullUri = $"{uri}";

            //Act
            var response = await this.sut.GetAsync($"{uri}");
            var responseAsDto = await response.Content.ReadAsAsync<IEnumerable<PhotoAlbumDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(100, responseAsDto.Count());
        }


        //  Based off doc's from 
        //  http://jsonplaceholder.typicode.com/

        //Briefly looking at the API results, it was observed
        //that user with Id 1 had at least two albums associated with it.
        [Theory]
        [InlineData("/api/photoalbums", 1)]
        public async Task ShouldReturnAtLeastTwoPhotoAlbumsForUserId1OnGET(string uri, int userId)
        {
            // Arrange
            var fullUri = $"{uri}?userId={userId}";

            //Act
            var response = await this.sut.GetAsync(fullUri);
            var responseAsDto = await response.Content.ReadAsAsync<IEnumerable<PhotoAlbumDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(responseAsDto.Count() > 1);
        }

        public void Dispose()
        {
            this.sut.Dispose();
        }
    }
}
