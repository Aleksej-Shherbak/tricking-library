using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TrickingLibrary.WebApi.Constants;

namespace TrickingLibrary.WebApi.Extensions
{
    public static class IdentityExtension
    {
        public static IServiceCollection AddTrickingLibraryIdentity(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddDbContext<IdentityDbContext>(config =>
                config.UseInMemoryDatabase("DevIdentity"));

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
                {
                    if (environment.IsDevelopment())
                    {
                        options.Password.RequireDigit = false;
                        options.Password.RequiredLength = 4;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireUppercase = false;
                    }
                    else
                    {
                        // TODO: configure for production
                    }
                })
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/Login";
                config.LogoutPath = "/api/auth/logout";
            });
            
            var identityServerBuilder = services.AddIdentityServer();

            identityServerBuilder.AddAspNetIdentity<IdentityUser>();
                
            
            if (environment.IsDevelopment())
            {
                identityServerBuilder.AddInMemoryIdentityResources(new IdentityResource[]
                {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile(),
                });

                identityServerBuilder.AddInMemoryApiScopes(new ApiScope[]
                {
                    new ApiScope(IdentityServerConstants.LocalApi.ScopeName, new string[]
                    {
                        ClaimTypes.Role
                    }),
                });
                
                identityServerBuilder.AddInMemoryClients(new Client[]
                {
                    new Client
                    {
                        ClientId = "web-client",
                        AllowedGrantTypes = GrantTypes.Code,
                        
                        RedirectUris = new[] { "http://localhost:3000" },
                        PostLogoutRedirectUris = new[] { "http://localhost:3000" },
                        AllowedCorsOrigins = new[] { "http://localhost:3000" },
                        
                        AllowedScopes = new []
                        {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            IdentityServerConstants.LocalApi.ScopeName,
                        }, 
                        
                        RequirePkce = true,
                        AllowAccessTokensViaBrowser = true,
                        RequireConsent = false,
                        RequireClientSecret = false,
                    }
                });

                identityServerBuilder.AddDeveloperSigningCredential();
            }
            else
            {
                // TODO: configure for production
            }

            services.AddLocalApiAuthentication();
            
            services.AddAuthorization(options =>
            {
                options.AddPolicy(TrickingLibraryConstants.Policies.Mod, policy =>
                {
                    var is4Policy = options.GetPolicy(IdentityServerConstants.LocalApi.PolicyName);
                    policy.Combine(is4Policy);
                    policy.RequireClaim(TrickingLibraryConstants.Claims.Role, TrickingLibraryConstants.Roles.Mod);
                });
            });
            
            return services;
        }
    }
}