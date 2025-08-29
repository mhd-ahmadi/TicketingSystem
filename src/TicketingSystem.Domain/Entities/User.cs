using TicketingSystem.Domain.Common;
using TicketingSystem.Domain.Constants;
using TicketingSystem.Domain.Enums;

namespace TicketingSystem.Domain.Entities;

public class User : Entity
{
    public string FullName { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string Role { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public virtual ICollection<Ticket> Tickets { get; set; } = [];

    protected User() { }

    private User(string fullName, string email, string passwordHash, string role, DateTime now)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name is required.", nameof(fullName));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash is required.", nameof(passwordHash));

        FullName = fullName.Trim();
        Email = email.Trim().ToLowerInvariant();
        PasswordHash = passwordHash;
        Role = role;
        CreatedAt = now;
    }

    public static User CreateEmployee(string fullName, string email, string passwordHash, DateTime now)
        => new(fullName, email, passwordHash, RoleNames.Employee, now);

    public static User CreateAdmin(string fullName, string email, string passwordHash, DateTime now)
        => new(fullName, email, passwordHash, RoleNames.Admin, now);


    public void ChangeFullName(string newFullName)
    {
        if (string.IsNullOrWhiteSpace(newFullName))
            throw new ArgumentException("Full name is required.", nameof(newFullName));

        FullName = newFullName.Trim();
    }

    public void ChangeEmail(string newEmail)
    {
        if (string.IsNullOrWhiteSpace(newEmail))
            throw new ArgumentException("Email is required.", nameof(newEmail));

        Email = newEmail.Trim().ToLowerInvariant();
    }

    public void ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new ArgumentException("Password hash is required.", nameof(newPasswordHash));

        PasswordHash = newPasswordHash;
    }

    public void PromoteToAdmin()
    {
        if (Role == RoleNames.Admin)
            throw new InvalidOperationException("User is already an Admin.");

        Role = RoleNames.Admin;
    }

    public void DemoteToEmployee()
    {
        if (Role == RoleNames.Employee)
            throw new InvalidOperationException("User is already an Employee.");

        Role = RoleNames.Employee;
    }
}
