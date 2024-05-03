using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using NOS.Engineering.Challenge.Database;
using NOS.Engineering.Challenge.Managers;
using NOS.Engineering.Challenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOS.Engineering.Challenge.Tests.DatabaseTests
{
    public class EntityFrameworkDatabaseTests
    {
        private Content CreateEmptyContent()
        {
            return new Content(new(), "", "", "", "", 0, new(), new(), []);
        }

        [Fact]
        public async Task Create_CallsAdd()
        {
            // Arrange
            var mockDbContextFactory = new Mock<IDbContextFactory<ContentDbContext>>();

            var mockDbContext = new Mock<ContentDbContext>();

            mockDbContextFactory
                .Setup(handler => handler.CreateDbContext())
                .Returns(mockDbContext.Object);

            var mockedData = CreateEmptyContent();

            var mockMapper = new Mock<IMapper<Content?, ContentDto>>();
            mockMapper
                .Setup(handler => handler.Map(It.IsAny<Guid>(), It.IsAny<ContentDto>()))
                .Returns(mockedData);

            var database = new EntityFrameworkDatabase<Content, ContentDto>(mockMapper.Object, mockDbContextFactory.Object);

            // Act
            var response = await database.Create(new ContentDto(new List<string>()));

            // Assert
            mockMapper.Verify(mock => mock.Map(It.IsAny<Guid>(), It.IsAny<ContentDto>()), Times.Once);
            mockDbContext.Verify(mock => mock.AddAsync(It.IsAny<Content>(), It.IsAny<CancellationToken>()), Times.Once);
            mockDbContext.Verify(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(response, mockedData);
        }

        [Fact]
        public async Task Read_CallsFind()
        {
            // Arrange
            var mockDbContextFactory = new Mock<IDbContextFactory<ContentDbContext>>();
            var mockDbContext = new Mock<ContentDbContext>();

            var mockedData = CreateEmptyContent();

            mockDbContext
                .Setup(handler => handler.FindAsync<Content>(It.IsAny<Guid>()))
                .ReturnsAsync(mockedData);

            mockDbContextFactory
                .Setup(handler => handler.CreateDbContext())
                .Returns(mockDbContext.Object);

            var mockMapper = new Mock<IMapper<Content?, ContentDto>>();

            var database = new EntityFrameworkDatabase<Content, ContentDto>(mockMapper.Object, mockDbContextFactory.Object);

            // Act
            var response = await database.Read(new Guid());

            // Assert
            mockDbContext.Verify(mock => mock.FindAsync<Content>(It.IsAny<Guid>()), Times.Once);
            Assert.Equal(response, mockedData);
        }

        [Fact]
        public async Task Update_CallsFind()
        {
            // Arrange
            var mockDbContextFactory = new Mock<IDbContextFactory<ContentDbContext>>();

            var mockDbContext = new Mock<ContentDbContext>();

            var mockedData = CreateEmptyContent();

            mockDbContext
                .Setup(handler => handler.FindAsync<Content>(It.IsAny<Guid>()))
                .ReturnsAsync(mockedData);

            mockDbContextFactory
                .Setup(handler => handler.CreateDbContext())
                .Returns(mockDbContext.Object);

            var mockMapper = new Mock<IMapper<Content?, ContentDto>>();
            mockMapper
                .Setup(handler => handler.Patch(It.IsAny<Content>(), It.IsAny<ContentDto>()))
                .Returns(mockedData);

            var database = new EntityFrameworkDatabase<Content, ContentDto>(mockMapper.Object, mockDbContextFactory.Object);

            // Act
            var response = await database.Update(new Guid(), new ContentDto(new List<string>()));

            // Assert
            mockDbContext.Verify(mock => mock.FindAsync<Content>(It.IsAny<Guid>()), Times.Once);
            mockMapper.Verify(mock => mock.Patch(It.IsAny<Content>(), It.IsAny<ContentDto>()), Times.Once);
            mockDbContext.Verify(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(response, mockedData);
        }

        [Fact]
        public async Task Delete_CallsRemove()
        {
            // Arrange
            var mockDbContextFactory = new Mock<IDbContextFactory<ContentDbContext>>();

            var mockDbContext = new Mock<ContentDbContext>();

            var mockedData = CreateEmptyContent();

            mockDbContext
                .Setup(handler => handler.FindAsync<Content>(It.IsAny<Guid>()))
                .ReturnsAsync(mockedData);

            mockDbContextFactory
                .Setup(handler => handler.CreateDbContext())
                .Returns(mockDbContext.Object);

            var mockMapper = new Mock<IMapper<Content?, ContentDto>>();

            var database = new EntityFrameworkDatabase<Content, ContentDto>(mockMapper.Object, mockDbContextFactory.Object);

            // Act
            var response = await database.Delete(new Guid());

            // Assert
            mockDbContext.Verify(mock => mock.FindAsync<Content>(It.IsAny<Guid>()), Times.Once);
            mockDbContext.Verify(mock => mock.Attach<Content>(It.IsAny<Content>()), Times.Once);
            mockDbContext.Verify(mock => mock.Remove<Content>(It.IsAny<Content>()), Times.Once);
            mockDbContext.Verify(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(response, new Guid());
        }

    }
}
