using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MsSqlAccessor.Interfaces;
using MsSqlAccessor.Models;
using Newtonsoft.Json;

namespace MsSqlAccessor.WebAPIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventParticipantTeamsHub : ControllerBase
    {
        private readonly CompetitionBdTestContext _context;
		private readonly ILoggerManager _logger;

		public EventParticipantTeamsHub(CompetitionBdTestContext context, ILoggerManager logger)
        {
            _context = context;
			_logger = logger;
		}

        // GET: api/EventParticipantTeams
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventParticipantTeam>>> GetEventParticipantTeams()
        {
			_logger.LogInfo("Here is info message from the controller.");
			_logger.LogDebug("Here is debug message from the controller.");
			_logger.LogWarn("Here is warn message from the controller.");
			_logger.LogError("Here is error message from the controller.");

			return await _context.EventParticipantTeams.ToListAsync();
        }

        // GET: api/EventParticipantTeams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventParticipantTeam>> GetEventParticipantTeam(int id)
        {
            var eventParticipantTeam = await _context.EventParticipantTeams.FindAsync(id);

            if (eventParticipantTeam == null)
            {
                return NotFound();
            }

			_logger.LogInfo(JsonConvert.SerializeObject(eventParticipantTeam));

			return eventParticipantTeam;
        }

        // PUT: api/EventParticipantTeams/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEventParticipantTeam(int id, EventParticipantTeam eventParticipantTeam)
        {
            if (id != eventParticipantTeam.ParticipantId)
            {
                return BadRequest();
            }

            _context.Entry(eventParticipantTeam).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventParticipantTeamExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/EventParticipantTeams
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EventParticipantTeam>> PostEventParticipantTeam(EventParticipantTeam eventParticipantTeam)
        {
            _context.EventParticipantTeams.Add(eventParticipantTeam);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EventParticipantTeamExists(eventParticipantTeam.ParticipantId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEventParticipantTeam", new { id = eventParticipantTeam.ParticipantId }, eventParticipantTeam);
        }

        // DELETE: api/EventParticipantTeams/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEventParticipantTeam(int id)
        {
            var eventParticipantTeam = await _context.EventParticipantTeams.FindAsync(id);
            if (eventParticipantTeam == null)
            {
                return NotFound();
            }

            _context.EventParticipantTeams.Remove(eventParticipantTeam);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventParticipantTeamExists(int id)
        {
            return _context.EventParticipantTeams.Any(e => e.ParticipantId == id);
        }
    }
}
