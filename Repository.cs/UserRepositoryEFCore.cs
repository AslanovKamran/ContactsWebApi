using AspWebApiGlebTest.Models;
using Microsoft.EntityFrameworkCore;

namespace AspWebApiGlebTest.Repository.cs
{
	public class UserRepositoryEFCore : IUserRepository
	{
		private readonly ContactsDbContext _context;
		public UserRepositoryEFCore(ContactsDbContext context)
		{
			_context = context;
		}

		//Get All Users
		public async Task<IEnumerable<User>> GetUsersAsync()
		{
			return await _context.Users
				.Include(x=>x.Role)
				.ToListAsync();
		}

		//Get a Specific User
		public async Task<User> GetUserAsync(int id)
		{
			var user = await _context.Users
				.Include(x => x.Role)
				.FirstOrDefaultAsync(x => x.Id == id);

			return user!;
		}

		//Add a User
		public async Task<User> AddUserAsync(User user)
		{
			await _context.Users.AddAsync(user);
			await _context.SaveChangesAsync();
			return user;
		}

		//Authenticate a User
		public async Task<User> AuthenticateUserAsync(string login, string password)
		{
			var user = await _context.Users
				.Include(x => x.Role)
				.FirstOrDefaultAsync(x => x.Login == login && x.Password == password );

			return user!;
		}

		

		
	}
}
