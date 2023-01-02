using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MsSqlAccessor.Enums;
using MsSqlAccessor.Helpers;
using MsSqlAccessor.Models;
using MsSqlAccessor.Services;

namespace MsSqlAccessor.DbControllers
{
    public class EventsDbController
    {
        private readonly CompetitionBdTestContext _context;

        public EventsDbController(CompetitionBdTestContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<EventDTO>>> GetEvents()
        {
            //IEnumerable<Event> eventsDb = await _context.Events.IncludeVirtualProperties(new Event { }).ToListAsync();
            //IEnumerable<EventDTO> eventsDTO = eventsDb.Select(e => e.ConvertToDto<Event, EventDTO>());

            return await _context.Events.IncludeVirtualProperties(new Event { })
                .Select(e => e.ConvertToDto<Event, EventDTO>())
                .ToListAsync();
        }

        public async Task<ActionResult<EventDTO>> GetEvent(int id)
        {
            var eventDb = await _context.Events.IncludeVirtualProperties(new Event { }).FirstOrDefaultAsync(e => e.Id == id);
            if (!EventExists(id))
            {
                throw new ServerError(AppError.NoData);
            }
            else
            {
                var eventDTO = eventDb.ConvertToDto<Event, EventDTO>();
                return eventDb.ConvertToDto<Event, EventDTO>();
            }
        }

        public async Task<ActionResult<EventDTO>> PutEvent(int id, EventDTO eventDTO, int userId)
        {
            Event eventDb;

            if (id != eventDTO.Id)
            {
                throw new ServerError(AppError.BadRequest);
            }

            try
            {
                eventDb = await _context.Events.IncludeVirtualProperties(new Event { }).FirstOrDefaultAsync(e => e.Id == id);
                //eventDb = eventDTO.ConvertFromDto<Event, EventDTO>();
                eventDb = eventDb.MakeChangesFromDto<Event, EventDTO>(eventDTO);
                eventDb.UpdateDate = DateTime.Now;
                eventDb.UpdateUserId = userId;
                _context.Entry(eventDb).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(id))
                    {
                        throw new ServerError(AppError.NoData);
                    }
                    else
                    {
                        throw new ServerError(AppError.General);
                    }
                }
            }
            catch
            {
                if (!EventExists(id))
                {
                    throw new ServerError(AppError.NoData);
                }
                else
                {
                    throw new ServerError(AppError.General);
                }
            }
          
            var eventResult = await _context.Events.IncludeVirtualProperties(new Event { }).FirstOrDefaultAsync(e => e.Id == id);
            EventDTO eventDTOResult = eventResult.ConvertToDto<Event, EventDTO>();

            return eventDTOResult;
        }

        public async Task<ActionResult<EventDTO>> PostEvent(EventDTO eventDTO, int userId)
        {
            Event eventDb = @eventDTO.ConvertFromDto<Event,EventDTO>();

            eventDb.CreateDate = DateTime.Now;
            eventDb.UpdateDate = DateTime.Now;
            eventDb.CreateUserId = userId;
            eventDb.UpdateUserId = userId;
            eventDb.StatusId = (int)StatusEnm.Active;

            _context.Events.Add(eventDb);

            await _context.SaveChangesAsync();
           
            EventDTO eventDTOResult = eventDb.ConvertToDto<Event, EventDTO>();

            return eventDTOResult;
        }
        public async Task<ActionResult<bool>> DeleteEvent(int id)
        {
            var eventDb = await _context.Events.FindAsync(id);
            if (eventDb == null)
            {
                throw new ServerError(AppError.NoData);
            }
            eventDb.StatusId = (int)StatusEnm.NotActive;
            _context.Entry(eventDb).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                {
                    throw new ServerError(AppError.NoData);
                }
                else
                {
                    throw new ServerError(AppError.General);
                }
            }
        }

        public async Task<ActionResult<bool>> DeleteEventForce(int id)
        {
            var eventDb = await _context.Events.FindAsync(id);
            if (eventDb == null)
            {
                throw new ServerError(AppError.NoData);
            }

            _context.Events.Remove(eventDb);
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                {
                    throw new ServerError(AppError.NoData);
                }
                else
                {
                    throw new ServerError(AppError.General);
                }
            }
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
