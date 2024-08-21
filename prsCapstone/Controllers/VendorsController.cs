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
    public class VendorsController : ControllerBase
    {
        private readonly prsCapstoneContext _context;

        public VendorsController(prsCapstoneContext context)
        {
            _context = context;
        }

        // GET: api/Vendors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vendor>>> GetVendor()
        {
            return await _context.Vendors.ToListAsync();
        }
        //PO METHOD
        [HttpGet("po/{vendorId}")]
        public async Task<ActionResult<Po>> CreatePo(int vendorId)
        {
            var vendor = await _context.Vendors.FindAsync(vendorId);

            var values = await (from p in _context.RequestLines
                               where p.Requests!.Status == "APPROVED"
                               && p.Product!.VendorId == vendorId
                               group p by p.Product!.Name into g
                               select new
                               {

                                   Product = g.Key,
                                   TotalQuantity = g.Sum(p => p.Quantity),
                                   //TotalPrice = g.Sum(p => p.Quantity * p.Products.Price),
                                   // Assuming you want the total value (LineTotal) as well
                                   LineTotal = g.Sum(p => p.Quantity * p.Product.Price)


                               }).ToListAsync();

            /*var sortedLines = new SortedList<int, Poline>();
            //foreach (var line in lines)
            //{
            //    if (!sortedLines.ContainsKey(line))
            //    {
            //        var poline = new Poline()
            //        {
            //            Product = values,
            //            Quantity = 0,
            //            Price = l.Price,
            //            LineTotal = l.LineTotal
            //        };
            //        sortedLines.Add(line.Id, poline);
            //    }
            //    sortedLines[l.Id].Quantity += l.Quantity;
            //}
            */
            List<Poline> polines = new List<Poline>();

            foreach (var p in values)
            {
                int i = 0;
                polines.Add( new Poline { Product = p.Product, Quantity = p.TotalQuantity, LineTotal = p.LineTotal });
                i++;
            }
            var pototal = (from p in polines select p.LineTotal).Sum();
            var po = new Po(vendor, polines, pototal);
            ///var test1 = new Poline({ Product = product, });
            
            
            return po;
        }
        // GET: api/Vendors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vendor>> GetVendor(int id)
        {
            var vendor = await _context.Vendors.FindAsync(id);

            if (vendor == null)
            {
                return NotFound();
            }

            return vendor;
        }

        // PUT: api/Vendors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVendor(int id, Vendor vendor)
        {
            if (id != vendor.Id)
            {
                return BadRequest();
            }

            _context.Entry(vendor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendorExists(id))
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

        // POST: api/Vendors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Vendor>> PostVendor(Vendor vendor)
        {
            _context.Vendors.Add(vendor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVendor", new { id = vendor.Id }, vendor);
        }

        // DELETE: api/Vendors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVendor(int id)
        {
            var vendor = await _context.Vendors.FindAsync(id);
            if (vendor == null)
            {
                return NotFound();
            }

            _context.Vendors.Remove(vendor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VendorExists(int id)
        {
            return _context.Vendors.Any(e => e.Id == id);
        }
    }
}
