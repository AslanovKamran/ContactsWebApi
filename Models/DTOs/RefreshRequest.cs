using System.ComponentModel.DataAnnotations;

namespace AspWebApiGlebTest.Models.DTOs
{
	public class RefreshRequest
	{
		[Required(AllowEmptyStrings = false)]
		public string RefreshToken { get; set; } = string.Empty;
	}
}
