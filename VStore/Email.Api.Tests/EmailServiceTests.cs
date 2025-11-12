using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using VStore.EmailApi.Domain.Interfaces;
using VStore.EmailApi.Infrastructure.EmailService;

namespace Email.Api.Tests
{
    public class EmailServiceTests
    {

        private readonly Mock<IConfiguration> _configMock;
        private readonly Mock<ILogger<EmailService>> _loggerMock;
        private readonly IEmailService _emailService;

        public EmailServiceTests()
        {
            _configMock = new Mock<IConfiguration>();
            _loggerMock = new Mock<ILogger<EmailService>>();

            // Configuração mockada
            var configurationSectionMock = new Mock<IConfigurationSection>();
            configurationSectionMock.Setup(x => x.Value).Returns("smtp.gmail.com");

            _configMock.Setup(x => x["SmtpSettings:Server"]).Returns("smtp.gmail.com");
            _configMock.Setup(x => x["SmtpSettings:Port"]).Returns("587");
            _configMock.Setup(x => x["SmtpSettings:User"]).Returns("test@example.com");
            _configMock.Setup(x => x["SmtpSettings:Pass"]).Returns("password123");

            _emailService = new EmailService(_configMock.Object, _loggerMock.Object);
        }

        

        [Theory]
        [InlineData("", "user@example.com", "John", "message")]
        [InlineData("Subject", "", "John", "message")]
        [InlineData("Subject", "user@example.com", "", "message")]
        [InlineData("Subject", "user@example.com", "John", "")]
        public async Task SendEmailService_WithInvalidParameters_ShouldThrowException(
            string subject, string toEmail, string userName, string message)
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _emailService.SendEmailService(subject, toEmail, userName, message));
        }

        [Theory]
        [InlineData("email-invalido")]
        [InlineData("sem-arroba.com")]
        [InlineData("@sem-prefixo.com")]
        public async Task SendEmailService_WithInvalidEmailFormat_ShouldThrowException(string invalidEmail)
        {
            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _emailService.SendEmailService("Test", invalidEmail, "User", "Message"));

            // Pelo menos verifica que alguma exceção foi lançada
            Assert.NotNull(exception);
        }

        [Fact]
        public async Task SendEmailService_WhenSmtpFails_ShouldLogErrorAndThrow()
        {
            // Arrange
            var subject = "Test Subject";
            var toEmail = "user@example.com";
            var userName = "John Doe";
            var message = "<h1>Test Message</h1>";

            // Configurar SMTP para falhar (porta inválida)
            _configMock.Setup(x => x["SmtpSettings:Port"]).Returns("9999");

            // Re-criar o serviço com a configuração que causa erro
            var emailServiceWithError = new EmailService(_configMock.Object, _loggerMock.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<System.Net.Sockets.SocketException>(() =>
                emailServiceWithError.SendEmailService(subject, toEmail, userName, message));

            // Verifica se o erro foi logado
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Erro ao enviar email para:")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}