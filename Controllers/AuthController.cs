using AspWebApiGlebTest.Models;
using AspWebApiGlebTest.Models.DTOs;
using AspWebApiGlebTest.Repository.Interfaces;
using AspWebApiGlebTest.Tokens;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspWebApiGlebTest.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Produces("application/json")]
	public class AuthController : ControllerBase
	{
		private readonly ITokenGenerator _tokenGenerator;
		private readonly IUserRepository _userRepository;
		public AuthController(ITokenGenerator tokenGenerator, IUserRepository userRepository)
		{
			_tokenGenerator = tokenGenerator;
			_userRepository = userRepository;
		}

		/// <summary>
		///	Log In User and Get an Access Token
		/// </summary>
		/// <param name="request">Login and Password</param>
		/// <returns></returns>

		[HttpPost]
		[Route("login")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		public async Task<IActionResult> Login([FromForm]LoginRequest request)
		{
			try
			{
				var user = await _userRepository.LogInUserAsync(request.Login, request.Password);
				if (user is not null)
				{
					return Ok(new { AccessToken = _tokenGenerator.GenerateToken(user) });
				}
				return BadRequest("Wrong login or password");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}

		}


		/// <summary>
		/// Register a New User and Get an Access Token with Inserted User Data
		/// </summary>
		/// <param name="userDTO">Valid Credentials</param>
		/// <returns>New User</returns>

		[HttpPost]
		[Route("register")]
		[ProducesResponseType(201)]
		[ProducesResponseType(400)]
		public async Task<IActionResult> Register([FromForm] PostUserDTO userDTO)
		{
			if (ModelState.IsValid)
			{
				User user = new User
				{
					Login = userDTO.Login,
					Password = userDTO.Password,
					RoleId = userDTO.RoleId
				};
				try
				{
					user = await _userRepository.RegisterUserAsync(user);
					return Ok(new { AccessToken = _tokenGenerator.GenerateToken(user), User = user });
				}
				catch (Exception ex)
				{
					return BadRequest(ex.Message);
				}

			}
			return BadRequest(ModelState);
		}
	}
}
