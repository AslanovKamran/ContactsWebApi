using Microsoft.EntityFrameworkCore;

namespace AspWebApiGlebTest.Models
{
	public class ContactsDbContext: DbContext
	{
		public ContactsDbContext(DbContextOptions options):base(options)
		{
		}
		public DbSet<Contact> Contacts{ get; set; }
	}
}
