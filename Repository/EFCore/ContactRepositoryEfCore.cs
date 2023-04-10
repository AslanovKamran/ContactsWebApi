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
		
		public async Task<Contact> AddContactAsync(Contact contact)
		{
			var added = await _dbContext.Contacts.AddAsync(contact);
			await _dbContext.SaveChangesAsync();
			return added.Entity;
		}

		public async Task DeleteContactAsync(int id)
		{
			var contactToDelete = await _dbContext.Contacts.FirstOrDefaultAsync(x => x.Id == id);
			if (contactToDelete is not null) 
			{
				_dbContext.Contacts.Remove(contactToDelete);
				await _dbContext.SaveChangesAsync();
			}
		}

		public async Task<Contact> GetContactAsync(int id)
		{
			var contact = await _dbContext.Contacts.FirstOrDefaultAsync(x => x.Id == id);
			return contact!;

		}

		public async Task<IEnumerable<Contact>> GetContactsAsync()
		{
			return await _dbContext.Contacts.ToListAsync();
		}

		public async Task<Contact> UpdateContactAsync(Contact contact)
		{
			var updated = _dbContext.Update(contact).Entity;
			await _dbContext.SaveChangesAsync();
			return updated;
		}
	}
}
