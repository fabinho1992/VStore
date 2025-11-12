using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using UserApi.Domain;
using UserApi.Domain.Events;
using UserApi.Domain.Interfaces.IAuthService;
using UserApi.Domain.ModelsAutentication;
using MassTransit;
using BookReviewManager.Infrastructure.Service.Identity;
using UserApi.Infrastructure.Service.Identity;

namespace UserApi.UnitTests.Infrastructure;

public class CreateUserTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IPublishEndpoint> _publishEndpointMock;
    private readonly Mock<ILogger<CreateUser>> _loggerMock;
    private readonly CreateUser _createUser;

    public CreateUserTests()
    {
        _userManagerMock = new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);

        _publishEndpointMock = new Mock<IPublishEndpoint>();
        _loggerMock = new Mock<ILogger<CreateUser>>();

        _createUser = new CreateUser(_userManagerMock.Object, _publishEndpointMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task CreateUserAsync_WithNewUser_ShouldCreateUserAndPublishEvent()
    {
        // Arrange
        var registerUser = new RegisterUser(userName: "John Doe", email: "john@example.com",
            password: "Password123!");

        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            DisplayName = registerUser.UserName,
            UserName = registerUser.Email,
            Email = registerUser.Email,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        _userManagerMock
            .Setup(x => x.FindByEmailAsync(registerUser.Email))
            .ReturnsAsync((User)null); // Usuário não existe

        _userManagerMock
            .Setup(x => x.CreateAsync(It.IsAny<User>(), registerUser.Password))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _createUser.CreateUserAsync(registerUser);

        // Assert
        Assert.Equal("Ok", result.Status);
        Assert.Equal("User created successfully", result.Message);

        _userManagerMock.Verify(x => x.FindByEmailAsync(registerUser.Email), Times.Once);
        _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<User>(), registerUser.Password), Times.Once);
        _publishEndpointMock.Verify(x => x.Publish(It.IsAny<UserCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateUserAsync_WhenUserAlreadyExists_ShouldReturnError()
    {
        // Arrange
        var registerUser = new RegisterUser(userName: "John Doe", email: "john@example.com",
            password: "Password123!");

        var existingUser = new User { Email = registerUser.Email, UserName = registerUser.UserName };

        _userManagerMock
            .Setup(x => x.FindByEmailAsync(registerUser.Email))
            .ReturnsAsync(existingUser); // Usuário já existe

        // Act
        var result = await _createUser.CreateUserAsync(registerUser);

        // Assert
        Assert.Equal("Erro", result.Status);
        Assert.Equal("User already exists!", result.Message);

        _userManagerMock.Verify(x => x.FindByEmailAsync(registerUser.Email), Times.Once);
        _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
        _publishEndpointMock.Verify(x => x.Publish(It.IsAny<UserCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateUserAsync_WhenCreationFails_ShouldReturnError()
    {
        // Arrange
        var registerUser = new RegisterUser(userName: "John Doe", email: "john@example.com",
            password: "Password123!");

        _userManagerMock
            .Setup(x => x.FindByEmailAsync(registerUser.Email))
            .ReturnsAsync((User)null);

        var identityErrors = new List<IdentityError>
        {
            new IdentityError { Description = "Password too weak" }
        };

        _userManagerMock
            .Setup(x => x.CreateAsync(It.IsAny<User>(), registerUser.Password))
            .ReturnsAsync(IdentityResult.Failed(identityErrors.ToArray()));

        // Act
        var result = await _createUser.CreateUserAsync(registerUser);

        // Assert
        Assert.Equal("Error creating user", result.Status);
        Assert.Equal("Erro", result.Message);

        _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<User>(), registerUser.Password), Times.Once);
        _publishEndpointMock.Verify(x => x.Publish(It.IsAny<UserCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateUserAsync_WhenSuccessful_ShouldPublishCorrectEvent()
    {
        // Arrange
        var registerUser = new RegisterUser(userName: "John Doe", email: "john@example.com",
            password: "Password123!");

        User createdUser = null;

        _userManagerMock
            .Setup(x => x.FindByEmailAsync(registerUser.Email))
            .ReturnsAsync((User)null);

        _userManagerMock
            .Setup(x => x.CreateAsync(It.IsAny<User>(), registerUser.Password))
            .Callback<User, string>((user, password) => createdUser = user)
            .ReturnsAsync(IdentityResult.Success);

        UserCreatedEvent publishedEvent = null;
        _publishEndpointMock
            .Setup(x => x.Publish(It.IsAny<UserCreatedEvent>(), It.IsAny<CancellationToken>()))
            .Callback<UserCreatedEvent, CancellationToken>((evt, token) => publishedEvent = evt);

        // Act
        var result = await _createUser.CreateUserAsync(registerUser);

        // Assert
        _publishEndpointMock.Verify(x => x.Publish(It.IsAny<UserCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);

        Assert.NotNull(publishedEvent);
        Assert.Equal(registerUser.Email, publishedEvent.Email);
        Assert.Equal(registerUser.Email, publishedEvent.UserName); // Note: UserName = Email no seu código
        Assert.True((DateTime.UtcNow - publishedEvent.CreatedAt).TotalSeconds < 5); // Verifica que foi criado agora
    }
}