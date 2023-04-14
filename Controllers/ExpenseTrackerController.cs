using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotnetapp.Models; 

namespace dotnetapp.Controllers 
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseTrackerController : ControllerBase
    {
        private readonly ExpenseTrackerApiDbContext _context;

        public ExpenseTrackerController(ExpenseTrackerApiDbContext context)
        {
            _context = context;
        }

        // GET: api/ExpenseTrackerApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseTrackerApi>>> GetExpenseTrackerApis()
        {
            return await _context.ExpenseTrackerApis.ToListAsync();
        }

        // GET: api/ExpenseTrackerApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseTrackerApi>> GetExpenseTrackerApi(int id)
        {
            var ExpenseTrackerApi = await _context.ExpenseTrackerApis.FindAsync(id);

            if (ExpenseTrackerApi == null)
            {
                return NotFound();
            }

            return ExpenseTrackerApi;
        }

        // POST: api/ExpenseTrackerApi
        [HttpPost]
        public async Task<ActionResult<ExpenseTrackerApi>> PostExpenseTrackerApi(ExpenseTrackerApi ExpenseTrackerApi)
        {
            _context.ExpenseTrackerApis.Add(ExpenseTrackerApi);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetExpenseTrackerApi), new { id = ExpenseTrackerApi.Id }, ExpenseTrackerApi);
        }

        // PUT: api/ExpenseTrackerApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpenseTrackerApi(int id, ExpenseTrackerApi ExpenseTrackerApi)
        {
            if (id != ExpenseTrackerApi.Id)
            {
                return BadRequest();
            }

            _context.Entry(ExpenseTrackerApi).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpenseTrackerApiExists(id))
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

        // DELETE: api/ExpenseTrackerApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpenseTrackerApi(int id)
        {
            var ExpenseTrackerApi = await _context.ExpenseTrackerApis.FindAsync(id);
            if (ExpenseTrackerApi == null)
            {
                return NotFound();
            }

            _context.ExpenseTrackerApis.Remove(ExpenseTrackerApi);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExpenseTrackerApiExists(int id)
        {
            return _context.ExpenseTrackerApis.Any(e => e.Id == id);
        }
    }
}
