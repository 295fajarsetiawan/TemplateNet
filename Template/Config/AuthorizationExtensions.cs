namespace Template.Config;

public static class AuthorizationExtensions
{
    public static void AddCustomAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("ApiPolicy", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "api.read");
            });
        });
    }
}