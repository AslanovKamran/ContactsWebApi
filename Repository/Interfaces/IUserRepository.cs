using AspWebApiGlebTest.Models;

namespace AspWebApiGlebTest.Repository.Interfaces
{
	public interface IUserRepository
	{
		Task<IEnumerable<User>> GetUsersAsync();
		Task<User> GetUserAsync(int id);
		Task<User> AddUserAsync(User user);

		Task<User> AuthenticateUserAsync(string login, string password);
	}
}
