using AspWebApiGlebTest.Models;
using AspWebApiGlebTest.Models.DTOs;
using AspWebApiGlebTest.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspWebApiGlebTest.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Produces("application/json")]
	public class UsersController : ControllerBase
	{
		private readonly IUserRepository _repos;
		public UsersController(IUserRepository repos) => _repos = repos;

		/// <summary>
		/// Gets A List Of Users
		/// </summary>
		/// <returns>List of users</returns>

		[HttpGet]
		[ProducesResponseType(200)]
		[ProducesResponseType(401)]
		[ProducesResponseType(403)]
		//[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetUsers()
		{
			var users = await _repos.GetUsersAsync();
			return Ok(users);
		}

		
		/// <summary>
		/// Gets A Specific User by id
		/// </summary>
		/// <param name="id">Valid User Id</param>
		/// <returns>Contact</returns>

		[HttpGet]
		[Route("{id}")]
		[ProducesResponseType(200)]
		[ProducesResponseType(401)]
		[ProducesResponseType(403)]
		[ProducesResponseType(404)]
		//[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetUser(int id)
		{
			var user = await _repos.GetUserAsync(id);
			return user == null ? NotFound("No Such A User") : Ok(user);
		}

	}
}
