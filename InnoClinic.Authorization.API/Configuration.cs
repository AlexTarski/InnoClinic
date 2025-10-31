using InnoClinic.Shared;
using InnoClinic.Authorization.Business;
using InnoClinic.Authorization.Business.Configuration;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityModel;

namespace InnoClinic.Authorization.API;

public static class Configuration
{
    private const string apiSignInUri = "https://.../signin-oidc";
    private const string apiSignOutUri = "https://.../signout-oidc";
    private const string allowedCorsOriginsUris = "https://...";

    #region CustomClaims
    private const string photoId = "photo_id";
    #endregion

    public static IEnumerable<Client> GetClients() => new List<Client>
    {
        new Client
        {
            ClientId = ClientType.ProfilesAPI.GetStringValue(),
            ClientName = ClientType.ProfilesAPI.GetStringValue(),
            AllowedGrantTypes = GrantTypes.Code,
            RequireClientSecret = false,
            RequirePkce = true,
            RedirectUris = { apiSignInUri },
            PostLogoutRedirectUris = { apiSignOutUri },
            AllowedCorsOrigins = { allowedCorsOriginsUris },
            AllowedScopes =
            {
                IdentityServerConstants.StandardScopes.OfflineAccess,
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                IdentityServerConstants.StandardScopes.Email,
                ClientType.ProfilesAPI.GetStringValue(),

            },
            AllowAccessTokensViaBrowser = true
        },

        new Client
        {
            ClientId = ClientType.OfficesAPI.GetStringValue(),
            ClientName = ClientType.OfficesAPI.GetStringValue(),
            AllowedGrantTypes = GrantTypes.Code,
            RequireClientSecret = false,
            RequirePkce = true,
            RedirectUris = { apiSignInUri },
            PostLogoutRedirectUris = { apiSignOutUri },
            AllowedCorsOrigins = { allowedCorsOriginsUris },
            AllowedScopes =
            {
                IdentityServerConstants.StandardScopes.OfflineAccess,
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                IdentityServerConstants.StandardScopes.Email,
                ClientType.OfficesAPI.GetStringValue()
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
                ClientType.OfficesAPI.GetStringValue(),
                ClientType.ClientUI.GetStringValue(),
                photoId
            },
            AllowOfflineAccess = true,
            AllowAccessTokensViaBrowser = true,
            RequireConsent = false,
            AlwaysSendClientClaims = true,
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
                ClientType.OfficesAPI.GetStringValue(),
                ClientType.EmployeeUI.GetStringValue(),
                photoId,
            },
            AllowOfflineAccess = true,
            AllowAccessTokensViaBrowser = true,
            RequireConsent = false,
        }
    };

    public static IEnumerable<ApiResource> GetApiResources() => new List<ApiResource>
    {
        new ApiResource(ClientType.ProfilesAPI.GetStringValue(), ClientType.ProfilesAPI.GetStringValue(), 
            new[] { JwtClaimTypes.Name, JwtClaimTypes.Role })
        {
            Scopes = { ClientType.ProfilesAPI.GetStringValue() }
        },

        new ApiResource(ClientType.OfficesAPI.GetStringValue(), ClientType.OfficesAPI.GetStringValue(), 
            new[] { JwtClaimTypes.Name, JwtClaimTypes.Role })
        {
            Scopes = { ClientType.OfficesAPI.GetStringValue() }
        }
    };

    public static IEnumerable<IdentityResource> GetIdentityResources() => new List<IdentityResource>
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new IdentityResources.Email(),
        new IdentityResources.Phone(),

        new IdentityResource(
                name: photoId,
                userClaims: new[] { photoId },
                displayName: "Photo ID")
    };

    public static IEnumerable<ApiScope> GetApiScopes() => new List<ApiScope>()
    {
        new ApiScope(ClientType.ProfilesAPI.GetStringValue(), ClientType.ProfilesAPI.GetStringValue()),
        new ApiScope(ClientType.OfficesAPI.GetStringValue(), ClientType.ProfilesAPI.GetStringValue()),
        new ApiScope(ClientType.ClientUI.GetStringValue(), ClientType.ClientUI.GetStringValue()),
        new ApiScope(ClientType.EmployeeUI.GetStringValue(), ClientType.EmployeeUI.GetStringValue())
    };
}