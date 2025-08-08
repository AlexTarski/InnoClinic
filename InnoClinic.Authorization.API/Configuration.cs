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
            ClientId = "profiles",
            ClientName = "profiles",
            AllowedGrantTypes = GrantTypes.Code,
            RequireClientSecret = false,
            RequirePkce = true,
            RedirectUris = { "https://.../signin-oidc" },
            PostLogoutRedirectUris = { "https://.../signout-oidc" },
            AllowedCorsOrigins = { "https://..." },
            AllowedScopes =
            {
                IdentityServerConstants.StandardScopes.OfflineAccess,
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                IdentityServerConstants.StandardScopes.Email,
                "profiles"
            },
            AllowAccessTokensViaBrowser = true
        },
        
        new Client
        {
            ClientId = "client_ui",
            ClientName = "client_ui",
            AllowedGrantTypes = GrantTypes.Code,
            RequirePkce = true,
            RequireClientSecret = false,
            
            RedirectUris = { "https://localhost:4200" },
            PostLogoutRedirectUris = { "https://localhost:4200" },
            AllowedCorsOrigins = { "https://localhost:4200" },
            AllowedScopes =
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                IdentityServerConstants.StandardScopes.OfflineAccess,
                IdentityServerConstants.StandardScopes.Email,
                "profiles",
            },
            AllowOfflineAccess = true,
            AllowAccessTokensViaBrowser = true,
            RequireConsent = false,
        }
    };

    public static IEnumerable<ApiResource> GetApiResources() => new List<ApiResource>
    {
        new ApiResource("profiles", "profiles", new[] { JwtClaimTypes.Name })
        {
            Scopes = { "profiles" }
        }
    };

    public static IEnumerable<IdentityResource> GetIdentityResources() => new List<IdentityResource>
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new IdentityResources.Email(),
    };

    public static IEnumerable<ApiScope> GetApiScopes() => new List<ApiScope>()
    {
        new ApiScope("profiles", "profiles")
    };
}