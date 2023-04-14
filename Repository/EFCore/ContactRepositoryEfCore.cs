using AspWebApiGlebTest.Data;
using AspWebApiGlebTest.Models.Domain;
using AspWebApiGlebTest.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspWebApiGlebTest.Repository.EFCore
{
	public class ContactRepositoryEfCore : IContactRepository
	{
		private readonly ContactsTestDbContext _dbContext;
		public ContactRepositoryEfCore(ContactsTestDbContext dbContext) => _dbContext = dbContext;

		//Get All Contacts
		public async Task<IEnumerable<Contact>> GetContactsAsync(int itemsPerPage, int currentPage)
		{
			int skip = (currentPage - 1) * itemsPerPage;
			int take = itemsPerPage;
			return await _dbContext.Contacts.Skip(skip).Take(take).ToListAsync();
		}

		//Get a specific Contact
		public async Task<Contact> GetContactAsync(int id)
		{
			var contact = await _dbContext.Contacts.FirstOrDefaultAsync(x => x.Id == id);
			return contact!;

		}

		//Add a Contact
		public async Task<Contact> AddContactAsync(Contact contact)
		{
			var added = await _dbContext.Contacts.AddAsync(contact);
			await _dbContext.SaveChangesAsync();
			return added.Entity;
		}

		//Delete a Contact
		public async Task DeleteContactAsync(int id)
		{
			var contactToDelete = await _dbContext.Contacts.FirstOrDefaultAsync(x => x.Id == id);
			if (contactToDelete is not null)
			{
				_dbContext.Contacts.Remove(contactToDelete);
				await _dbContext.SaveChangesAsync();
			}
		}

		//Udpate a Contact
		public async Task<Contact> UpdateContactAsync(Contact contact)
		{
			var updated = _dbContext.Update(contact).Entity;
			await _dbContext.SaveChangesAsync();
			return updated;
		}

		//Add range of contacts
		public async Task BulkInsertAsync(List<Contact> contacts)
		{
			await _dbContext.AddRangeAsync(contacts);
			await _dbContext.SaveChangesAsync();
		}

		//Get Contacts count
		public async Task<int> GetContactsCountAsync()
		{
			var totalCount = await _dbContext.Contacts.CountAsync();
			return totalCount;
		}
	}
}
