using System.ComponentModel.DataAnnotations;

namespace AspWebApiGlebTest.Models.Requests
{
	public class LoginRequest
	{
		[Required(AllowEmptyStrings = false)]
		[MaxLength(100)]
		public string Login { get; set; } = string.Empty;

		[Required(AllowEmptyStrings = false)]
		[MaxLength(100)]
		public string Password { get; set; } = string.Empty;
	}
}
