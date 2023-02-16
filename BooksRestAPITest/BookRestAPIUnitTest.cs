using BooksRestAPI.Controllers;
using BooksRestAPI.Models;
using BooksRestAPI.Models.DataAccess.Contract;
using BooksRestAPI.Models.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BooksRestAPITest
{
        public class BookRestAPIUnitTest
        {
            private BooksController booksController;
            private int Id = 1;
            private readonly Mock<IBookRepo> bookStub = new Mock<IBookRepo>();
            Book sampleBook = new Book
            {
                Id = 1,
                Name = "State Patsy",
                Genere = "Action/Adventure",
                PublisherName = "Queens",
            };
            Book toBePostedBook = new Book
            {
                Name = "Federal Matters",
                Genere = "Suspense",
                PublisherName = "Harpers",
            };
            [Fact]
            public async Task GetBook_BasedOnId_WithNotExistingBook_ReturnNotFound()
            {
                //Arrange
                //use the mock to set up the test. we are basically telling here that whatever int id we pass to this method
                //it will always return null
                booksController = new BooksController(bookStub.Object);
                bookStub.Setup(repo => repo.GetBook(It.IsAny<int>())).ReturnsAsync(new NotFoundResult());
                //Act
                var actionResult = await booksController.GetBook(1);
                //Assert
                Assert.IsType<NotFoundResult>(actionResult.Result);
            }
            [Fact]
            public async Task GetBook_BasedOnId_WithExistingBook_ReturnBook()
            {
                //Arrange
                //use the mock to set up the test. we are basically telling here that whatever int id we pass to this method
                //it will always return a new Book object
                bookStub.Setup(service => service.GetBook(It.IsAny<int>())).ReturnsAsync(sampleBook);
                booksController = new BooksController(bookStub.Object);
                //Act
                var actionResult = await booksController.GetBook(1);
                //Assert
                Assert.IsType<Book>(actionResult.Value);
                var result = actionResult.Value;
                //Compare the result member by member
                sampleBook.Should().BeEquivalentTo(result,
                    options => options.ComparingByMembers<Book>());
            }
            [Fact]
            public async Task PostVideoGame_WithNewVideogame_ReturnNewlyCreatedVideogame()
            {
                //Arrange
                bookStub.Setup(repo => repo.PostBook(It.IsAny<Book>())).ReturnsAsync(sampleBook);

                booksController = new BooksController(bookStub.Object);
                //Act
                var actionResult = await booksController.PostBook(toBePostedBook);
                //Assert
                Assert.Equal("201", ((CreatedAtActionResult)actionResult.Result).StatusCode.ToString());

            }
            [Fact]
            public async Task PostVideoGame_WithException_ReturnsInternalServerError()
            {
                //Arrange
                bookStub.Setup(service => service.PostBook(It.IsAny<Book>())).Throws(new Exception());
                booksController = new BooksController(bookStub.Object);
                //Act
                var actionResult = await booksController.PostBook(null);
                //Assert
                Assert.Equal("500", ((StatusCodeResult)actionResult.Result).StatusCode.ToString());
            }
            [Fact]
            public async Task PutVideoGame_WithException_ReturnsConcurrencyExecption()
            {
                //Arrange
                bookStub.Setup(service => service.PutBook(It.IsAny<int>(), It.IsAny<Book>())).Throws(new DbUpdateConcurrencyException());
                booksController = new BooksController(bookStub.Object);
                //Act
                var actionResult = await booksController.PutBook(Id, sampleBook);
                //Assert
                Assert.Equal("409", ((StatusCodeResult)actionResult).StatusCode.ToString());

            }
            [Fact]
            public async Task PutVideoGame_WithException_ReturnsExecption()
            {
                //Arrange
                bookStub.Setup(service => service.PutBook(It.IsAny<int>(), It.IsAny<Book>())).Throws(new Exception());
                booksController = new BooksController(bookStub.Object);
                //Act
                var actionResult = await booksController.PutBook(Id, sampleBook);
                //Assert
                Assert.Equal("500", ((StatusCodeResult)actionResult).StatusCode.ToString());
            }
            [Fact]
            public async Task PutVideoGame_WithExistingVideogame_BasedOnId_ReturnUpdatedVideogame()
            {
                //Arrange
                bookStub.Setup(service => service.PutBook(It.IsAny<int>(), It.IsAny<Book>())).ReturnsAsync(new NoContentResult());
                booksController = new BooksController(bookStub.Object);
                //Act
                var actionResult = await booksController.PutBook(Id, sampleBook);
                //Assert
                actionResult.Should().BeOfType<NoContentResult>();
            }
        }
    }