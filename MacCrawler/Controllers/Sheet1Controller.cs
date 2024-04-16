using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MacCrawler.Models;
using MacCrawler.DisplayData;
using MacCrawler.Implementation;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using MacCrawler.Interface;

namespace MacCrawler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Sheet1Controller : ControllerBase
    {

        private readonly MacCrawlerContext _context;
        private readonly IConfiguration _configuration;
        public ISheet1 _Sheet1;
        public Sheet1Controller(MacCrawlerContext context, ISheet1 sheet, IConfiguration configuration)
        {
            _context = context;
            _Sheet1 = sheet;
            _configuration = configuration;
        }
        private SqlConnection GetSqlConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
        //// GET: api/Sheet1
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Sheet1>>> GetSheet1()
        //{
        //    return await _context.Sheet1.ToListAsync();
        //}
        //// GET: api/Sheet1/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Sheet1>> GetSheet1(int id)
        //{

        //    var sheet1 = await _context.Sheet1.FindAsync(id);

        //    if (sheet1 == null)
        //    {
        //        return NotFound();
        //    }

        //    return sheet1;
        //}

        //// PUT: api/Sheet1/5      
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutSheet1(int id, Sheet1 sheet1)
        //{
        //    if (id != sheet1.Pid)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(sheet1).State = EntityState.Modified;
        //    if (_context.Sheet1.Any(x => x.Name == sheet1.Name&&x.Id==sheet1.Id))
        //    {
        //        return Conflict("A record with the same knownManufacturerText already exists.");
        //    }
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!Sheet1Exists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Sheet1       
        //[HttpPost]
        //public async Task<ActionResult<Sheet1>> PostSheet1(Sheet1 sheet1)
        //{
        //    if (!_context.Sheet1.Any(x => x.Name == sheet1.Name))
        //    {
        //        _context.Sheet1.Add(sheet1);
        //        await _context.SaveChangesAsync();
        //        return CreatedAtAction("GetSheet1", new { id = sheet1.Pid }, sheet1);
        //    }
        //    else
        //    {
        //        return Conflict("A record with the same knownManufacturerText already exists.");
        //    }


        //}

        //// DELETE: api/Sheet1/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Sheet1>> DeleteSheet1(int id)
        //{
        //    var sheet1 = await _context.Sheet1.FindAsync(id);
        //    if (sheet1 == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Sheet1.Remove(sheet1);
        //    await _context.SaveChangesAsync();

        //    return sheet1;
        //}

        [HttpGet]
        public async Task<ActionResult<List<Sheet1>>> GetSheet()
        {
            var sheet = await _Sheet1.GetSheet();
            return sheet;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<List<Sheet1>>> GetSheet(int id)
        {
            var sheet = await _Sheet1.GetSheet(id);
            return sheet;
        }
        [HttpPost]
        public async Task<ActionResult<string>> PostSheet(Sheet1 sheet)
        {
            try
            {
                await _Sheet1.PostSheet(sheet);
                return Ok("Successfully Data Added");
            }
            catch (Exception ex)
            {          
                return BadRequest($"Error: {ex.Message}");
            }
        }
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutSheet(int id,Sheet1 sheet)
        //{
        //    if (id != sheet.Pid)
        //    {
        //        return BadRequest();
        //    }
        //    try
        //        {

        //    }
        //    catch
        //    {

        //    }
        //}

        [HttpPost("Incomingdata")]
        public async Task<ActionResult<List<ManufactureName>>> Incomingdata(List<string> incomingdata)
        {
            Fuzzy fuzzy = new Fuzzy();
            List<Sheet1> sheets = await _context.Sheet1.ToListAsync();
            List<ManufactureName> manufactureName = fuzzy.Comparedata(incomingdata, sheets);
            return manufactureName;
        }
        [HttpGet("GetData")]
        public async Task<ActionResult<DataTableResponse<Sheet1>>> Getdata([FromQuery] int pageNumber = 0, [FromQuery] int size = 10)
        {
            var list = _Sheet1.GetSheetData(pageNumber, size);
            var totalRecords = _context.Sheet1.Count();

            var response = new DataTableResponse<Sheet1>
            {
                Data = await list,
                RecordsTotal = totalRecords,
                RecordsFiltered = totalRecords
            };

            return response;

        }
        [HttpPost("GetData1")]
        public async Task<ActionResult<DataTableResponse<Sheet1>>> Getdata1([FromBody] DataTableRequest dataTableRequest)
        {
            var list = _Sheet1.GetSheetData(dataTableRequest.pageNumber - 1, dataTableRequest.size);
            var totalRecords = _context.Sheet1.Count();

            var response = new DataTableResponse<Sheet1>
            {
                Data = await list,
                RecordsTotal = totalRecords,
                RecordsFiltered = totalRecords
            };

            return response;

        }
        private bool Sheet1Exists(int id)
        {
            return _context.Sheet1.Any(e => e.Pid == id);
        }
    }
}
