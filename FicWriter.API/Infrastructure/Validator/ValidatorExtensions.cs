namespace FicWriter.API.Infrastructure.Validator;

public static class ValidatorExtensions
{
    public static bool IsNotValid(this FluentValidation.Results.ValidationResult validationResult)
    {
        return !validationResult.IsValid;
    }
}
