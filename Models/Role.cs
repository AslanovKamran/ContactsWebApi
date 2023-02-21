using System.ComponentModel.DataAnnotations;

namespace AspWebApiGlebTest.Models
{
	public class Role
	{
		[Key]
		public int Id { get; set; }

		[Required(AllowEmptyStrings =false)]
		[MaxLength(100)]
		public string Name { get; set; } = string.Empty;
	}
}
