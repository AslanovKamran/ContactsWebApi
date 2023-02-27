using System.ComponentModel.DataAnnotations;

namespace AspWebApiGlebTest.Models
{
	public class Contact
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(100)]
		public string Name { get; set; } = string.Empty;

		[Required]
		[MaxLength(100)]
		public string Surname { get; set; } = string.Empty;

		[Phone]
		public string? Phone { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;

		public bool? IsFavorite { get; set; }

		public Contact(int id, string name, string surname, string phone, string email, bool isFavorite)
		{
			Id = id;
			Name = name;
			Surname = surname;
			Phone = phone;
			Email = email;
			IsFavorite = isFavorite;
		}
		public Contact() { }
	}
}
