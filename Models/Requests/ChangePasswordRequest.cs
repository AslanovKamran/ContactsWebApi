using System.ComponentModel.DataAnnotations;

namespace AspWebApiGlebTest.Models.Requests
{
	public class ChangePasswordRequest
	{
		[Required(AllowEmptyStrings = false)]
		[MaxLength(100)]
		public string Login { get; set; } = string.Empty;

		[Required(AllowEmptyStrings = false)]
		[MaxLength(100)]
		public string OldPassword { get; set; } = string.Empty;

		[Required(AllowEmptyStrings = false)]
		[MaxLength(100)]
		public string NewPassword { get; set; } = string.Empty;
	}
}
