namespace TodoMicroSaas.Domain.Exceptions;

public class TodoToggleDoneException() : DomainException("You are not allowed to toggle todo from another owner.");