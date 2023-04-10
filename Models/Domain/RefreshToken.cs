namespace AspWebApiGlebTest.Models.Domain
{
	public class RefreshToken
	{
		public int Id { get; set; }

		public string Token { get; set; } = string.Empty;

		public int UserId { get; set; }

		public DateTime Expires { get; set; }

		//Navigation Property
		public virtual User User { get; set; } = new();
	}
}
