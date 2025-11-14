using Moq;
using Microsoft.Extensions.Logging;
using VStore.EmailApi.Domain.Interfaces;
using VStore.EmailApi.Infrastructure.ServiceEmail;
using VStore.EmailApi.Application.UserHttpClient;

namespace SendEmailTests
{
    public class SendEmailTests
    {
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<ILogger<SendEmail>> _loggerMock;
        private readonly Mock<IHttpClientUser> _httpClientUserMock;
        private readonly SendEmail _sendEmail;

        public SendEmailTests()
        {
            _emailServiceMock = new Mock<IEmailService>();
            _loggerMock = new Mock<ILogger<SendEmail>>();
            _httpClientUserMock = new Mock<IHttpClientUser>();

            // 👇 AGORA passando todos os 3 parâmetros necessários
            _sendEmail = new SendEmail(
                _emailServiceMock.Object,
                _loggerMock.Object,          // 👈 Este estava faltando
                _httpClientUserMock.Object   // 👈 Este também estava faltando
            );
        }

        [Fact]
        public async Task SendWelcomeEmail_WithValidParameters_ShouldCallEmailService()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var email = "usuario@example.com";
            var userName = "João Silva";

            // Act
            await _sendEmail.SendWelcomeEmail(userId, email, userName);

            // Assert
            _emailServiceMock.Verify(
                x => x.SendEmailService(
                    It.Is<string>(subject => subject.Contains("Bem-vindo")),
                    email,
                    userName,
                    It.Is<string>(message => message.Contains(userName) && message.Contains("VStore"))
                ),
                Times.Once
            );
        }

        [Fact]
        public async Task SendWelcomeEmail_ShouldUseCorrectEmailTemplate()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var email = "test@example.com";
            var userName = "Maria Santos";

            string capturedSubject = "";
            string capturedMessage = "";

            _emailServiceMock
                .Setup(x => x.SendEmailService(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Callback<string, string, string, string>((subj, to, name, msg) =>
                {
                    capturedSubject = subj;
                    capturedMessage = msg;
                })
                .Returns(Task.CompletedTask);

            // Act
            await _sendEmail.SendWelcomeEmail(userId, email, userName);

            // Assert
            Assert.Equal("Bem-vindo à VStore!", capturedSubject);
            Assert.Contains(userName, capturedMessage);
            Assert.Contains("VStore", capturedMessage);
            Assert.Contains("Visitar Loja", capturedMessage);
            Assert.Contains("ff6b35", capturedMessage); // Cor do template
        }

        [Theory]
        [InlineData("", "Nome Usuário")]
        [InlineData("email@example.com", "")]
        public async Task SendWelcomeEmail_WithInvalidParameters_ShouldStillCallService(string email, string userName)
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            await _sendEmail.SendWelcomeEmail(userId, email, userName);

            // Assert - Verifica que o método foi chamado mesmo com parâmetros inválidos
            _emailServiceMock.Verify(
                x => x.SendEmailService(
                    It.IsAny<string>(),
                    email,
                    userName,
                    It.IsAny<string>()),
                Times.Once);
        }
    }
}