using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MsSqlAccessor.Helpers;
using MsSqlAccessor.Models;
using MsSqlAccessor.Services;
using NuGet.Configuration;

namespace MsSqlAccessor.Hubs
{
    public class EventParticipantTeamHub : Hub
    {
        private readonly CompetitionBdTestContext _context;

        public EventParticipantTeamHub(CompetitionBdTestContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task GetEventParticipantTeams()
        {
            var dbItems = await _context.EventParticipantTeams
                .IncludeVirtualProperties(new EventParticipantTeam { })
                .Select(e => e.ConvertToDto<EventParticipantTeam, EventParticipantTeamDTO>())
                .ToListAsync();

            await Clients.All.SendAsync("ReceiveGetEventParticipantTeams", dbItems);
        }

        public async System.Threading.Tasks.Task GetEventParticipantTeam(int id)
        {
            var eventParticipantTeam = await _context.EventParticipantTeams
                .IncludeVirtualProperties(new EventParticipantTeam { })
                .FirstOrDefaultAsync(e => e.Id == id);
            
            if (eventParticipantTeam == null)
            {
                await Clients.All.SendAsync("ReceiveGetEventParticipantTeam", new EventParticipantTeamDTO());
                return;
            }
            var eventParticipantTeamDb = eventParticipantTeam.ConvertToDto<EventParticipantTeam, EventParticipantTeamDTO>();

            await Clients.All.SendAsync("ReceiveGetEventParticipantTeam", eventParticipantTeamDb);
        }

        public async System.Threading.Tasks.Task PutEventParticipantTeam(int id, EventParticipantTeamDTO eventParticipantTeam)
        {
            EventParticipantTeam eventParticipantTeamDb;
            if (id != eventParticipantTeam.ParticipantId)
            {
                await Clients.All.SendAsync("ReceivePutParticipantTeam", "Bad Request");
                return;
            }

            eventParticipantTeamDb = await _context.EventParticipantTeams
                .IncludeVirtualProperties(new EventParticipantTeam { })
                .FirstOrDefaultAsync(e => e.Id == id);

            //eventDb = eventDTO.ConvertFromDto<Event, EventDTO>();
            eventParticipantTeamDb = eventParticipantTeamDb
                .MakeChangesFromDto<EventParticipantTeam, EventParticipantTeamDTO>(eventParticipantTeam);

            _context.Entry(eventParticipantTeam).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventParticipantTeamExists(id))
                {
                    await Clients.All.SendAsync("ReceivePutParticipantTeam", new EventParticipantTeamDTO());
                    return;
                }
                else
                {
                    throw;
                }
            }

            var eventParticipantTeamResult = await _context.EventParticipantTeams.IncludeVirtualProperties(new EventParticipantTeam { }).FirstOrDefaultAsync(e => e.Id == id);
            EventParticipantTeamDTO eventParticipantTeamDTOResult = eventParticipantTeamResult.ConvertToDto<EventParticipantTeam, EventParticipantTeamDTO>();
            await Clients.All.SendAsync("ReceivePutParticipantTeam", eventParticipantTeamDTOResult);
        }

        public async System.Threading.Tasks.Task PostEventParticipantTeam(EventParticipantTeamDTO eventParticipantTeam)
        {
            EventParticipantTeam eventParticipantTeamDb = eventParticipantTeam.ConvertFromDto<EventParticipantTeam, EventParticipantTeamDTO>();

            _context.EventParticipantTeams.Add(eventParticipantTeamDb);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EventParticipantTeamExists(eventParticipantTeam.ParticipantId))
                {
                    await Clients.All.SendAsync("ReceivePostEventParticipantTeam", "Conflict");
                    return;
                }
                else
                {
                    throw;
                }
            }

            EventParticipantTeamDTO eventParticipantTeamResult = eventParticipantTeamDb.ConvertToDto<EventParticipantTeam, EventParticipantTeamDTO>();

            await Clients.All.SendAsync("ReceivePostEventParticipantTeam", eventParticipantTeamResult);
        }

        public async System.Threading.Tasks.Task DeleteEventParticipantTeam(int id)
        {
            var eventParticipantTeam = await _context.EventParticipantTeams.FindAsync(id);
            if (eventParticipantTeam == null)
            {
                await Clients.All.SendAsync("ReceiveDeleteEventParticipantTeam", "not found");
                return;
            }

            _context.EventParticipantTeams.Remove(eventParticipantTeam);
            await _context.SaveChangesAsync();

            await Clients.All.SendAsync("ReceiveDeleteEventParticipantTeam", new EventParticipantTeamDTO());
            return;
        }

        private bool EventParticipantTeamExists(int id)
        {
            return _context.EventParticipantTeams.Any(e => e.ParticipantId == id);
        }
    }
}
