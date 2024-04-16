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

namespace MacCrawler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KnownManufacturersController : ControllerBase
    {
        private readonly MacCrawlerContext _context;
        public IKnownManufactures _knownManufactures; 

        public KnownManufacturersController(MacCrawlerContext context, IKnownManufactures knownManufactures)
        {
            _context = context;
            _knownManufactures = knownManufactures;
        }

        [HttpGet]
        public async Task<ActionResult<List<DisplayKnownManufactures>>> GetKnownManufacturer()
        {
            return await _knownManufactures.display();
            /*return await _context.KnownManufacturer.ToListAsync();*/
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<KnownManufacturer>> GetKnownManufacturer(int id)
        {
            var knownManufacturer = await _context.KnownManufacturer.FindAsync(id);

            if (knownManufacturer == null)
            {
                return NotFound();
            }

            return knownManufacturer;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutKnownManufacturer(int id, KnownManufacturer knownManufacturer)
        {
            if (id != knownManufacturer.Id)
            {
                return BadRequest();
            }
            if (_context.KnownManufacturer.Any(km => km.Id != id && km.ManufacturerText == knownManufacturer.ManufacturerText))
            {
                return Conflict("A record with the same knownManufacturerText already exists.");
            }

            _context.Entry(knownManufacturer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KnownManufacturerExists(id))
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
        public async Task<ActionResult<KnownManufacturer>> PostKnownManufacturer(KnownManufacturer knownManufacturer)
        {
            if (!_context.KnownManufacturer.Any(km => km.ManufacturerText == knownManufacturer.ManufacturerText))
            {              
                _context.KnownManufacturer.Add(knownManufacturer);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetKnownManufacturer", new { id = knownManufacturer.Id }, knownManufacturer);
            }
            else
            {           
                return Conflict("A record with the same knownManufacturerText already exists.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<KnownManufacturer>> DeleteKnownManufacturer(int id)
        {
            var knownManufacturer = await _context.KnownManufacturer.FindAsync(id);
            if (knownManufacturer == null)
            {
                return NotFound();
            }

            _context.KnownManufacturer.Remove(knownManufacturer);
            await _context.SaveChangesAsync();

            return knownManufacturer;
        }

        private bool KnownManufacturerExists(int id)
        {
            return _context.KnownManufacturer.Any(e => e.Id == id);
        }
    }
}
