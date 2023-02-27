using System.Security.Cryptography;
using System.Text;

namespace AspWebApiGlebTest.Helpers
{
	public static class Hasher
	{
		public static string HashPassword(string password)
		{
			SHA256 hash = SHA256.Create();
			var hashedPasswordBytes = hash.ComputeHash(Encoding.UTF8.GetBytes(password));
			return Convert.ToBase64String(hashedPasswordBytes);
		}
	}
}
