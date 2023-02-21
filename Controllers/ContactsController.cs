using AspWebApiGlebTest.Models;
using AspWebApiGlebTest.Models.DTOs;
using AspWebApiGlebTest.Repository.cs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspWebApiGlebTest.Controllers
{

	[ApiController]
	[Route("api/[controller]")]
	[Produces("application/json")]

	public class ContactsController : ControllerBase
	{
		private readonly IContactRepository _repos;
		public ContactsController(IContactRepository repos) => _repos = repos;





		/// <summary>
		/// Gets A List Of Contacts
		/// </summary>
		/// <returns>List of contacts</returns>

		[HttpGet]
		[ProducesResponseType(200)]
		[ProducesResponseType(401)]
		[Authorize(Roles ="User,Admin")]
		public async Task <IActionResult>GetCotacts()
		{
			var contacts = await _repos.GetContactsAsync();
			return Ok(contacts);
		}

		/// <summary>
		/// Gets A Specific Contact by id
		/// </summary>
		/// <param name="id">Valid Contact Id</param>
		/// <returns>Contact</returns>

		[HttpGet]
		[Route("{id}")]
		[ProducesResponseType(200)]
		[ProducesResponseType(401)]
		[ProducesResponseType(404)]
		[Authorize(Roles = "User,Admin")]
		public async Task<IActionResult> GetContact(int id)
		{
			var contact = await _repos.GetContactAsync(id);
			return contact == null ? NotFound("No Such A Contact") : Ok(contact);
		}

		/// <summary>
		/// Inserts A Contact
		/// </summary>
		/// <param name="contactDTO">Contact Example</param>
		/// <returns></returns>

		[HttpPost]
		[ProducesResponseType(201)]
		[ProducesResponseType(400)]
		[ProducesResponseType(401)]
		[ProducesResponseType(403)]
		[Authorize(Roles = "Admin")]
		public async  Task<IActionResult> PostContact(PostContactDTO contactDTO)
		{
			
			if (ModelState.IsValid)
			{
				Contact contact = new Contact()
			{
				Name = contactDTO.Name,
				Surname = contactDTO.Surname,
				Phone = contactDTO.Phone,
				Email = contactDTO.Email,
				IsFavorite = contactDTO.IsFavorite,
			};
				contact = await _repos.AddContactAsync(contact);
				return CreatedAtAction(nameof(GetContact),new { id = contact.Id}, contact);
			}
			return BadRequest(ModelState);
		}

		/// <summary>
		/// Updates A Contact
		/// </summary>
		/// <param name="contact">Contact</param>
		/// <returns></returns>

		[HttpPut]
		[ProducesResponseType(201)]
		[ProducesResponseType(400)]
		[ProducesResponseType(401)]
		[ProducesResponseType(403)]
		[ProducesResponseType(404)]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> PutContact(Contact contact)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var updatedContact = await _repos.UpdateContactAsync(contact);
					return CreatedAtAction(nameof(GetContact),new { id = updatedContact.Id }, updatedContact);
				}
				catch (Exception)
				{
					return NotFound();
				}

			}
			return BadRequest(ModelState);
		}

		/// <summary>
		/// Deletes A Specific Contact
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		
		[HttpDelete]
		[Route("{id}")]
		[ProducesResponseType(204)]
		[ProducesResponseType(401)]
		[ProducesResponseType(403)]
		[ProducesResponseType(404)]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteContact(int id)
		{
			var contact = await _repos.GetContactAsync(id);
			if (contact is not null)
			{
				await _repos.DeleteContactAsync(id);
				return NoContent();
			}
			return NotFound();
		}

	}

}
