﻿using AspWebApiGlebTest.Models.Domain;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AspWebApiGlebTest.Tokens
{
	public class TokenGenerator : ITokenGenerator
	{
		private readonly JwtOptions _options;
		public TokenGenerator(IOptions<JwtOptions> options) => _options = options.Value;

		public string GenerateToken(User user)
		{
			var issuedAt = DateTime.Now;

			//Generating Claims
			var claims = new List<Claim>();

			claims.Add(new Claim("id", user.Id.ToString()));
			claims.Add(new Claim("sub", user.Login));
			claims.Add(new Claim("role", user.Role.Name));
			claims.Add(new Claim("iat", ToUnixEpochDate(DateTime.Now).ToString(), ClaimValueTypes.Integer64));

			//Generating a Token
			var token = new JwtSecurityToken(
				issuer: _options.Issuer,
				audience: _options.Audience,
				claims,
				expires: issuedAt + _options.AccessValidFor,
				signingCredentials: _options.SigningCredentials
				);

			//Returning a Token as string
			return new JwtSecurityTokenHandler().WriteToken(token);
		}
		public string GenerateRefreshToken() => Guid.NewGuid().ToString();

		private static long ToUnixEpochDate(DateTime date)
		{
			var offset = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
			return (long)Math.Round((date.ToUniversalTime() - offset).TotalSeconds);
		}
	}
}
