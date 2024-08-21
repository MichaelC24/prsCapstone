using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prsCapstone.Data;
using prsCapstone.Model;

namespace prsCapstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly prsCapstoneContext _context;

        public RequestsController(prsCapstoneContext context)
        {
            _context = context;
        }
        [HttpGet("reviews/{id}")]
        public async Task<ActionResult<IEnumerable<Request>>> GetReviews(int id)
        {
            return await (from r in _context.Requests
                         where r.Id != id
                         select r).ToListAsync();
        }
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequestByStatus(string status)
        {
            return await _context.Requests
                                    .Include(x => x.User)
                                    .Where(x => x.Status == status).ToListAsync();
        }

        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequest()
        {
            return await _context.Requests.Include(u => u.User).ToListAsync();
        }

        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            var request = await _context.Requests
                             .Include(x => x.User)
                             .Include(r => r.RequestLines)!
                             .ThenInclude(p => p.Product)
                             .SingleOrDefaultAsync(a => a.Id == id);
            if (request == null)
            {
                return NotFound();
            }

            return Ok(request);
        }
        

        [HttpPut("review/{id}")] //SETS STATUS TO REVIEW
        public async Task<ActionResult<Request>> Review(int id, Request request)
        {
            

            if (request == null)
            {
                return NotFound();
            }
            else if (request.Total <= 50)
            {
                    request.Status = "APPROVED";
            }else
                {
                    request.Status = "REVIEW";
                }
            _context.Entry(request).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("approve/{id}")]//SETS STATUS TO APPROVE
        public async Task<ActionResult<Request>> Approve( int id,Request request)
        {
            

            if (request == null)
            {
                return NotFound();
            }
            request.Status = "APPROVED";
            _context.Entry(request).State = EntityState.Modified;
            //_context.Add(request);
            await _context.SaveChangesAsync();
            return request;
        }
        [HttpPut("reject/{id}")] //SETS STATUS TO REJECT
        public async Task<ActionResult<Request>> Reject(int id, Request request)
        {
            var re = await _context.Requests.FindAsync(id);
            if (re == null)
            {
                return NotFound();
            }
            re.Status = "REJECTED";
            re.RejectionReason = request.RejectionReason;
            _context.Entry(re).State = EntityState.Modified;
            //_context.Add(request);
            _context.SaveChanges();
            return re;
        }
        // PUT: api/Requests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, Request request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
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

        // POST: api/Requests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest(Request request)
        {
            request.DeliveryMode = "Pickup";
            request.Status = "NEW";
            request.Total = 0;
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = request.Id }, request);
        }

        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequestExists(int id)
        {
            return _context.Requests.Any(e => e.Id == id);
        }
    }
}




