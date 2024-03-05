namespace TodoMicroSaas.Domain.Exceptions;

public class TodoLimitExceededWithNoSubscriptionException() : DomainException(
    "The limit of todo creation is 5 for those who doesn't have a active subscription");