using AspWebApiGlebTest.Models;

namespace AspWebApiGlebTest.Tokens
{
	public interface ITokenGenerator
	{
		string GenerateToken(User user);
		public string GenerateRefreshToken();
	}
}
