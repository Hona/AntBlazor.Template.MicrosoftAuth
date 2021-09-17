using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;

public static class ServiceCollectionExtensions
{
    public static void AddMicrosoftAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(MicrosoftAccountDefaults.AuthenticationScheme)
            .AddCookie(options => { options.AccessDeniedPath = "/access-denied"; })
            .AddMicrosoftAccount(options =>
            {
                options.ClientId = configuration["Authentication:Microsoft:ClientId"];
                options.ClientSecret = configuration["Authentication:Microsoft:ClientSecret"];
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.AuthorizationEndpoint =
                    MicrosoftAccountDefaults.AuthorizationEndpoint + "?prompt=select_account";
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(Constants.RequireLoggedInPolicy, policy =>
            {
                policy.RequireAuthenticatedUser()
                    .RequireClaim(ClaimTypes.Email)
                    .RequireClaim(ClaimTypes.GivenName);

                var allowedEmails = configuration
                    .GetSection(Constants.RequireLoggedInPolicy)
                    .GetChildren()
                    .Select(x => x.Value)
                    .ToArray();
                
                if (allowedEmails.Any())
                {
                    policy.RequireAssertion(context =>
                    {
                        var emailAddress = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
                        var allowedEmails = configuration
                            .GetSection(Constants.RequireLoggedInPolicy)
                            .GetChildren()
                            .Select(x => x.Value)
                            .ToArray();

                        if (!allowedEmails.Any())
                        {
                            return true;
                        }

                        return allowedEmails.Any(x => x == emailAddress);
                    });
                }
            });
            
            options.AddPolicy(Constants.RequireAdminPolicy, policy =>
            {
                policy.RequireAuthenticatedUser()
                    .RequireClaim(ClaimTypes.Email)
                    .RequireAssertion(context =>
                    {
                        var emailAddress = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)
                            ?.Value;
                        
                        var adminEmails = configuration
                            .GetSection(Constants.RequireAdminPolicy)
                            .GetChildren()
                            .Select(x => x.Value)
                            .ToArray();

                        if (!adminEmails.Any())
                        {
                            return false;
                        }

                        return adminEmails.Any(x => x == emailAddress);
                    });
            });
        });
    }
}