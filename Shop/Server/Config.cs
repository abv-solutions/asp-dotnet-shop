using System.Collections.Generic;
using IdentityServer4.Models;

namespace Shop.Server
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource("country", new [] { "country" })
            };


        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            { };

        
        public static IEnumerable<IdentityServer4.Models.Client> Clients =>
            new IdentityServer4.Models.Client[]
            { };
    }
}