using AspWebApiGlebTest.Helpers;
using AspWebApiGlebTest.Models;
using AspWebApiGlebTest.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspWebApiGlebTest.Repository
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

		//Regiseter a User (Works Improperly - Adds new role to the database)
		public async Task<User> RegisterUserAsync(User user)
		{
			user.Salt = Guid.NewGuid().ToString();
			user.Password = Hasher.HashPassword($"{user.Password}{user.Salt}");

			await _context.Users.AddAsync(new User 
			{
				Login = user.Login,
				Password = user.Password,
				RoleId = user.RoleId,
				Salt = user.Salt
			});
			await _context.SaveChangesAsync();
			return user;
		}

		//Log In a User
		public async Task<User> LogInUserAsync(string login, string password)
		{
			

			var userFound = _context?.Users?.Any(x => x.Login == login);
			if ((userFound != null) && userFound == true)
			{
				var loggedInUser = await _context?.Users?.Include(x=>x.Role).FirstOrDefaultAsync(x => x.Login == login)!;

				if (Hasher.HashPassword($"{password}{loggedInUser!.Salt}") == loggedInUser.Password)
				{
					return loggedInUser;
				}
				return null!;
				
			}
			return null!;
			
		}

		

		
	}
}
