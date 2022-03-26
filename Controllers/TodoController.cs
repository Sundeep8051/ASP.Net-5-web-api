using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleWebapi.Data;
using SampleWebapi.Models;

namespace SampleWebapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public TodoController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetItems(){
            var items = await _context.Items.ToListAsync();
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem(ItemData data){
            if(ModelState.IsValid){
                await _context.Items.AddAsync(data);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetItemById", new {data.Id}, data);
            }

            return new JsonResult("Something went wrong") 
            {StatusCode = 500};
        }

        [HttpGet("{id}")]
        
        public async Task<IActionResult> GetItemById(int id){
            var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == id);
            
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItemById(int id, ItemData data){
            if(id != data.Id)
            return BadRequest();
            
            var item = await _context.Items
                        .FirstOrDefaultAsync(i => i.Id == id);

            if(item == null)
            return NotFound();

            if(ModelState.IsValid){
                item.Title = data.Title;
                item.Description = data.Description;
                item.Done = data.Done;

                await _context.SaveChangesAsync();

                return NoContent();
            }

            return new JsonResult("Something went wrong") 
            {StatusCode = 500};
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id){
            var item = await _context.Items
                        .FirstOrDefaultAsync(i => i.Id == id);

            if(item == null) NotFound();

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return Ok(item);
        }
    }
}