using System.ComponentModel.DataAnnotations;

namespace AspWebApiGlebTest.Models
{
	public class User
	{
		[Key]
		public int Id { get; set; }

		[Required(AllowEmptyStrings =false)]
		[MaxLength(100)]
		public string Login { get; set; } = string.Empty;

		[Required(AllowEmptyStrings =false)]
		[MaxLength(100)]
		public string Password { get; set; } = string.Empty;

		[Required]
		public int RoleId { get; set; }

		[Required]
		[MaxLength(255)]
		public string Salt { get; set; } = string.Empty;


		//Nav Properties 
		public Role Role { get; set; } = new();
		//public virtual ICollection<RefreshToken> RefreshTokens { get; } = new List<RefreshToken>();

	}
}
