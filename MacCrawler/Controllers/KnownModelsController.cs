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
    public class KnownModelsController : ControllerBase
    {
        private readonly MacCrawlerContext _context;
        public IKnownModel _knownModel;
        public KnownModelsController(MacCrawlerContext context, IKnownModel knownModel)
        {
            _context = context;
            _knownModel = knownModel;
        }

       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DisplayKnownModel>>> GetKnownModel()
        {
            return await _knownModel.displays();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<KnownModel>> GetKnownModel(int id)
        {
            var knownModel = await _context.KnownModel.FindAsync(id);

            if (knownModel == null)
            {
                return NotFound();
            }

            return knownModel;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutKnownModel(int id, KnownModel knownModel)
        {
            if (id != knownModel.Id)
            {
                return BadRequest();
            }
            if (_context.KnownModel.Any(m => m.Id != id && m.ModelText == knownModel.ModelText && m.ManufacturerId == knownModel.ManufacturerId))
            {
                return Conflict("A record with the same model text and manufacturer id already exists.");
            }
            _context.Entry(knownModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KnownModelExists(id))
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
        public async Task<ActionResult<KnownModel>> PostKnownModel(KnownModel knownModel)
        {
            if (_context.KnownModel.Any(m => m.ModelText == knownModel.ModelText && m.ManufacturerId == knownModel.ManufacturerId))
            {
                return Conflict("A record with the same model text and manufacturer id already exists.");
            }
            _context.KnownModel.Add(knownModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKnownModel", new { id = knownModel.Id }, knownModel);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<KnownModel>> DeleteKnownModel(int id)
        {
            var knownModel = await _context.KnownModel.FindAsync(id);
            if (knownModel == null)
            {
                return NotFound();
            }

            _context.KnownModel.Remove(knownModel);
            await _context.SaveChangesAsync();

            return knownModel;
        }

        private bool KnownModelExists(int id)
        {
            return _context.KnownModel.Any(e => e.Id == id);
        }
    }
}
