using Azure.Core;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MsSqlAccessor.Models;

namespace MsSqlAccessor.Hubs
{
    public class TestHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly CompetitionBdTestContext _context;
        public TestHub(CompetitionBdTestContext context)
        {
            _context = context;

        }

        public async System.Threading.Tasks.Task GetAll(string user, string message)
        {
            var items = await _context.Users.ToArrayAsync();

            await Clients.All.SendAsync("ReceiveUsers", items);
        }

        public async System.Threading.Tasks.Task GetOne(int id)
        {
            var user = await _context.Users.Where(e => e.Id == id).FirstOrDefaultAsync();

            await Clients.All.SendAsync("ReceiveUser", user);
        }

        public async System.Threading.Tasks.Task UpdateOne(int id, User request)
        {
            var dbUser = await _context.Users.FindAsync(id); // ADD HANDLE

            dbUser.UpdateDate = DateTime.Now;
            dbUser.FirstName = request.FirstName;
            dbUser.LastName = request.LastName;
            dbUser.Email = request.Email;
            dbUser.Github = request.Github;
            dbUser.Login = request.Login;
            dbUser.Password = request.Password;
            dbUser.RoleId = request.RoleId;

            await _context.SaveChangesAsync();

            await Clients.All.SendAsync("UpdateUser", dbUser);
        }

        public async System.Threading.Tasks.Task DeleteOne(int id)
        {
            var user = await _context.Users.FindAsync(id); // ADD HANDLE

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            await Clients.All.SendAsync("DeleteUser", user);
        }


    }
}