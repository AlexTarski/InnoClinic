using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

using InnoClinic.Authorization.Business;
using InnoClinic.Authorization.Business.Configuration;

namespace InnoClinic.Authorization.API;

public static class Configuration
{
    public static IEnumerable<Client> GetClients() => new List<Client>
    {
        new Client
        {
            ClientId = ClientType.ProfilesAPI.GetStringValue(),
            ClientName = ClientType.ProfilesAPI.GetStringValue(),
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
                ClientType.ProfilesAPI.GetStringValue()
            },
            AllowAccessTokensViaBrowser = true
        },
        
        new Client
        {
            ClientId = ClientType.ClientUI.GetStringValue(),
            ClientName = ClientType.ClientUI.GetStringValue(),
            AllowedGrantTypes = GrantTypes.Code,
            RequirePkce = true,
            RequireClientSecret = false,
            
            RedirectUris = { AppUrls.ClientUiUrl },
            PostLogoutRedirectUris = { AppUrls.ClientUiUrl },
            AllowedCorsOrigins = { AppUrls.ClientUiUrl },
            AllowedScopes =
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                IdentityServerConstants.StandardScopes.OfflineAccess,
                IdentityServerConstants.StandardScopes.Email,
                ClientType.ProfilesAPI.GetStringValue(),
                ClientType.ClientUI.GetStringValue()
            },
            AllowOfflineAccess = true,
            AllowAccessTokensViaBrowser = true,
            RequireConsent = false,
        },

        new Client
        {
            ClientId = ClientType.EmployeeUI.GetStringValue(),
            ClientName = ClientType.EmployeeUI.GetStringValue(),
            AllowedGrantTypes = GrantTypes.Code,
            RequirePkce = true,
            RequireClientSecret = false,

            RedirectUris =
            {
                AppUrls.EmployeeUiUrl,
                $"{AppUrls.EmployeeUiUrl}/login-success"
            },
            PostLogoutRedirectUris = { AppUrls.EmployeeUiUrl },
            AllowedCorsOrigins = { AppUrls.EmployeeUiUrl },
            AllowedScopes =
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                IdentityServerConstants.StandardScopes.OfflineAccess,
                IdentityServerConstants.StandardScopes.Email,
                ClientType.ProfilesAPI.GetStringValue(),
                ClientType.EmployeeUI.GetStringValue()
            },
            AllowOfflineAccess = true,
            AllowAccessTokensViaBrowser = true,
            RequireConsent = false,
        }

    };

    public static IEnumerable<ApiResource> GetApiResources() => new List<ApiResource>
    {
        new ApiResource(ClientType.ProfilesAPI.GetStringValue(), ClientType.ProfilesAPI.GetStringValue(), new[] { JwtClaimTypes.Name })
        {
            Scopes = { ClientType.ProfilesAPI.GetStringValue() }
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
        new ApiScope(ClientType.ProfilesAPI.GetStringValue(), ClientType.ProfilesAPI.GetStringValue()),
        new ApiScope(ClientType.ClientUI.GetStringValue(), ClientType.ClientUI.GetStringValue()),
        new ApiScope(ClientType.EmployeeUI.GetStringValue(), ClientType.EmployeeUI.GetStringValue())
    };
}