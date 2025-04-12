using CommonTestUtils.Requests;
using FicWriter.API.Features.Users.Create;
using Shouldly;

namespace FicWriter.Tests.User.Validators;

public class CreateUserValidatorTest
{
    [Fact]
    public void Success()
    {
        var request = CreateUserRequestBuilder.Build();
        var validator = new CreateUserValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void ShouldFail_WhenNameIsEmpty()
    {
        var request = CreateUserRequestBuilder.Build() with { Name = string.Empty };
        var validator = new CreateUserValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe("Name is required.");
    }

    [Fact]
    public void ShouldFail_WhenPasswordIsNotRequiredLength()
    {
        var request = CreateUserRequestBuilder.Build(passwordLength: 3);
        var validator = new CreateUserValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe("Password must be at least 6 characters long.");
    }
}
