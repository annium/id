using Annium.Extensions.Validation;

namespace Site.Public.Pages.Login;

public class LoginData
{
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginDataValidator : Validator<LoginData>
{
    public LoginDataValidator()
    {
        Field(x => x.Login).Required().Then().MinLength(3).MaxLength(50);
        Field(x => x.Password).Required().Then().MinLength(8).MaxLength(50);
    }
}
