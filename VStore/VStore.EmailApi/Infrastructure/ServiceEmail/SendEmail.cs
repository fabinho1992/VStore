using Microsoft.AspNetCore.Identity;
using VStore.EmailApi.Application.UserHttpClient;
using VStore.EmailApi.Domain.Interfaces;
using VStore.Shared.Contracts.Events;

namespace VStore.EmailApi.Infrastructure.ServiceEmail
{
    public class SendEmail : ISendEmail
    {

        private readonly IEmailService _emailService;
        private readonly ILogger<SendEmail> _logger;
        private readonly IHttpClientUser _userApiClient;

        public SendEmail(IEmailService emailService, ILogger<SendEmail> logger, IHttpClientUser userApiClient)
        {
            _emailService = emailService;
            _logger = logger;
            _userApiClient = userApiClient;
        }

        public async Task SendWelcomeEmail(Guid userId, string email, string userName)
        {
            var subject = "Bem-vindo à VStore!";
            var message = $@"
        <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
            <div style='background: #ff6b35; color: white; padding: 20px; text-align: center;'>
                <h1>🛍️ VStore</h1>
            </div>
            <div style='padding: 20px; background: #f9f9f9;'>
                <h2>Olá, {userName}!</h2>
                <p>Bem-vindo(a) à VStore! Sua conta foi criada com sucesso.</p>
                <p>Estamos felizes em tê-lo(a) conosco. Comece a explorar nossos produtos:</p>
                <div style='text-align: center; margin: 20px 0;'>
                    <a href='https://vstore.com' 
                       style='background: #ff6b35; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>
                        Visitar Loja
                    </a>
                </div>
                <p>Atenciosamente,<br/><strong>Equipe VStore</strong></p>
            </div>
        </div>";

            await _emailService.SendEmailService(
                subject: subject,
                toEmail: email,
                userName: userName,
                message: message
            );
        }

        public Task ResetPassword(string email, string name, string code)
        {
            throw new NotImplementedException();
        }

        public Task SendEmailConfirmation(Guid id)
        {

            throw new ArgumentException("Usuário não encontrado");

        }

        public async Task OrderConfimed(OrderCreatedEvent orderEvent)
        {
            try
            {
                _logger.LogInformation($"Preparando email de confirmação para o pedido #{orderEvent.Id}");

                // Busca os dados do usuário
                var user = await _userApiClient.GetUserByIdAsync(orderEvent.CustomerId);

                if (user == null)
                {
                    _logger.LogWarning($"Usuário {orderEvent.CustomerId} não encontrado. Email não enviado.");
                    return;
                }

                var subject = $"Confirmação de Pedido #{orderEvent.Id} - Aguardando Pagamento";
                var customerEmail = user.Email; // Você precisará obter o email real do cliente
                var userName = $"Cliente {user.DisplayName}"; // Você precisará obter o nome real do cliente

                var emailBody = GenerateOrderConfirmationEmail(orderEvent);

                await _emailService.SendEmailService(subject, customerEmail, userName, emailBody);

                _logger.LogInformation($"Email de confirmação enviado com sucesso para o pedido #{orderEvent.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao enviar email de confirmação para o pedido #{orderEvent.Id}");
                throw;
            }
        }

        private string GenerateOrderConfirmationEmail(OrderCreatedEvent orderEvent)
        {
            var itemsHtml = string.Join("", orderEvent.OrderItems.Select(item => $@"
                <tr>
                    <td style=""padding: 8px; border: 1px solid #ddd;"">{item.ProductName}</td>
                    <td style=""padding: 8px; border: 1px solid #ddd; text-align: center;"">{item.Quantity}</td>
                    <td style=""padding: 8px; border: 1px solid #ddd; text-align: right;"">R$ {item.UnitPrice:F2}</td>
                    <td style=""padding: 8px; border: 1px solid #ddd; text-align: right;"">R$ {item.Subtotal:F2}</td>
                </tr>"));

            return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset=""utf-8"">
                <style>
                    body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                    .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                    .header {{ background: #f8f9fa; padding: 20px; text-align: center; border-radius: 5px; }}
                    .content {{ padding: 20px; }}
                    .order-info {{ background: #e9ecef; padding: 15px; border-radius: 5px; margin-bottom: 20px; }}
                    .table {{ width: 100%; border-collapse: collapse; margin: 20px 0; }}
                    .footer {{ margin-top: 30px; padding: 20px; background: #f8f9fa; border-radius: 5px; text-align: center; }}
                </style>
            </head>
            <body>
                <div class=""container"">
                    <div class=""header"">
                        <h1>🎉 Pedido Confirmado!</h1>
                        <p>Seu pedido foi recebido e está aguardando pagamento</p>
                    </div>
        
                    <div class=""content"">
                        <div class=""order-info"">
                            <h3>Resumo do Pedido</h3>
                            <p><strong>Número do Pedido:</strong> #{orderEvent.Id}</p>
                            <p><strong>Data:</strong> {orderEvent.CreatedDate:dd/MM/yyyy 'às' HH:mm}</p>
                            <p><strong>Status:</strong> <span style=""color: #e67e22;"">🟡 Aguardando Pagamento</span></p>
                            <p><strong>Valor Total:</strong> <strong style=""font-size: 1.2em;"">R$ {orderEvent.TotalAmount:F2}</strong></p>
                        </div>

                        <h3>📦 Itens do seu Pedido</h3>
                        <table class=""table"">
                            <thead>
                                <tr style=""background: #007bff; color: white;"">
                                    <th style=""padding: 10px; text-align: left;"">Produto</th>
                                    <th style=""padding: 10px; text-align: center;"">Qtd</th>
                                    <th style=""padding: 10px; text-align: right;"">Preço Unit.</th>
                                    <th style=""padding: 10px; text-align: right;"">Subtotal</th>
                                </tr>
                            </thead>
                            <tbody>
                                {itemsHtml}
                            </tbody>
                            <tfoot>
                                <tr style=""background: #f8f9fa; font-weight: bold;"">
                                    <td colspan=""3"" style=""padding: 10px; text-align: right;"">Total do Pedido:</td>
                                    <td style=""padding: 10px; text-align: right;"">R$ {orderEvent.TotalAmount:F2}</td>
                                </tr>
                            </tfoot>
                        </table>

                        <div style=""background: #fff3cd; padding: 15px; border-radius: 5px; border-left: 4px solid #ffc107;"">
                            <h4 style=""margin-top: 0; color: #856404;"">⚠️ Próximos Passos</h4>
                            <p>Para finalizar sua compra, realize o pagamento através dos métodos disponíveis em nossa plataforma.</p>
                            <p>Assim que o pagamento for confirmado, você receberá uma nova notificação.</p>
                        </div>
                    </div>

                    <div class=""footer"">
                        <p>Obrigado por comprar conosco! 💙</p>
                        <p><strong>Equipe VStore</strong></p>
                        <p style=""font-size: 0.9em; color: #6c757d;"">
                            Em caso de dúvidas, entre em contato conosco através dos nossos canais de atendimento.
                        </p>
                    </div>
                </div>
            </body>
            </html>";
        }


    }
}
