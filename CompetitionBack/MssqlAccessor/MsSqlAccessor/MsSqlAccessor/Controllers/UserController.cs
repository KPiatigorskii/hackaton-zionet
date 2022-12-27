using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MsSqlAccessor.Models;

namespace MsSqlAccessor.Controllers
{

	public class UserWithoutRelatioon {

		public int Id { get; set; }

		public string Email { get; set; } = null!;

		public string? Github { get; set; }

		public string FirstName { get; set; } = null!;

		public string LastName { get; set; } = null!;

		public string Login { get; set; } = null!;

		public string Password { get; set; } = null!;

		public int RoleId { get; set; }
	}

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
			var user = await _context.Users.Where(e => e.Id == id).FirstOrDefaultAsync(); ;
			if (user == null)
				return BadRequest("user not found");
			return Ok(user);
		}

		[HttpGet]
		public async Task<ActionResult<List<User>>> Get()
		{
			return Ok(await _context.Users.ToArrayAsync());
		}


		[HttpPost]
		public async Task<ActionResult<List<User>>> AddUser([FromBody]User user)
		{
			user.CreateDate= DateTime.Now;
			user.UpdateDate= DateTime.Now;
			user.UpdateUserId = 1;
			user.CreateUserId= 1;
			_context.Users.Add(user);

			await _context.SaveChangesAsync();
			return Ok(await _context.Users.ToArrayAsync());
		}

		[HttpPut]
		public async Task<ActionResult<List<User>>> UpdateUser(User request)
		{
			var user = await _context.Users.FindAsync(request.Id);
			if (user == null)
				return BadRequest("user not found");
			user.UpdateDate = DateTime.Now;
			user.FirstName= request.FirstName;
			user.LastName= request.LastName;
			user.Email= request.Email;
			user.Github= request.Github;
			user.Login= request.Login;
			user.Password= request.Password;
			user.RoleId= request.RoleId;

			await _context.SaveChangesAsync();
			return Ok(await _context.Users.ToArrayAsync());
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<User>> Delete(int id)
		{
			var user = await _context.Users.FindAsync(id);
			if (user == null)
				return BadRequest("user not found");
			_context.Users.Remove(user);
			await _context.SaveChangesAsync();

			return Ok(await _context.Users.ToArrayAsync());
		}
	}
}
