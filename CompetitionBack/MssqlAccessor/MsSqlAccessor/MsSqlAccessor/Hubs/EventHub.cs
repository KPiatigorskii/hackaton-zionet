using Azure.Core;
//using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MsSqlAccessor.Models;
using MsSqlAccessor.Enums;

namespace MsSqlAccessor.Hubs
{
    public class EventHub : Hub
    {
        private readonly CompetitionBdTestContext _context;
        public EventHub(CompetitionBdTestContext context)
        {
            _context = context;

        }

        public async System.Threading.Tasks.Task GetAll()
        {
            var dbItems = await _context.Events.ToArrayAsync();

            await Clients.All.SendAsync("ReceiveEvents", dbItems);
        }

        public async System.Threading.Tasks.Task GetOne(int id)
        {
            var dbItem = await _context.Events.Where(e => e.Id == id).FirstOrDefaultAsync();

            await Clients.All.SendAsync("ReceiveEvent", dbItem);
        }

        public async System.Threading.Tasks.Task UpdateOne(int id, Event request)
        {
            var dbItem = await _context.Events.FindAsync(id); // ADD HANDLE

            dbItem.Title = !string.IsNullOrWhiteSpace(request.Title) ? request.Title : dbItem.Title;
            dbItem.Address = request.Address;
            dbItem.DateTime = request.DateTime;
            dbItem.NumberParticipantsInTeam = !(request.NumberParticipantsInTeam == 0) ? request.NumberParticipantsInTeam : dbItem.NumberParticipantsInTeam;
            dbItem.NumberConcurrentTasks = !(request.NumberConcurrentTasks == 0) ? request.NumberConcurrentTasks : dbItem.NumberConcurrentTasks;
            dbItem.Hashcode = request.Hashcode;
            dbItem.UpdateDate = DateTime.Now;
            dbItem.UpdateUserId = request.UpdateUserId;
            dbItem.StatusId = !(request.StatusId == 0) ? request.StatusId : dbItem.StatusId;

            await _context.SaveChangesAsync();

            await Clients.All.SendAsync("UpdateEvent", dbItem);
        }

        public async System.Threading.Tasks.Task DeleteOne(int id)
        {
            var dbItem = await _context.Events.FindAsync(id); // ADD HANDLE

            dbItem.StatusId = (int)StatusEnm.NotActive;
            await _context.SaveChangesAsync();
            await Clients.All.SendAsync("DeleteEvent", dbItem);
        }

        public async System.Threading.Tasks.Task ForceDeleteOne(int id)
        {
            var dbItem = await _context.Events.FindAsync(id); // ADD HANDLE

            _context.Events.Remove(dbItem);
            await _context.SaveChangesAsync();
            await Clients.All.SendAsync("ForceDeleteEvent", dbItem);
        }


    }
}