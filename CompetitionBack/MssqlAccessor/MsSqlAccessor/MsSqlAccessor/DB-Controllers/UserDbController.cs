using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MsSqlAccessor.Models;

namespace MsSqlAccessor.DbControllers
{

	//[Route("api/[controller]")]
	//[ApiController]
	public class UserDbController : ControllerBase
	{


		private readonly IHubContext<MssqlHubOld<User>> _hubContext;
		private readonly CompetitionBdTestContext _context;

		public UserDbController(IHubContext<MssqlHubOld<User>> hubContext, CompetitionBdTestContext context)
		{
			_context = context;
			_hubContext = hubContext;
		}

		//[HttpGet]
		public async Task<ActionResult<List<User>>> GetAllUsers()
		{
			var items = await _context.Set<User>().ToListAsync();

			// Send items to hub
			//await _hubContext.Clients.All.SendAsync("getAll", items);

			// Return items to client
			return Ok(items);
		}



		/*
				public async Task<ActionResult<User>> Get(int id)
				{
					var user = await _context.Users.Where(e => e.Id == id).FirstOrDefaultAsync(); ;
					if (user == null)
						return BadRequest("user not found");
					return Ok(user);
				}

				public async Task<ActionResult<List<User>>> Get()
				{
					return Ok(await _context.Users.ToArrayAsync());
				}


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
				}*/
	}
}
