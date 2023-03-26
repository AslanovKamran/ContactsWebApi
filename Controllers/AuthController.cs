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
		public async Task<IActionResult> Login([FromForm] LoginRequest request)
		{
			try
			{
				var user = await _userRepository.LogInUserAsync(request.Login, request.Password);
				if (user is not null)
				{
					//Generate an Access Token
					var accessToken = _tokenGenerator.GenerateToken(user);

					//Generate a RefreshToken
					var refreshToken = new RefreshToken
					{
						Token = _tokenGenerator.GenerateRefreshToken(),
						UserId = user.Id,
						Expires = DateTime.Now + TimeSpan.FromDays(30),
					};

					//Add a new RefreshToken to the Data Base
					await _userRepository.AddRefreshTokenAsync(refreshToken);
					UserTokensDTO userTokens = new(accessToken, refreshToken.Token);
					return Ok(userTokens);
				}
				return BadRequest("Wrong login or password");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}


		/// <summary>
		/// Register a New User and Get the User's Id, Login and Role
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
					return Ok(new { user.Id, user.Login, Role = user.Role.Name });
				}
				catch (Exception ex)
				{
					return BadRequest(new { Error = ex.Message });
				}
			}
			return BadRequest(ModelState);
		}


		/// <summary>
		/// Get a New Pair of Refresh and Access Tokens
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>

		[HttpPost]
		[Route("refresh")]
		[ProducesResponseType(200)]
		[ProducesResponseType(401)]
		public async Task<ActionResult> Refresh(RefreshRequest request)
		{
			//Get OldToken (with mapped user) by refreshToken
			var oldToken = await _userRepository.GetRefreshTokenByToken(request.RefreshToken);
			if (oldToken == null) return Unauthorized();

			else if (oldToken != null && oldToken.Expires < DateTime.Now)
			{
				//RemoveOldToken
				await _userRepository.RemoveOldRefreshToken(oldToken.Token);
				return Unauthorized();
			}
			var user = oldToken?.User;

			//RemoveOldToken
			await _userRepository.RemoveOldRefreshToken(oldToken!.Token);

			//Create new refreshToken and add a new RefreshToken to the DB
			RefreshToken newRefreshToken = new()
			{
				Token = _tokenGenerator.GenerateRefreshToken(),
				UserId = user!.Id,
				Expires = DateTime.Now + TimeSpan.FromDays(30),
			};
			await _userRepository.AddRefreshTokenAsync(newRefreshToken);
			var newAccessToken = _tokenGenerator.GenerateToken(user!);

			UserTokensDTO tokens = new(newAccessToken, newRefreshToken.Token);
			return Ok(tokens);
		}

		/// <summary>
		/// Log out a User (Delete all their Refresh Tokens from the Database)
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>

		[HttpPost]
		[Route("logout")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> LogOut(LogOutRequest request)
		{
			var token = await _userRepository.GetRefreshTokenByToken(request.RefreshToken);
			
			if (token == null || token.User.Login != request.Login)
			{
				return BadRequest(new { LogOutFailure = "Invalid Login or Refresh Token" });
			}

			await _userRepository.RemoveUsersRefreshTokens(token.UserId);
			return Ok(new { Result = "Logged out Successfully" });
		}
	}
}
