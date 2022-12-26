using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MsSqlAccessor.Models;

namespace MsSqlAccessor.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private static List<User> users = new List<User>
			{
				new User { id = 1,
					first_name = "qwer",
					last_name = "asdf",
					create_date = "132412341234",
					update_date = "12312312312"
					},
				new User { id = 2,
					first_name = "qwer",
					last_name = "asdf",
					create_date = "132412341234",
					update_date = "12312312312"
					}
				};

		[HttpGet("{id}")]
		public async Task<ActionResult<User>> Get(int id) 
		{
			var user = users.Find(h => h.id == id);
			if (user == null)
				return BadRequest("user not found");
			return Ok(user);
		}

		[HttpGet]
		public async Task<ActionResult<List<User>>> Get()
		{
			return Ok(users);
		}


		[HttpPost]
		public async Task<ActionResult<List<User>>> AddUser(User my_user)
		{
			users.Add(my_user);
			return Ok(users);
		}

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
		}

		[HttpDelete]
		public async Task<ActionResult<User>> Delete(int id)
		{
			var user = users.Find(h => h.id == id);
			if (user == null)
				return BadRequest("user not found");
			users.Remove(user);
			return Ok(users);
		}
	}
}
