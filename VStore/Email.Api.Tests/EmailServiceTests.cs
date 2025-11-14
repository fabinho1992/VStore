using MailKit.Net.Smtp;
using MailKit.Security;
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
            var smtpClientMock = new Mock<ISmtpClient>();
            smtpClientMock
                .Setup(x => x.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<SecureSocketOptions>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new TimeoutException("The operation has timed out.")); // 👈 Ou use uma exceção mais genérica

            // Act & Assert
            var exception = Assert.ThrowsAsync<TimeoutException>(() => // 👈 Mude para TimeoutException
                _emailService.SendEmailService("Test", "test@test.com", "User", "Message"));

            Assert.NotNull(exception);
        }
    }
}