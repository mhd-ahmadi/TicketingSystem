using Ardalis.SmartEnum;

namespace TicketingSystem.Domain.Enums;

public abstract class TicketStatus : SmartEnum<TicketStatus, short>
{
    public static readonly TicketStatus Open = new OpenStatus();
    public static readonly TicketStatus InProgress = new InProgressStatus();
    public static readonly TicketStatus Closed = new ClosedStatus();

    private TicketStatus(string name, short value) : base(name, value)
    {
    }

    public abstract bool CanTransitionTo(TicketStatus next);

    private sealed class OpenStatus : TicketStatus
    {
        public OpenStatus() : base("Open", 1) { }

        public override bool CanTransitionTo(TicketStatus next) =>
            next == InProgress || next == Closed;
    }

    private sealed class InProgressStatus : TicketStatus
    {
        public InProgressStatus() : base("InProgress", 5) { }

        public override bool CanTransitionTo(TicketStatus next) =>
            next == Closed;
    }

    private sealed class ClosedStatus : TicketStatus
    {
        public ClosedStatus() : base("Closed", 10) { }

        public override bool CanTransitionTo(TicketStatus next) =>
            next == InProgress;
    }
}