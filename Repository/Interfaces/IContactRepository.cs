using AspWebApiGlebTest.Models.Domain;

namespace AspWebApiGlebTest.Repository.Interfaces
{
	public interface IContactRepository
	{
		Task<IEnumerable<Contact>> GetContactsAsync(int itemsPerPage, int currentPage);
		Task<Contact> GetContactAsync(int id);

		Task<Contact> AddContactAsync(Contact contact);
		Task<Contact> UpdateContactAsync(Contact contact);
		Task DeleteContactAsync(int id);
		Task<int> GetContactsCountAsync();

		Task BulkInsertAsync(List<Contact> contacts);

		
	}
}
