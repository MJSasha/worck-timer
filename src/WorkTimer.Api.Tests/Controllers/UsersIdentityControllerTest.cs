using System;
using System.Net;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using QuickActions.Api.Identity.Services;
using QuickActions.Common.Data;
using QuickActions.Common.Specifications;
using WorkTimer.Api.Controllers;
using WorkTimer.Api.Repository;
using WorkTimer.Common.Data;
using WorkTimer.Common.Models;

namespace WorkTimer.Api.Tests.Controllers;

[TestFixture]
[TestOf(typeof(UsersIdentityController))]
public class UsersIdentityControllerTest
{
    private Mock<SessionsService<User>> _sessionsServiceMock;
    private Mock<UsersRepository> _usersRepositoryMock;
    private UsersIdentityController _controller;

    [SetUp]
    public void Setup()
    {
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var keyName = "session_key";
        var sessionLifeTime = 3600;
        Func<Session<User>, string[], bool> rolesChecker = (session, roles) => true;


        _sessionsServiceMock = new Mock<SessionsService<User>>(httpContextAccessorMock.Object, keyName, sessionLifeTime, rolesChecker);

        var dbContextMock = new Mock<AppDbContext>();
        _usersRepositoryMock = new Mock<UsersRepository>(dbContextMock.Object);
        _controller = new UsersIdentityController(_sessionsServiceMock.Object, _usersRepositoryMock.Object);
    }

    [Test]
    public void Login_NullCredentials_ThrowsBadRequestException()
    {
        // Arrange
        var authModel = new AuthModel { Email = null, Password = null };

        // Act & Assert
        Assert.ThrowsAsync<HttpResponseException>(() => _controller.Login(authModel));
    }

    [Test]
    public void Login_UserNotFound_ThrowsUnauthorizedException()
    {
        // Arrange
        var authModel = new AuthModel { Email = "test@example.com", Password = "password123" };

        _usersRepositoryMock.Setup(repo => repo.Read(It.IsAny<Specification<User>>())).ReturnsAsync((User)null);

        // Act & Assert
        var exception = Assert.ThrowsAsync<HttpResponseException>(() => _controller.Login(authModel));
        Assert.That(exception.Response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        Assert.That(exception.Response.ReasonPhrase, Is.EqualTo("User not found."));
    }
}