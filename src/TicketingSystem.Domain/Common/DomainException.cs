﻿namespace TicketingSystem.Domain.Common;

public abstract class DomainException(string message) : Exception(message)
{
}
