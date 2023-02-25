using AspWebApiGlebTest.Models;
using AspWebApiGlebTest.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspWebApiGlebTest.Repository
{
	public class ContactRepositoryEFCore : IContactRepository
	{
		private readonly ContactsDbContext _context;
		public ContactRepositoryEFCore(ContactsDbContext context) => _context = context;
	


		//Get All Contacts
		public async Task<IEnumerable<Contact>> GetContactsAsync()
		{
			return await _context.Contacts.ToListAsync();
		}

		//Get a specific Contact
		public async Task<Contact> GetContactAsync(int id)
		{
			var contact = await _context.Contacts.FirstOrDefaultAsync(cont => cont.Id == id);
			return contact!;
		}

		//Add a Contact
		public async Task<Contact> AddContactAsync(Contact contact)
		{
			await _context.Contacts.AddAsync(contact);
			await _context.SaveChangesAsync();
			return contact;
		}

		//Delete a Contact
		public async Task DeleteContactAsync(int id)
		{
			var contactToDelete = await _context.Contacts.FirstOrDefaultAsync(cont => cont.Id == id);
			if (contactToDelete is not null) 
			{
				_context.Contacts.Remove(contactToDelete);
				await _context.SaveChangesAsync();
			}
		}

		//Update a contact
		public async Task<Contact> UpdateContactAsync(Contact contact)
		{
			_context.Contacts.Update(contact);
			await _context.SaveChangesAsync();
			return contact;
		}

		
	}
}
