using System.ComponentModel.DataAnnotations;

namespace AspWebApiGlebTest.Models.DTOs
{
	public class LogOutRequest
	{
		[Required(AllowEmptyStrings = false)]
		[MaxLength(100)]
		public string Login { get; set; } = string.Empty;

		[Required(AllowEmptyStrings = false)]
		public string RefreshToken { get; set; } = string.Empty;
	}
}
