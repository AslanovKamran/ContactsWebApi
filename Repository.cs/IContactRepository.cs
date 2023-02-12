using AspWebApiGlebTest.Models;

namespace AspWebApiGlebTest.Repository.cs
{
	public interface IContactRepository
	{
		Task<IEnumerable<Contact>> GetContactsAsync();
		Task<Contact> GetContactAsync(int id);

		Task<Contact> AddContactAsync(Contact contact);
		Task<Contact> UpdateContactAsync(Contact contact);
		Task DeleteContactAsync(int id);

	
	}
}
