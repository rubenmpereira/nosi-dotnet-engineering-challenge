using Moq;
using NOS.Engineering.Challenge.Database;
using NOS.Engineering.Challenge.Managers;
using NOS.Engineering.Challenge.Models;

namespace NOS.Engineering.Challenge.Tests.ManagerTests
{
    public class ContentManagerTests
    {
        private Content CreateEmptyContent()
        {
            return new Content(new(), "", "", "", "", 0, new(), new(), []);
        }

        #region GetManyContents

        [Fact]
        public async Task GetManyContents_CallsReadAll()
        {
            // Arrange
            var mockDatabase = new Mock<IDatabase<Content?, ContentDto>>();
            var mockedData = new List<Content?>() { CreateEmptyContent() };

            mockDatabase
                .Setup(handler => handler.ReadAll())
                .Returns(Task.FromResult(mockedData.AsEnumerable()));

            var manager = new ContentsManager(mockDatabase.Object);

            // Act
            var response = await manager.GetManyContents();

            // Assert
            mockDatabase.Verify(mock => mock.ReadAll(), Times.Once);
            Assert.Equal(response, mockedData);
        }

        #endregion

        #region GetManyContentsFilter

        [Fact]
        public async Task GetManyContentsFilter_CallsReadAll()
        {
            // Arrange
            var mockDatabase = new Mock<IDatabase<Content?, ContentDto>>();
            var mockedData = new List<Content?>() { CreateEmptyContent() };

            mockDatabase
                .Setup(handler => handler.ReadAll())
                .Returns(Task.FromResult(mockedData.AsEnumerable()));

            var manager = new ContentsManager(mockDatabase.Object);

            // Act
            var response = await manager.GetManyContentsFilter("", "");

            // Assert
            mockDatabase.Verify(mock => mock.ReadAll(), Times.Once);
            Assert.Equal(response, mockedData);
        }

        [Fact]
        public async Task GetManyContentsFilter_FiltersReadAll()
        {
            // Arrange
            var mockDatabase = new Mock<IDatabase<Content?, ContentDto>>();
            var mockedData = new List<Content?>() { CreateEmptyContent() };

            mockDatabase
                .Setup(handler => handler.ReadAll())
                .Returns(Task.FromResult(mockedData.AsEnumerable()));

            var manager = new ContentsManager(mockDatabase.Object);

            // Act
            var response = await manager.GetManyContentsFilter("a", "");

            // Assert
            Assert.Equal(response, new List<Content?>());
        }

        #endregion

        #region GetContent

        [Fact]
        public async Task GetContent_CallsRead()
        {
            // Arrange
            var mockDatabase = new Mock<IDatabase<Content?, ContentDto>>();
            Content? mockedData = CreateEmptyContent();

            mockDatabase
                .Setup(handler => handler.Read(It.IsAny<Guid>()))
                .Returns(Task.FromResult(mockedData ?? default));

            var manager = new ContentsManager(mockDatabase.Object);

            // Act
            var response = await manager.GetContent(new Guid());

            // Assert
            mockDatabase.Verify(mock => mock.Read(It.IsAny<Guid>()), Times.Once);
            Assert.Equal(response, mockedData);
        }

        #endregion

        #region CreateContent

        [Fact]
        public async Task CreateContent_CallsCreate()
        {
            // Arrange
            var mockDatabase = new Mock<IDatabase<Content?, ContentDto>>();
            Content? mockedData = CreateEmptyContent();

            mockDatabase
                .Setup(handler => handler.Create(It.IsAny<ContentDto>()))
                .Returns(Task.FromResult(mockedData ?? default));

            var manager = new ContentsManager(mockDatabase.Object);

            // Act
            var response = await manager.CreateContent(new ContentDto(new List<string>()));

            // Assert
            mockDatabase.Verify(mock => mock.Create(It.IsAny<ContentDto>()), Times.Once);
            Assert.Equal(response, mockedData);
        }

        #endregion

        #region UpdateContent

        [Fact]
        public async Task UpdateContent_CallsUpdate()
        {
            // Arrange
            var mockDatabase = new Mock<IDatabase<Content?, ContentDto>>();
            Content? mockedData = CreateEmptyContent();

            mockDatabase
                .Setup(handler => handler.Update(It.IsAny<Guid>(), It.IsAny<ContentDto>()))
                .Returns(Task.FromResult(mockedData ?? default));

            var manager = new ContentsManager(mockDatabase.Object);

            // Act
            var response = await manager.UpdateContent(new Guid(), new ContentDto(new List<string>()));

            // Assert
            mockDatabase.Verify(mock => mock.Update(It.IsAny<Guid>(), It.IsAny<ContentDto>()), Times.Once);
            Assert.Equal(response, mockedData);
        }

        #endregion

        #region DeleteContent

        [Fact]
        public async Task DeleteContent_CallsDelete()
        {
            // Arrange
            var mockDatabase = new Mock<IDatabase<Content?, ContentDto>>();
            var mockedData = new Guid();

            mockDatabase
                .Setup(handler => handler.Delete(It.IsAny<Guid>()))
                .Returns(Task.FromResult(mockedData));

            var manager = new ContentsManager(mockDatabase.Object);

            // Act
            var response = await manager.DeleteContent(new Guid());

            // Assert
            mockDatabase.Verify(mock => mock.Delete(It.IsAny<Guid>()), Times.Once);
            Assert.Equal(response, mockedData);
        }

        #endregion

        #region AddGenres

        [Fact]
        public async Task AddGenres_CallsUpdate()
        {
            // Arrange
            var mockDatabase = new Mock<IDatabase<Content?, ContentDto>>();
            Content? mockedData = CreateEmptyContent();

            mockDatabase
                .Setup(handler => handler.Read(It.IsAny<Guid>()))
                .Returns(Task.FromResult(mockedData ?? default));

            mockDatabase
                .Setup(handler => handler.Update(It.IsAny<Guid>(), It.IsAny<ContentDto>()))
                .Returns(Task.FromResult(mockedData ?? default));

            var manager = new ContentsManager(mockDatabase.Object);

            // Act
            var response = await manager.AddGenres(new Guid(), new List<string>());

            // Assert
            mockDatabase.Verify(mock => mock.Update(It.IsAny<Guid>(), It.IsAny<ContentDto>()), Times.Once);
            Assert.Equal(response, mockedData);
        }

        #endregion

        #region RemoveGenres

        [Fact]
        public async Task RemoveGenres_CallsUpdate()
        {
            // Arrange
            var mockDatabase = new Mock<IDatabase<Content?, ContentDto>>();
            Content? mockedData = CreateEmptyContent();

            mockDatabase
                .Setup(handler => handler.Read(It.IsAny<Guid>()))
                .Returns(Task.FromResult(mockedData ?? default));

            mockDatabase
                .Setup(handler => handler.Update(It.IsAny<Guid>(), It.IsAny<ContentDto>()))
                .Returns(Task.FromResult(mockedData ?? default));

            var manager = new ContentsManager(mockDatabase.Object);

            // Act
            var response = await manager.RemoveGenres(new Guid(), new List<string>());

            // Assert
            mockDatabase.Verify(mock => mock.Read(It.IsAny<Guid>()), Times.Once);
            mockDatabase.Verify(mock => mock.Update(It.IsAny<Guid>(), It.IsAny<ContentDto>()), Times.Once);
            Assert.Equal(response, mockedData);
        }

        #endregion

    }
}
