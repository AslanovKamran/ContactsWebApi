using System.ComponentModel.DataAnnotations;

namespace AspWebApiGlebTest.Models.DTOs
{
	public class PostContactDTO
	{
		[Required]
		[MaxLength(100)]
		public string Name { get; set; } = string.Empty;

		[Required]
		[MaxLength(100)]
		public string Surname { get; set; } = string.Empty;

		[Phone]
		public string? Phone { get; set; }

		[EmailAddress]
		public string Email { get; set; } = string.Empty;

		public bool? IsFavorite { get; set; }
	}
}
