using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace IdentityServer
{
    public static class Configuration
    {
        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
                new Client()
                {
                    ClientId = "blazor_client",
                    
                    AllowedGrantTypes = GrantTypes.Code,

                    AllowedScopes = 
                    {
                        "ContactAPI",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },

                    RedirectUris = { "https://localhost:7001/authentication/login-callback"},
                    PostLogoutRedirectUris = { "https://localhost:7001/authentication/logout-callback"},
                    
                    RequireConsent = false,
                    RequireClientSecret= false,
                    RequirePkce = true,
                    AllowOfflineAccess = true,

                    AllowedCorsOrigins = { "https://localhost:7001" }
                }
            };

        public static IEnumerable<ApiScope> GetApiScopes() =>
            new List<ApiScope>
            {
                new ApiScope("ContactAPI")
            };

        public static IEnumerable<ApiResource> GetApiResources() =>
            new List<ApiResource>
            {
                new ApiResource("ContactAPI")
            };

        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
    }
}
