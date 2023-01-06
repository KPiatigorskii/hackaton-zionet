using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MsSqlAccessor.Models;
using MsSqlAccessor.DbControllers;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MsSqlAccessor.Services;
using MsSqlAccessor.Enums;

namespace MsSqlAccessor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly EventsDbController _dbController;

        public EventsController(EventsDbController dbController)
        {
            _dbController = dbController;

        }

        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDTO>>> GetEvents()
        {
            var events = await _dbController.GetEvents();

            return events;
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventDTO>> GetEvent(int id)
        {
            try
            {
                return await _dbController.GetEvent(id);
            }
            catch (ServerError ex)
            {
                switch (ex.Error)
                {
                    case AppError.NoData:
                        return NotFound();

                    default:
                        return BadRequest();
                }
            }
        }

        // PUT: api/Events/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<EventDTO>> PutEvent(int id, EventDTO @event)
        {
            var userId = 1;

            try 
            {
                return await _dbController.PutEvent(id, @event, userId);
            }
            catch(ServerError ex) 
            { 
                switch (ex.Error)
                {
                    case AppError.BadRequest:
                        return BadRequest();

                    case AppError.NoData: 
                        return NotFound();

                    default:
                        return BadRequest();
                }
            }
        }

        // POST: api/Events
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EventDTO>> PostEvent(EventDTO @event)
        {
            int userId = 1;
            try
            {
                return await _dbController.PostEvent(@event, userId);
            }
            catch (ServerError ex)
            {
                return BadRequest();
            }
            
        }

        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteEvent(int id)
        {
            try
            {
                return await _dbController.DeleteEvent(id);
            }
            catch (ServerError ex)
            {
                switch (ex.Error)
                {
                    case AppError.NoData:
                        return NotFound();

                    default:
                        return BadRequest();
                }
            }
        }

        // DELETE: api/Events/Force/5
        [HttpDelete("force/{id}")]
        public async Task<ActionResult<bool>> DeleteEventForce(int id)
        {
            try
            {
                return await _dbController.DeleteEventForce(id);
            }
            catch (ServerError ex)
            {
                switch (ex.Error)
                {
                    case AppError.NoData:
                        return NotFound();

                    default:
                        return BadRequest();
                }
            }

        }
    }
}
