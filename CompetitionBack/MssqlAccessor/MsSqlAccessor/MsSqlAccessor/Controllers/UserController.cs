using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MsSqlAccessor.Models;

namespace MsSqlAccessor.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{


		private readonly CompetitionBdTestContext _context;
		public UserController(CompetitionBdTestContext context)
		{ 
			_context = context;
		}


		[HttpGet("{id}")]
		public async Task<ActionResult<User>> Get(int id)
		{
			var user = await _context.Users.Where(e => e.Id == id).Select(e => new {
				e.Id,
				e.FirstName,
				e.LastName,
				e.Email,
				e.Github,
				e.RoleId
			}).FirstOrDefaultAsync(); ;
			if (user == null)
				return BadRequest("user not found");
			return Ok(user);
		}

		[HttpGet]
		public async Task<ActionResult<List<User>>> Get()
		{
			return Ok(await _context.Users.Select(e => new {
				e.Id,
				e.FirstName,
				e.LastName,
				e.Email,
				e.Github,
				e.RoleId
			}).ToArrayAsync());
		}

/*
		[HttpPost]
		public async Task<ActionResult<List<User>>> AddUser(User my_user)
		{
			users.Add(my_user);
			return Ok(users);
		}
*//*
		[HttpPut]
		public async Task<ActionResult<List<User>>> UpdateUser(User request)
		{
			var user = users.Find(h => h.id == request.id);
			if (user == null)
				return BadRequest("user not found");

			user.first_name= request.first_name;
			user.last_name= request.last_name;
			user.update_date= request.update_date;
			user.create_date= request.create_date;
			users.Add(user);
			return Ok(users);
		}*/

/*		[HttpDelete]
		public async Task<ActionResult<User>> Delete(int id)
		{
			var user = users.Find(h => h.id == id);
			if (user == null)
				return BadRequest("user not found");
			users.Remove(user);
			return Ok(users);
		}*/
	}
}
