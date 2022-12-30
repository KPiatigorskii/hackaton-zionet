using Azure.Core;
//using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MsSqlAccessor.Enums;
using MsSqlAccessor.Models;

namespace MsSqlAccessor.Hubs
{
    public class UserHub : Hub
    {
        private readonly CompetitionBdTestContext _context;
        public UserHub(CompetitionBdTestContext context)
        {
            _context = context;

        }

        public async System.Threading.Tasks.Task GetAll()
        {
            var dbItems = await _context.Users.ToArrayAsync();

            await Clients.All.SendAsync("ReceiveUsers", dbItems);
        }

        public async System.Threading.Tasks.Task GetOne(int id)
        {
            var dbItem = await _context.Users.Where(e => e.Id == id).FirstOrDefaultAsync();

            await Clients.All.SendAsync("ReceiveUser", dbItem);
        }

        public async System.Threading.Tasks.Task UpdateOne(int id, User request)
        {
            var dbItem = await _context.Users.FindAsync(id); // ADD HANDLE

            
            dbItem.FirstName = !string.IsNullOrWhiteSpace(request.FirstName) ? request.FirstName : dbItem.FirstName;
            dbItem.LastName = !string.IsNullOrWhiteSpace(request.LastName) ? request.LastName : dbItem.LastName;
            dbItem.Email = !string.IsNullOrWhiteSpace(request.Email) ? request.Email : dbItem.Email;
            dbItem.Github = request.Github;
            dbItem.Login = !string.IsNullOrWhiteSpace(request.Login) ? request.Login : dbItem.Login;
            dbItem.Password = !string.IsNullOrWhiteSpace(request.Password) ? request.Password : dbItem.Password;
            dbItem.RoleId = !(request.RoleId == 0) ? request.RoleId : dbItem.RoleId;
            dbItem.UpdateDate = DateTime.Now;
            dbItem.UpdateUserId = request.UpdateUserId;
            dbItem.StatusId = !(request.StatusId == 0) ? request.StatusId : dbItem.StatusId;

            await _context.SaveChangesAsync();

            await Clients.All.SendAsync("UpdateUser", dbItem);
        }

        public async System.Threading.Tasks.Task DeleteOne(int id)
        {
            var dbItem = await _context.Events.FindAsync(id); // ADD HANDLE

            dbItem.StatusId = (int)StatusEnm.NotActive;
            await _context.SaveChangesAsync();
            await Clients.All.SendAsync("DeleteUser", dbItem);
        }

        public async System.Threading.Tasks.Task ForceDeleteOne(int id)
        {
            var user = await _context.Users.FindAsync(id); // ADD HANDLE

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            await Clients.All.SendAsync("DeleteUser", user);
        }


    }
}