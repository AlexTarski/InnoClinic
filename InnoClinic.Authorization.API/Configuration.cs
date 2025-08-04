using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace InnoClinic.Authorization.API;

public static class Configuration
{
    public static IEnumerable<Client> GetClients() => new List<Client>
    {
        new Client
        {
            ClientId = "profiles-api",
            ClientName = "Profiles API",
            AllowedGrantTypes = GrantTypes.Code,
            RequireClientSecret = false,
            RequirePkce = true,
            RedirectUris = { "https://.../signin-oidc" },
            PostLogoutRedirectUris = { "https://.../signout-oidc" },
            AllowedCorsOrigins = { "https://..." },
            AllowedScopes =
            {
                "Profiles.API"
            },
            AllowAccessTokensViaBrowser = true
        }
    };

    public static IEnumerable<ApiResource> GetApiResources() => new List<ApiResource>
    {
        new ApiResource("Profiles.API", "Profiles.API", new[] { JwtClaimTypes.Name })
        {
            Scopes = { "Profiles.API" }
        }
    };


    public static IEnumerable<IdentityResource> GetIdentityResources() => new List<IdentityResource>
    {
        new IdentityResources.OpenId()
    };

    public static IEnumerable<ApiScope> GetApiScopes() => new List<ApiScope>()
    {
        new ApiScope("Profiles.API", "Profiles.API")
    };
}