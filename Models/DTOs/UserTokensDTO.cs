namespace AspWebApiGlebTest.Models.DTOs
{
	public class UserTokensDTO
	{
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }

		public UserTokensDTO(string accessToken, string refreshToken)
		{
			AccessToken = accessToken;
			RefreshToken = refreshToken;
		}
	}
}
