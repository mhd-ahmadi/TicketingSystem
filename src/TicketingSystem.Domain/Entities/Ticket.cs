using TicketingSystem.Domain.Common;
using TicketingSystem.Domain.Enums;

namespace TicketingSystem.Domain.Entities;

public class Ticket : Entity
{
    protected Ticket() { }

    public string Title { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public TicketStatus Status { get; private set; } = TicketStatus.Open;
    public TicketPriority Priority { get; private set; } = TicketPriority.Medium;
    public Guid CreatedByUserId { get; private set; }
    public virtual User? AssignedToUser { get; private set; }
    public Guid? AssignedToUserId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Ticket(string title, string description, TicketPriority priority, Guid createdByUserId, DateTime now)
    {
        Title = title;
        Description = description;
        Priority = priority;
        Status = TicketStatus.Open;
        CreatedAt = now;
        CreatedByUserId = createdByUserId;
    }

    public static Ticket Create(string title, string description, TicketPriority priority, Guid createdByUserId, DateTime now)
        => new(title, description, priority, createdByUserId, now);

    public void AssignTo(Guid adminUserId, DateTime now)
    {
        if (Status == TicketStatus.Closed)
            throw new InvalidOperationException("Cannot assign a closed ticket.");

        AssignedToUserId = adminUserId;
        Touch(now);
    }

    public void SetStatus(TicketStatus newStatus, DateTime now)
    {
        if (!Status.CanTransitionTo(newStatus))
            throw new InvalidOperationException($"Cannot change status to {newStatus.Name}");

        Status = newStatus;
        Touch(now);
    }

    public void StartProgress(DateTime now)
    {
        if (!Status.CanTransitionTo(TicketStatus.Open))
            throw new InvalidOperationException("Only open tickets can be moved to InProgress.");

        Status = TicketStatus.InProgress;
        Touch(now);
    }

    public void Close(DateTime now)
    {
        if (Status == TicketStatus.Closed)
            throw new InvalidOperationException("Ticket is already closed.");

        Status = TicketStatus.Closed;
        Touch(now);
    }

    public void ChangePriority(TicketPriority newPriority, DateTime now)
    {
        if (Priority == newPriority)
            return;

        Priority = newPriority;
        Touch(now);
    }

    public void UpdateDescription(string newDescription, DateTime now)
    {
        if (string.IsNullOrWhiteSpace(newDescription))
            throw new ArgumentException("Description is required.", nameof(newDescription));

        Description = newDescription.Trim();
        Touch(now);
    }

    private void Touch(DateTime now) => UpdatedAt = now;
}
