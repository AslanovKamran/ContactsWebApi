using AspWebApiGlebTest.Models.DTOs;
using AspWebApiGlebTest.Repository.cs;
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
		public async Task<IActionResult> Login(LoginRequest request)
		{
			var user = await _userRepository.AuthenticateUserAsync(request.Login, request.Password);
			if (user is not null)
			{
				return Ok(new { AccessToken = _tokenGenerator.GenerateToken(user) });
			}
			return BadRequest("Wrong login or password");
		}
	}
}
