using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MacCrawler.Models;
using MacCrawler.Interface;
using MacCrawler.DisplayData;
using MacCrawler.Implementation;

namespace MacCrawler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KnownReferenceNumbersController : ControllerBase
    {
        private readonly MacCrawlerContext _context;
        public IKnownReferenceNumbers knownReferenceNumbers;
        public KnownReferenceNumbersController(MacCrawlerContext context, IKnownReferenceNumbers knownReference)
        {
            knownReferenceNumbers = knownReference;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DisplayKnownReferenceNumber>>> GetKnownReferenceNumber()
        {
           
            return await knownReferenceNumbers.display();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<KnownReferenceNumber>> GetKnownReferenceNumber(int id)
        {
            var knownReferenceNumber = await _context.KnownReferenceNumber.FindAsync(id);

            if (knownReferenceNumber == null)
            {
                return NotFound();
            }

            return knownReferenceNumber;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutKnownReferenceNumber(int id, KnownReferenceNumber knownReferenceNumber)
        {
            if (id != knownReferenceNumber.Id)
            {
                return BadRequest();
            }
            if (_context.KnownReferenceNumber.Any(x => x.ReferenceNumberText.Trim() == knownReferenceNumber.ReferenceNumberText.Trim() && x.ModelId == knownReferenceNumber.ModelId && x.ManufacturerId == knownReferenceNumber.ManufacturerId))
            {
                return Conflict("Data is already exists");
            }
            _context.Entry(knownReferenceNumber).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KnownReferenceNumberExists(id))
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


        [HttpPost]
        public async Task<ActionResult<KnownReferenceNumber>> PostKnownReferenceNumber(KnownReferenceNumber knownReferenceNumber)
        {
            if (_context.KnownReferenceNumber.Any(x => x.ReferenceNumberText.Trim() == knownReferenceNumber.ReferenceNumberText.Trim() && x.ModelId == knownReferenceNumber.ModelId && x.ManufacturerId == knownReferenceNumber.ManufacturerId))
            {
                return Conflict("Data is already present");
            }     
            _context.KnownReferenceNumber.Add(knownReferenceNumber);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKnownReferenceNumber", new { id = knownReferenceNumber.Id }, knownReferenceNumber);
        }

        // DELETE: api/KnownReferenceNumbers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<KnownReferenceNumber>> DeleteKnownReferenceNumber(int id)
        {
            var knownReferenceNumber = await _context.KnownReferenceNumber.FindAsync(id);
            if (knownReferenceNumber == null)
            {
                return NotFound();
            }

            _context.KnownReferenceNumber.Remove(knownReferenceNumber);
            await _context.SaveChangesAsync();

            return knownReferenceNumber;
        }

        private bool KnownReferenceNumberExists(int id)
        {
            return _context.KnownReferenceNumber.Any(e => e.Id == id);
        }
    }
}
