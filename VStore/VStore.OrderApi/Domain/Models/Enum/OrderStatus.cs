namespace VStore.OrderApi.Domain.Enum
{
    public enum OrderStatus
    {
        // Status iniciais
        PendingPayment = 1,    // Aguardando pagamento
        PaymentApproved = 2,   // Pagamento aprovado
        PaymentDenied = 3,     // Pagamento negado

        // Processamento
        Processing = 4,        // Em processamento
        InSeparation = 5,      // Em separação no estoque

        // Envio
        ReadyForShipping = 6,  // Pronto para envio
        Shipped = 7,           // Enviado
        InTransit = 8,         // Em trânsito

        // Entrega
        OutForDelivery = 9,    // Saiu para entrega
        Delivered = 10,        // Entregue

        // Problemas
        DeliveryAttemptFailed = 11, // Tentativa de entrega falhou
        Returned = 12,         // Devolvido

        // Finalizados
        Completed = 13,        // Finalizado com sucesso
        Cancelled = 14,        // Cancelado
        Refunded = 15          // Reembolsado
    }
}
