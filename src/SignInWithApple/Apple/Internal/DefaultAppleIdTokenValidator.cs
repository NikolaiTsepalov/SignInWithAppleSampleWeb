/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace AspNet.Security.OAuth.Apple.Internal
{
    internal sealed class DefaultAppleIdTokenValidator : AppleIdTokenValidator
    {
        private readonly ILogger _logger;
        private readonly AppleKeyStore _keyStore;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public DefaultAppleIdTokenValidator(
            AppleKeyStore keyStore,
            JwtSecurityTokenHandler tokenHandler,
            ILogger<DefaultAppleIdTokenValidator> logger)
        {
            _keyStore = keyStore;
            _tokenHandler = tokenHandler;
            _logger = logger;
        }

        public override async Task ValidateAsync(AppleValidateIdTokenContext context)
        {
            if (!_tokenHandler.CanValidateToken)
            {
                throw new NotSupportedException($"The configured {nameof(JwtSecurityTokenHandler)} cannot validate tokens.");
            }

            byte[] keysJson = await _keyStore.LoadPublicKeysAsync(context);

            string json = Encoding.UTF8.GetString(keysJson);
            var keySet = JsonWebKeySet.Create(json);

            var parameters = new TokenValidationParameters()
            {
                ValidAudience = context.Options.ClientId,
                ValidIssuer = context.Options.TokenAudience,
                IssuerSigningKeys = keySet.Keys,
            };

            try
            {
                _tokenHandler.ValidateToken(context.IdToken, parameters, out var _);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Apple ID token validation failed for issuer {TokenIssuer} and audience {TokenAudience}. ID Token: {IdToken}",
                    parameters.ValidAudience,
                    parameters.ValidIssuer,
                    context.IdToken);

                throw;
            }
        }
    }
}