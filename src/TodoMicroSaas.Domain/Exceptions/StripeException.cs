namespace TodoMicroSaas.Domain.Exceptions;

public class StripeException(string message) : DomainException(message);
