using AspWebApiGlebTest.Models.Domain;

namespace AspWebApiGlebTest.Tokens
{
	public interface ITokenGenerator
	{
		string GenerateToken(User user);
		public string GenerateRefreshToken();
	}
}
