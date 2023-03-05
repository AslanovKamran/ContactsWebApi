using AspWebApiGlebTest.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AspWebApiGlebTest.Tokens
{
	public class TokenGenerator : ITokenGenerator
	{
		private readonly IConfiguration _config;
		public TokenGenerator(IConfiguration config) => _config = config;

		public string GenerateToken(User user)
		{
			//Generating Claims
			var claims = new List<Claim>();
			claims.Add(new Claim("name", user.Login));
			claims.Add(new Claim("role", user.Role.Name));
			

			//Generating Credentials
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			//Generating token
			var token = new JwtSecurityToken(
				_config["Jwt:Issuer"],
				_config["Jwt:Audience"],
				claims,
				expires: DateTime.Now.AddMinutes(15),
				signingCredentials: creds
				);
			return new JwtSecurityTokenHandler().WriteToken(token);

		}
	}
}
