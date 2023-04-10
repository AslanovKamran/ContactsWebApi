using System.ComponentModel.DataAnnotations;

namespace AspWebApiGlebTest.Models.Requests
{
	public class RefreshRequest
	{
		[Required(AllowEmptyStrings = false)]
		public string RefreshToken { get; set; } = string.Empty;
	}
}
