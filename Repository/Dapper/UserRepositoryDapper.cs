using AspWebApiGlebTest.Helpers;
using AspWebApiGlebTest.Models;
using AspWebApiGlebTest.Repository.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AspWebApiGlebTest.Repository.Dapper
{
	public class UserRepositoryDapper : IUserRepository
	{
		private readonly string _connectionString;
		public UserRepositoryDapper(string connectionString) => _connectionString = connectionString;


		//Get All Users
		public async Task<IEnumerable<User>> GetUsersAsync()
		{
			using (IDbConnection db = new SqlConnection(_connectionString))
			{
				string query = @"exec GetAllUsers";
				var users = (await db.QueryAsync<User, Role, User>(query, (user, role) =>
				{
					user.Role = role;
					return user;
				})).ToList();
				return users;
			}
		}

		public async Task<User> GetUserAsync(int id)
		{
			using (IDbConnection db = new SqlConnection(_connectionString))
			{
				var parameters = new DynamicParameters();
				parameters.Add("Id", id, DbType.Int32, ParameterDirection.Input);

				string query = "exec GetSingleUser @Id = @id";

				var user = (await db.QueryAsync<User, Role, User>(query, (user, role) =>
				{
					user.Role = role;
					return user;
				},parameters)).FirstOrDefault();

				return user!;
			}
		}

		public async Task<User> RegisterUserAsync(User user)
		{
			user.Salt = Guid.NewGuid().ToString();
			user.Password = Hasher.HashPassword($"{user.Password}{user.Salt}");

			using (IDbConnection db = new SqlConnection(_connectionString))
			{
				
				var parameters = new DynamicParameters();

				parameters.Add("Login", user.Login, DbType.String, ParameterDirection.Input);
				parameters.Add("Password", user.Password, DbType.String, ParameterDirection.Input);
				parameters.Add("RoleId", user.RoleId, DbType.String, ParameterDirection.Input);
				parameters.Add("Salt", user.Salt, DbType.String, ParameterDirection.Input);

				string query = @"exec AddUser @Login, @Password,@RoleId, @Salt";

				var insertedUserId = await db.QuerySingleAsync<int>(query, parameters);

				var insertedUser = await GetUserAsync(insertedUserId);
				return insertedUser;

			}
		}

		public async Task<User> LogInUserAsync(string login, string password)
		{
			using (IDbConnection db = new SqlConnection(_connectionString))
			{

				var parameters = new DynamicParameters();
				parameters.Add("Login", login, DbType.String, ParameterDirection.Input);
				

				string query = @"exec GetUserByLogin @Login";
				var loggedInUser = (await db.QueryAsync<User, Role, User>(query, (user, role) =>
				{
					user.Role = role;
					return user;
				}, parameters)).FirstOrDefault();
				if (loggedInUser != null && Hasher.HashPassword($"{password}{loggedInUser.Salt}") == loggedInUser.Password)
				{
					return loggedInUser;
				}
				else return null!;
			}
		}




	}
}
