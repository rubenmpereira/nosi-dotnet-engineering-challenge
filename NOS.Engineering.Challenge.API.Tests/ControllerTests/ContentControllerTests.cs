using Azure;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NOS.Engineering.Challenge.API.Controllers;
using NOS.Engineering.Challenge.API.Models;
using NOS.Engineering.Challenge.Database;
using NOS.Engineering.Challenge.Managers;
using NOS.Engineering.Challenge.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NOS.Engineering.Challenge.API.Tests.ControllerTests
{
    public class ContentControllerTests
    {
        private Content CreateEmptyContent()
        {
            return new Content(new(), "", "", "", "", 0, new(), new(), []);
        }

        #region GetManyContents

        [Fact]
        public async Task GetManyContents_CallsGetManyContents_OkObjectResult()
        {
            // Arrange
            var mockContentsManager = new Mock<IContentsManager>();
            var mockedData = new List<Content?>() { CreateEmptyContent() };

            mockContentsManager
                .Setup(handler => handler.GetManyContents())
                .Returns(Task.FromResult(mockedData.AsEnumerable()));

            var controller = new ContentController(mockContentsManager.Object);

            // Act
            var response = await controller.GetManyContents();

            // Assert
            var result = response as OkObjectResult;

            Assert.NotNull(response);
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task GetManyContents_CallsGetManyContents_NotFoundResult()
        {
            // Arrange
            var mockContentsManager = new Mock<IContentsManager>();
            var mockedData = new List<Content?>();

            mockContentsManager
                .Setup(handler => handler.GetManyContents())
                .Returns(Task.FromResult(mockedData.AsEnumerable()));

            var controller = new ContentController(mockContentsManager.Object);

            // Act
            var response = await controller.GetManyContents();

            // Assert
            var result = response as NotFoundResult;

            Assert.NotNull(response);
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        #endregion

        #region GetManyContentsFilter

        [Fact]
        public async Task GetManyContentsFilter_CallsGetManyContentsFilter_OkObjectResult()
        {
            // Arrange
            var mockContentsManager = new Mock<IContentsManager>();
            var mockedData = new List<Content?>() { CreateEmptyContent() };

            mockContentsManager
                .Setup(handler => handler.GetManyContentsFilter(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(mockedData.AsEnumerable()));

            var controller = new ContentController(mockContentsManager.Object);

            // Act
            var response = await controller.GetManyContentsFilter(new ContentFilterInput());

            // Assert
            var result = response as OkObjectResult;

            Assert.NotNull(response);
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task GetManyContentsFilter_CallsGetManyContentsFilter_NotFoundResult()
        {
            // Arrange
            var mockContentsManager = new Mock<IContentsManager>();
            var mockedData = new List<Content?>();

            mockContentsManager
                .Setup(handler => handler.GetManyContentsFilter(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(mockedData.AsEnumerable()));

            var controller = new ContentController(mockContentsManager.Object);

            // Act
            var response = await controller.GetManyContentsFilter(new ContentFilterInput());

            // Assert
            var okResult = response as NotFoundResult;

            Assert.NotNull(response);
            Assert.NotNull(okResult);
            Assert.Equal(404, okResult.StatusCode);
        }

        #endregion

        #region GetContent

        [Fact]
        public async Task GetContent_CallsGetContent_OkObjectResult()
        {
            // Arrange
            var mockContentsManager = new Mock<IContentsManager>();

            Content? mockedData = CreateEmptyContent();

            mockContentsManager
                .Setup(handler => handler.GetContent(It.IsAny<Guid>()))
                .Returns(Task.FromResult(mockedData ?? default));

            var controller = new ContentController(mockContentsManager.Object);

            // Act
            var response = await controller.GetContent(new Guid());

            // Assert
            var result = response as OkObjectResult;

            Assert.NotNull(response);
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task GetContent_CallsGetContent_NotFoundResult()
        {
            // Arrange
            var mockContentsManager = new Mock<IContentsManager>();

            mockContentsManager
                .Setup(handler => handler.GetContent(It.IsAny<Guid>()))
                .Returns(Task.FromResult((Content?)default));

            var controller = new ContentController(mockContentsManager.Object);

            // Act
            var response = await controller.GetContent(new Guid());

            // Assert
            var result = response as NotFoundResult;

            Assert.NotNull(response);
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        #endregion

        #region CreateContent

        [Fact]
        public async Task CreateContent_CallsCreateContent_OkObjectResult()
        {
            // Arrange
            var mockContentsManager = new Mock<IContentsManager>();

            Content? mockedData = CreateEmptyContent();

            mockContentsManager
                .Setup(handler => handler.CreateContent(It.IsAny<ContentDto>()))
                .Returns(Task.FromResult(mockedData ?? default));

            var controller = new ContentController(mockContentsManager.Object);

            // Act
            var response = await controller.CreateContent(new ContentInput());

            // Assert
            var result = response as OkObjectResult;

            Assert.NotNull(response);
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task CreateContent_CallsCreateContent_Problem()
        {
            // Arrange
            var mockContentsManager = new Mock<IContentsManager>();

            mockContentsManager
                .Setup(handler => handler.CreateContent(It.IsAny<ContentDto>()))
                .Returns(Task.FromResult((Content?)default));

            var controller = new ContentController(mockContentsManager.Object);

            // Act
            var response = await controller.CreateContent(new ContentInput());

            // Assert
            var result = response as ObjectResult;

            Assert.NotNull(response);
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
        }

        #endregion

        #region UpdateContent

        [Fact]
        public async Task UpdateContent_CallsUpdateContent_OkObjectResult()
        {
            // Arrange
            var mockContentsManager = new Mock<IContentsManager>();

            Content? mockedData = CreateEmptyContent();

            mockContentsManager
                .Setup(handler => handler.UpdateContent(It.IsAny<Guid>(), It.IsAny<ContentDto>()))
                .Returns(Task.FromResult(mockedData ?? default));

            var controller = new ContentController(mockContentsManager.Object);

            // Act
            var response = await controller.UpdateContent(new Guid(), new ContentInput());

            // Assert
            var result = response as OkObjectResult;

            Assert.NotNull(response);
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task UpdateContent_CallsUpdateContent_NotFound()
        {
            // Arrange
            var mockContentsManager = new Mock<IContentsManager>();

            mockContentsManager
                .Setup(handler => handler.UpdateContent(It.IsAny<Guid>(), It.IsAny<ContentDto>()))
                .Returns(Task.FromResult((Content?)default));

            var controller = new ContentController(mockContentsManager.Object);

            // Act
            var response = await controller.UpdateContent(new Guid(), new ContentInput());

            // Assert
            var result = response as NotFoundResult;

            Assert.NotNull(response);
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        #endregion

        #region DeleteContent

        [Fact]
        public async Task DeleteContent_CallsDeleteContent_OkObjectResult()
        {
            // Arrange
            var mockContentsManager = new Mock<IContentsManager>();

            mockContentsManager
                .Setup(handler => handler.DeleteContent(It.IsAny<Guid>()))
                .Returns(Task.FromResult(new Guid()));

            var controller = new ContentController(mockContentsManager.Object);

            // Act
            var response = await controller.DeleteContent(new Guid());

            // Assert
            var result = response as OkObjectResult;

            Assert.NotNull(response);
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        #endregion

        #region AddGenres

        [Fact]
        public async Task AddGenres_CallsAddGenres_OkObjectResult()
        {
            // Arrange
            var mockContentsManager = new Mock<IContentsManager>();

            Content? mockedData = CreateEmptyContent();

            mockContentsManager
                .Setup(handler => handler.AddGenres(It.IsAny<Guid>(), It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult(mockedData ?? default));

            var controller = new ContentController(mockContentsManager.Object);

            // Act
            var response = await controller.AddGenres(new Guid(), new List<string>());

            // Assert
            var result = response as OkObjectResult;

            Assert.NotNull(response);
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task AddGenres_CallsAddGenres_NotFound()
        {
            // Arrange
            var mockContentsManager = new Mock<IContentsManager>();

            mockContentsManager
                .Setup(handler => handler.AddGenres(It.IsAny<Guid>(), It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult((Content?)default));

            var controller = new ContentController(mockContentsManager.Object);

            // Act
            var response = await controller.AddGenres(new Guid(), new List<string>());

            // Assert
            var result = response as NotFoundResult;

            Assert.NotNull(response);
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        #endregion

        #region RemoveGenres

        [Fact]
        public async Task RemoveGenres_CallsRemoveGenres_OkObjectResult()
        {
            // Arrange
            var mockContentsManager = new Mock<IContentsManager>();

            Content? mockedData = CreateEmptyContent();

            mockContentsManager
                .Setup(handler => handler.RemoveGenres(It.IsAny<Guid>(), It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult(mockedData ?? default));

            var controller = new ContentController(mockContentsManager.Object);

            // Act
            var response = await controller.RemoveGenres(new Guid(), new List<string>());

            // Assert
            var result = response as OkObjectResult;

            Assert.NotNull(response);
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task RemoveGenres_CallsRemoveGenres_NotFound()
        {
            // Arrange
            var mockContentsManager = new Mock<IContentsManager>();

            mockContentsManager
                .Setup(handler => handler.RemoveGenres(It.IsAny<Guid>(), It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult((Content?)default));

            var controller = new ContentController(mockContentsManager.Object);

            // Act
            var response = await controller.RemoveGenres(new Guid(), new List<string>());

            // Assert
            var result = response as NotFoundResult;

            Assert.NotNull(response);
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        #endregion
    }
}
