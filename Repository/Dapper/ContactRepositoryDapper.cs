using AspWebApiGlebTest.Models;
using AspWebApiGlebTest.Repository.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AspWebApiGlebTest.Repository.Dapper
{
	public class ContactRepositoryDapper : IContactRepository
	{

		private readonly string _connectionString;
		public ContactRepositoryDapper(string connectionString) => _connectionString = connectionString;

		//Get All Contacts
		public async Task<IEnumerable<Contact>> GetContactsAsync()
		{
			using (IDbConnection db = new SqlConnection(_connectionString))
			{
				string query = @"exec GetAllContacts";
				return (await db.QueryAsync<Contact>(query, null)).ToList();
			}
		}

		//Get a specific Contact
		public async Task<Contact> GetContactAsync(int id)
		{
			using (IDbConnection db = new SqlConnection(_connectionString))
			{
				var parameters = new DynamicParameters();
				parameters.Add("Id", id, DbType.Int32, ParameterDirection.Input);

				string query = @"exec GetSingleContact @Id = @id";
				return await db.QueryFirstOrDefaultAsync<Contact>(query, parameters);
			}
		}

		//Add a Contact
		public async Task<Contact> AddContactAsync(Contact contact)
		{
			using (IDbConnection db = new SqlConnection(_connectionString))
			{
				var parameters = new DynamicParameters();
				parameters.Add("Name", contact.Name, DbType.String, ParameterDirection.Input);
				parameters.Add("Surname", contact.Surname, DbType.String, ParameterDirection.Input);
				parameters.Add("Phone", contact.Phone, DbType.String, ParameterDirection.Input);
				parameters.Add("Email", contact.Email, DbType.String, ParameterDirection.Input);
				parameters.Add("IsFavorite", contact.IsFavorite, DbType.Boolean, ParameterDirection.Input);

				string query = @"exec AddContact @Name, @Surname, @Phone, @Email, @IsFavorite";
				var insertedContact = await db.QuerySingleAsync<Contact>(query, parameters);
				return insertedContact;
			}

		}

		//Delete a Contact
		public async Task DeleteContactAsync(int id)
		{
			using (IDbConnection db = new SqlConnection(_connectionString))
			{
				var parameters = new DynamicParameters();
				parameters.Add("Id", id, DbType.Int32, ParameterDirection.Input);

				string query = @"DeleteContact @Id = @id";
				await db.ExecuteAsync(query, parameters);
			}
		}

		//Update a contact
		public async Task<Contact> UpdateContactAsync(Contact contact)
		{
			using (IDbConnection db = new SqlConnection(_connectionString))
			{
				var parameters = new DynamicParameters();
				parameters.Add("Id", contact.Id, DbType.Int32, ParameterDirection.Input);
				parameters.Add("Name", contact.Name, DbType.String, ParameterDirection.Input);
				parameters.Add("Surname", contact.Surname, DbType.String, ParameterDirection.Input);
				parameters.Add("Phone", contact.Phone, DbType.String, ParameterDirection.Input);
				parameters.Add("Email", contact.Email, DbType.String, ParameterDirection.Input);
				parameters.Add("IsFavorite", contact.IsFavorite, DbType.Boolean, ParameterDirection.Input);

				string query = @"exec UpdateContact @Id, @Name, @Surname, @Phone, @Email, @IsFavorite";
				var updatedContact = await db.QuerySingleAsync<Contact>(query, parameters);
				return updatedContact;
			}
		}
	}
}
