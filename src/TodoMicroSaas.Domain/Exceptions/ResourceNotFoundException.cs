namespace TodoMicroSaas.Domain.Exceptions;

public class ResourceNotFoundException(string entity) : DomainException($"Resource not found for entity {entity}");