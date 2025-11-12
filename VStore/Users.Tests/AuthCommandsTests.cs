using Moq;
using UserApi.Application.Services.IAuthService;
using UserApi.Domain.ModelsAutentication; 
using UserApi.Domain.Interfaces.IAuthService;
using VStore.OrderApi.Apllication_Order.Dtos;

namespace UserApi.UnitTests.Application;

public class AuthCommandsTests
{
    private readonly Mock<ICreateUser> _createUserMock;
    private readonly Mock<ILoginUser> _loginUserMock;
    private readonly AuthCommands _authCommands;

    public AuthCommandsTests()
    {
        _createUserMock = new Mock<ICreateUser>();
        _loginUserMock = new Mock<ILoginUser>();
        _authCommands = new AuthCommands(_createUserMock.Object, _loginUserMock.Object);
    }

    [Fact]
    public async Task CreateUser_WhenUserCreationSucceeds_ShouldReturnSuccess()
    {
        // Arrange
        var registerUser = new RegisterUser(userName: "John Doe", email: "john@example.com",
            password: "Password123!");
        

        var createUserResponse = new ResponseIdentityCreate 
        { 
            Status = "Ok", 
            Message = "User created successfully" 
        };

        _createUserMock
            .Setup(x => x.CreateUserAsync(registerUser))
            .ReturnsAsync(createUserResponse);

        // Act
        var result = await _authCommands.CreateUser(registerUser);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("User created successfully", result.Data);
        //Assert.Null(!result.IsSuccess);
        
        _createUserMock.Verify(x => x.CreateUserAsync(registerUser), Times.Once);
    }

    [Fact]
    public async Task CreateUser_WhenUserCreationFails_ShouldReturnError()
    {
        // Arrange
        var registerUser = new RegisterUser(userName: "John Doe", email: "john@example.com",
            password: "Password123!");

        var createUserResponse = new ResponseIdentityCreate 
        { 
            Status = "Erro", 
            Message = "User already exists!" 
        };

        _createUserMock
            .Setup(x => x.CreateUserAsync(registerUser))
            .ReturnsAsync(createUserResponse);

        // Act
        var result = await _authCommands.CreateUser(registerUser);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("User already exists!", result.Message);
        Assert.Null(result.Data);
        
        _createUserMock.Verify(x => x.CreateUserAsync(registerUser), Times.Once);
    }

    [Theory]
    [InlineData("Erro", "User already exists!")]
    [InlineData("Error creating user", "Password requirements not met")]
    public async Task CreateUser_WithDifferentErrorScenarios_ShouldReturnAppropriateError(string status, string message)
    {
        // Arrange
        var registerUser = new RegisterUser(userName: "John Doe", email: "john@example.com",
             password: "Password123!");

        var createUserResponse = new ResponseIdentityCreate 
        { 
            Status = status, 
            Message = message 
        };

        _createUserMock
            .Setup(x => x.CreateUserAsync(registerUser))
            .ReturnsAsync(createUserResponse);

        // Act
        var result = await _authCommands.CreateUser(registerUser);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(message, result.Message);
    }
}