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
    public class RequestLinesController : ControllerBase
    {
        private readonly prsCapstoneContext _context;

        public RequestLinesController(prsCapstoneContext context)
        {
            _context = context;
        }

        // GET: api/RequestLines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestLine>>> GetRequestLine()
        {
            return await _context.RequestLines.Include(x=> x.Product).ToListAsync();
        }

        // GET: api/RequestLines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RequestLine>> GetRequestLine(int id)
        {
            

            var requestLine = await _context.RequestLines.Include(x=> x.Product).SingleOrDefaultAsync(x => x.Id == id);

            if (requestLine == null)
            {
                return NotFound();
            }
            

            return requestLine;
        }
        //[HttpPut("{requestId}")]
        private async Task<IActionResult> RecalculateRequestTotal(int id)
        {
            var request = await _context.Requests.FindAsync(id);

            if (request == null)
            {
                return NotFound();
            }


            request.Total = (from rl in _context.RequestLines

                           join p in _context.Products
                              on rl.ProductId equals p.Id
                           where rl.RequestId == id
                           select  new
                           {
                            LineTotal = p.Price* rl.Quantity
                           }).Sum(x => x.LineTotal);
           
            await _context.SaveChangesAsync();

            return Ok();
        }

        // PUT: api/RequestLines/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequestLine(int id, RequestLine requestLine)
        {
            if (id != requestLine.Id)
            {
                return BadRequest();
            }

            _context.Entry(requestLine).State = EntityState.Modified;

            try
            {

                await _context.SaveChangesAsync();
                await RecalculateRequestTotal(requestLine.RequestId);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestLineExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
           await RecalculateRequestTotal(id);
            return NoContent();
        }

        // POST: api/RequestLines
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RequestLine>> PostRequestLine(RequestLine requestLine)
        {
            _context.RequestLines.Add(requestLine);
            await _context.SaveChangesAsync();
            await RecalculateRequestTotal(requestLine.RequestId);

            return CreatedAtAction("GetRequestLine", new { id = requestLine.Id }, requestLine);
        }

        // DELETE: api/RequestLines/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequestLine(int id)
        {
            var requestLine = await _context.RequestLines.FindAsync(id);
            if (requestLine == null)
            {
                return NotFound();
            }
            var requestId = requestLine.RequestId;
            _context.RequestLines.Remove(requestLine);
            await _context.SaveChangesAsync();
            await RecalculateRequestTotal(requestId);

            return NoContent();
        }

        private bool RequestLineExists(int id)
        {
            return _context.RequestLines.Any(e => e.Id == id);
        }
    }
}
