using AspWebApiGlebTest.Data;
using AspWebApiGlebTest.Helpers;
using AspWebApiGlebTest.Models.Domain;
using AspWebApiGlebTest.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspWebApiGlebTest.Repository.EFCore
{
	public class UserRepositoryEfCore : IUserRepository
	{
		private readonly ContactsTestDbContext _dbContext;
		public UserRepositoryEfCore(ContactsTestDbContext dbContext) => _dbContext = dbContext;

		//Get All Users
		public async Task<IEnumerable<User>> GetUsersAsync()
		{
			var users = await _dbContext.Users
				.Include(x => x.Role)
				.ToListAsync();
			return users;
		}

		//Get a specific User by their Id
		public async Task<User> GetUserAsync(int id)
		{
			var user = await _dbContext.Users
				.Include(x => x.Role)
				.FirstOrDefaultAsync(x => x.Id == id);
			return user!;
		}

		//Add new User to the Data Base
		public async Task<User> RegisterUserAsync(User user)
		{
			//Validate Login matching
			var users = await GetUsersAsync();
			if (users.Any(x => x.Login.ToLower() == user.Login.ToLower()))
			{
				throw new Exception("This login is occupied, try another one");
			}

			user.Salt = Guid.NewGuid().ToString();
			user.Password = Hasher.HashPassword($"{user.Password}{user.Salt}");

			var userRole = (await _dbContext.Roles.FirstOrDefaultAsync(x => x.Id == user.RoleId))!;
			user.Role = userRole;

			var userEntity = await _dbContext.AddAsync(user);
			await _dbContext.SaveChangesAsync();
			return userEntity.Entity;

		}

		//Verify a User
		public async Task<User> LogInUserAsync(string login, string password)
		{
			var loggedInUser = await _dbContext.Users
				.Include(u => u.Role)
									.FirstOrDefaultAsync(x => x.Login == login);
			if (loggedInUser != null && Hasher.HashPassword($"{password}{loggedInUser.Salt}") == loggedInUser.Password)
			{

				return loggedInUser;
			}
			else return null!;
		}

		//Add new RefreshToken to the Data Base
		public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
		{
			var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == refreshToken.UserId);
			refreshToken.User = user!;
			await _dbContext.RefreshTokens.AddAsync(refreshToken);
			await _dbContext.SaveChangesAsync();
		}

		//Get a specific RefreshToken by Token
		public async Task<RefreshToken> GetRefreshTokenByToken(string token)
		{
			var refreshToken = await _dbContext.RefreshTokens
				.Include(x=>x.User)
				.Include(x=>x.User.Role)
				.FirstOrDefaultAsync(x => x.Token == token);

			return refreshToken!;
		}

		//Remove old RefreshToken by Token from the Data Base
		public async Task RemoveOldRefreshToken(string token)
		{
			var oldToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
			if (oldToken is not null)
			{
				_dbContext.Remove(oldToken);
				await _dbContext.SaveChangesAsync();
			}
		}

		//Remove all Users Refresh Tokens (when Loggin out)
		public async Task RemoveUsersRefreshTokens(int userId)
		{
			var tokens = _dbContext.RefreshTokens.Where(x => x.UserId == userId);
			_dbContext.RemoveRange(tokens);
			await _dbContext.SaveChangesAsync();

		}

		//Change a User Password based on their old credentials
		public async Task ChangePasswordAsync(string login, string oldPassword, string newPassword)
		{
			var user = await LogInUserAsync(login, oldPassword);
			if(user == null) throw new Exception("Wrong user creds");

			newPassword = Hasher.HashPassword($"{newPassword}{user.Salt}");
			user.Password = newPassword;
			_dbContext.Users.Update(user);
			await _dbContext.SaveChangesAsync();

		}
	}
}
