using FluentValidation;

namespace TicketingSystem.Application.Common.Extensions;

public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, short?> IsInEnum<T>(
        this IRuleBuilder<T, short?> ruleBuilder, Type enumType)
    {
        return ruleBuilder.Must(value =>
        {
            if (!value.HasValue) return false;
            return Enum.IsDefined(enumType, value.Value);
        }).WithMessage("Invalid value for enum.");
    }
}