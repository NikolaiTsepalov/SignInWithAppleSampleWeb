// Copyright (c) Martin Costello, 2019. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using AspNet.Security.OAuth.Apple;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace MartinCostello.SignInWithApple;

internal static class AuthenticationModule
{
    private const string DeniedPath = "/denied";
    private const string RootPath = "/";
    private const string SignInPath = "/signin";
    private const string SignOutPath = "/signout";

    public static IServiceCollection AddSignInWithApple(this IServiceCollection builder)
    {
        // Adapted from https://weblog.west-wind.com/posts/2022/Mar/29/Combining-Bearer-Token-and-Cookie-Auth-in-ASPNET
        string schemeName = "JWT_OR_COOKIE";

        return builder
            .AddAuthentication(options =>
            {
                options.DefaultScheme = schemeName;
                options.DefaultChallengeScheme = schemeName;
            })
            .AddCookie(options =>
            {
                options.LoginPath = SignInPath;
                options.LogoutPath = SignOutPath;
            })
            .AddJwtBearer()
            .AddPolicyScheme(schemeName, schemeName, options =>
            {
                options.ForwardDefaultSelector = context =>
                {
                    string authorization = context.Request.Headers["Authorization"];
                    return authorization?.StartsWith("Bearer ") == true ?
                        JwtBearerDefaults.AuthenticationScheme :
                        CookieAuthenticationDefaults.AuthenticationScheme;
                };
            })
            .AddApple()
            .Services
            .AddOptions<AppleAuthenticationOptions>(AppleAuthenticationDefaults.AuthenticationScheme)
            .Configure<IConfiguration, IServiceProvider>((options, configuration, serviceProvider) =>
            {
                options.AccessDeniedPath = DeniedPath;
                options.ClientId = configuration["Apple:ClientId"];
                options.KeyId = configuration["Apple:KeyId"];
                options.TeamId = configuration["Apple:TeamId"];

                var client = serviceProvider.GetService<SecretClient>();

                if (client is not null)
                {
                    // Load the private key from Azure Key Vault if available
                    options.UseAzureKeyVaultSecret(
                        (keyId, cancellationToken) => client.GetSecretAsync($"AuthKey-{keyId}", cancellationToken: cancellationToken));
                }
                else
                {
                    // Otherwise assume the private key is stored locally on disk
                    var environment = serviceProvider.GetRequiredService<IHostEnvironment>();

                    options.UsePrivateKey(
                        keyId =>
                            environment.ContentRootFileProvider.GetFileInfo($"AuthKey_{keyId}.p8"));
                }
            })
            .Services
            .AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<IConfiguration>((options, configuration) =>
            {
                options.Audience = configuration["Apple:ClientId"];
                options.ClaimsIssuer = "https://appleid.apple.com";
                options.MetadataAddress = AppleAuthenticationDefaults.MetadataEndpoint;
            })
            .Services;
    }

    public static IEndpointRouteBuilder MapAuthenticationRoutes(this IEndpointRouteBuilder builder)
    {
        builder.MapGet(DeniedPath, () => Results.Redirect(RootPath + "?denied=true"));
        builder.MapGet(SignOutPath, () => Results.Redirect(RootPath));

        builder.MapPost(SignInPath, async (HttpContext context, IAntiforgery antiforgery) =>
        {
            if (!await antiforgery.IsRequestValidAsync(context))
            {
                return Results.Redirect(RootPath);
            }

            return Results.Challenge(
                new() { RedirectUri = RootPath },
                new[] { AppleAuthenticationDefaults.AuthenticationScheme });
        });

        builder.MapPost(SignOutPath, async (HttpContext context, IAntiforgery antiforgery) =>
        {
            if (!await antiforgery.IsRequestValidAsync(context))
            {
                return Results.Redirect(RootPath);
            }

            return Results.SignOut(
                new() { RedirectUri = RootPath },
                new[] { CookieAuthenticationDefaults.AuthenticationScheme });
        });

        return builder;
    }
}
