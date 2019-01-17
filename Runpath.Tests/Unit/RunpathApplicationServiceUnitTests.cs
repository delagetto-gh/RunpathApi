using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Runpath.Api;
using Xunit;

namespace Runpath.Tests
{
    public class RunpathApplicationServiceUnitTests
    {
        private readonly Mock<IJsonPlaceholderClient> jsonPlaceHolderApiClient;

        public RunpathApplicationServiceUnitTests()
        {
            this.jsonPlaceHolderApiClient = new Mock<IJsonPlaceholderClient>();

            jsonPlaceHolderApiClient.Setup(o => o.GetAlbumsAsync())
                                    .Returns(Task.FromResult<IEnumerable<JsonPlaceholderAlbum>>(new List<JsonPlaceholderAlbum>
                                    {
                                        new JsonPlaceholderAlbum{ Id = 1, UserId = 1337, Title = "Test Album"},
                                        new JsonPlaceholderAlbum{ Id = 3, UserId = 1234, Title = "Another Test Album"},
                                        new JsonPlaceholderAlbum{ Id = 9, UserId = 5678, Title = "Yet Another Test Album"},
                                    }));

            jsonPlaceHolderApiClient.Setup(o => o.GetPhotosAsync())
                                    .Returns(Task.FromResult<IEnumerable<JsonPlaceholderPhoto>>(new List<JsonPlaceholderPhoto>
                                    {
                                        new JsonPlaceholderPhoto{Id = 1, AlbumId = 1, Title= "Test Photo 1" ,Url= "http://test1", ThumbnailUrl ="http://testUrl1" },
                                        new JsonPlaceholderPhoto{Id = 2, AlbumId = 1, Title= "Test Photo 2", Url= "http://test2", ThumbnailUrl ="http://testUrl2" },
                                        new JsonPlaceholderPhoto{Id = 99, AlbumId = 3, Title= "A Random Test Photo", Url= "http://random", ThumbnailUrl ="http://random" },
                                    }));
        }

        [Fact]
        public async Task ShouldReturnAllPhotoAlbums()
        {
            //arrange
            var sut = new RunpathApplicationService(jsonPlaceHolderApiClient.Object);

            //act
            var result = await sut.GetPhotoAlbumsAsync();

            //assert
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task ShouldContainCorrectAlbumDetails()
        {
            //arrange
            var testAlbumId = 1;

            var sut = new RunpathApplicationService(jsonPlaceHolderApiClient.Object);

            //act
            var result = (await sut.GetPhotoAlbumsAsync()).SingleOrDefault(pa => pa.AlbumId == testAlbumId);

            //assert
            Assert.Equal(1, result.AlbumId);
            Assert.Equal(1337, result.UserId);
            Assert.Equal("Test Album", result.Title);
            Assert.Equal(2, result.Photos.Count);
        }

        [Fact]
        public async Task ShouldContainCorrectPhotosDetails()
        {
            //arrange
            var testAlbumId = 1;

            var sut = new RunpathApplicationService(jsonPlaceHolderApiClient.Object);

            //act
            var result = (await sut.GetPhotoAlbumsAsync()).SingleOrDefault(pa => pa.AlbumId == testAlbumId);

            //assert
            Assert.Equal(1, result.Photos[0].PhotoId);
            Assert.Equal(1, result.Photos[0].AlbumId);
            Assert.Equal("Test Photo 1", result.Photos[0].Title);
            Assert.Equal("http://test1", result.Photos[0].Url);
            Assert.Equal("http://testUrl1", result.Photos[0].ThumbnailUrl);

            Assert.Equal(2, result.Photos[1].PhotoId);
            Assert.Equal(1, result.Photos[1].AlbumId);
            Assert.Equal("Test Photo 2", result.Photos[1].Title);
            Assert.Equal("http://test2", result.Photos[1].Url);
            Assert.Equal("http://testUrl2", result.Photos[1].ThumbnailUrl);
        }

        [Theory]
        [InlineData(1337)]
        public async Task ShouldReturnCorrectNumberOfPhotoAlbumsForGivenUser(int userId)
        {
            //arrange
            var sut = new RunpathApplicationService(jsonPlaceHolderApiClient.Object);

            //act
            var result = await sut.GetPhotoAlbumsAsync(userId);

            //assert
            Assert.Equal(1, result.Count());
        }

        [Theory]
        [InlineData(1337)]
        public async Task ShouldReturnPhotoAlbumsForGivenUser(int userId)
        {
            //arrange
            var sut = new RunpathApplicationService(jsonPlaceHolderApiClient.Object);

            //act
            var result = await sut.GetPhotoAlbumsAsync(userId);

            //assert
            Assert.All(result, photoAlbum => Assert.Equal(userId, photoAlbum.UserId));
        }
    }
}
