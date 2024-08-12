using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Project2_TechTrendsAPI_38342626.Authentication;
using Project2_TechTrendsAPI_38342626.Models;

namespace Project2_TechTrendsAPI_38342626.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JobTelemetriesController : ControllerBase
    {
        private readonly NWUTechTrendsContext _context;
        //private readonly BasicAuthenticationHandler _authenticationHandler;

        public JobTelemetriesController(NWUTechTrendsContext context)
        {
            _context = context;
        }

        // Private method to check if a Telementry Entry exist
        private async Task<bool> JobTelemetryExist(int id)
        {
            return await _context.JobTelemetries.AnyAsync(t => t.Id == id);
        }

        // GET: api/JobTelemetries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobTelemetry>>> GetJobTelemetries()
        {
            return await _context.JobTelemetries.ToListAsync();
        }

        // GET: api/JobTelemetries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JobTelemetry>> GetJobTelemetry(int id)
        {
            var jobTelemetry = await _context.JobTelemetries.FindAsync(id);

            if (jobTelemetry == null)
            {
                return NotFound();
            }

            return jobTelemetry;
        }

        // PUT: api/JobTelemetries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJobTelemetry(int id, JobTelemetry jobTelemetry)
        {
            if(!await JobTelemetryExist(id))
            {
                return NotFound(new { message = $"The Job Telemetry with ID {id} not found." });
            }

            if (id != jobTelemetry.Id)
            {
                return BadRequest();
            }

            _context.Entry(jobTelemetry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobTelemetryExists(id))
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

        // POST: api/JobTelemetries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<JobTelemetry>> PostJobTelemetry(JobTelemetry jobTelemetry)
        {
            _context.JobTelemetries.Add(jobTelemetry);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJobTelemetry", new { id = jobTelemetry.Id }, jobTelemetry);
        }

        // DELETE: api/JobTelemetries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobTelemetry(int id)
        {
            if (!await JobTelemetryExist(id))
            {
                return NotFound(new { message = $"The Job Telemetry with ID {id} not found." });
            }

            var jobTelemetry = await _context.JobTelemetries.FindAsync(id);
            if (jobTelemetry == null)
            {
                return NotFound();
            }

            _context.JobTelemetries.Remove(jobTelemetry);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("GetSavings")]
        public async Task<IActionResult> GetSavings(Guid projectId, DateTime startDate, DateTime endDate)
        {
            // Fetch project data based on project ID given by parameter
            var project = await _context.Projects.FindAsync(projectId);

            //Input Validation
            if (project == null)
            {
                return NotFound(new { message = $"The Project with ID {projectId} not found." });
            }

            if (project.ProjectCreationDate <= startDate || project.ProjectCreationDate >= endDate)
            {
                return BadRequest();
            }

            // Fetch process data based on project ID -> Process (PK), Project (FK)
            var process = await _context.Processes.FirstOrDefaultAsync(p => p.ProjectId == project.ProjectId);
            if (process == null)
            {
                return BadRequest();
            }

            // Fetch telemetry data based on process ID -> Telemetry (FK), Process (PK)
            var telemetry = await _context.Processes.FirstOrDefaultAsync(p => p.ProcessId == process.ProcessId);
            if (telemetry == null)
            {
                return BadRequest();
            }

            // Calculate cumulative time and cost saved
            int days = endDate.DayOfYear - project.ProjectCreationDate.Value.DayOfYear;

            var totalTime = days * 24;
            var totalCostSaved = totalTime * 0.2;

            // Create a response object inline
            var response = new
            {
                TotalTimeSaved = totalTime,
                TotalCostSaved = totalCostSaved
            };

            // Return the result as JSON
            return Ok(response);
        }

        /*[AllowAnonymous]
        [HttpPost("Authirize")]
        public IActionResult AuthUser([FromBody] User user)
        {
            var token = AuthenticationHandler.Authenticate(user.username, user.password);
            if (token == null) return Unauthorized();

            return Ok(token);
        }*/

        private bool JobTelemetryExists(int id)
        {
            return _context.JobTelemetries.Any(e => e.Id == id);
        }
    }

    public class User
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
