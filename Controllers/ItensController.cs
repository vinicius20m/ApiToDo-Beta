#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiToDo.Areas.Identity.Data;
using ApiToDo.Models;

namespace ApiToDo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItensController : ControllerBase
    {
        private readonly Context _context;

        public ItensController(Context context)
        {
            _context = context;
        }

        // GET: api/Itens
        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        // {
        //     return await _context.Items.ToListAsync();
        // }


        // GET: api/Itens/5
        [HttpGet("{id}/{token}")]
        public async Task<ActionResult<Item>> GetItem(int id, string token)
        {

            User user = await _context.Users.FirstOrDefaultAsync(m => m.ApiToken == token) ;
            
            if(user != null){
                
                var item = _context.Items.Where(m => m.QuadroId == id);

                if (item == null)
                {
                    return NotFound();
                }

                return Ok(item);
            }else return NotFound() ;
        }


        // PUT: api/Itens/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}/{token}")]
        public async Task<IActionResult> PutItem(int id, string token, Item item)
        {

            User user = await _context.Users.FirstOrDefaultAsync(m => m.ApiToken == token) ;
            if(user == null)
                return NotFound() 
            ;

            Quadro_Permission permission = _context.Quadro_Permissions.Include(a => 
                a.PermissionType
            ).Single(m => 
                m.UserId == user.Id && 
                m.QuadroId == item.QuadroId
            ) ;
            
            if(permission == null)
                return NotFound()
            ;
            if(!permission.PermissionType.EditQuadro)
                return NotFound() 
            ;
                
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
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

        // POST: api/Itens
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{token}")]
        public async Task<ActionResult<Item>> PostItem(string token, Item item)
        {

            User user = await _context.Users.FirstOrDefaultAsync(m => m.ApiToken == token) ;
            if(user == null)
                return NotFound() 
            ;

            Quadro_Permission permission = _context.Quadro_Permissions.Include(a => 
                a.PermissionType
            ).Single(m => 
                m.UserId == user.Id &&
                m.QuadroId == item.QuadroId
            ) ;
            
            if(permission == null)
                return NotFound()
            ;
            if(!permission.PermissionType.EditQuadro)
                return NotFound() 
            ;
            
            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItem", new { id = item.Id, token = token }, item);
        }

        // DELETE: api/Itens/5
        [HttpDelete("{id}/{token}")]
        public async Task<IActionResult> DeleteItem(int id, string token)
        {

            User user = await _context.Users.FirstOrDefaultAsync(m => m.ApiToken == token) ;
            
            if(user == null)
                return NotFound() 
            ;
                
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            Quadro_Permission permission = _context.Quadro_Permissions.Include(a => 
                a.PermissionType
            ).Single(m => 
                m.UserId == user.Id && 
                m.QuadroId == item.QuadroId
            ) ;
            
            if(permission == null)
                return NotFound()
            ;
            if(!permission.PermissionType.EditQuadro)
                return NotFound() 
            ;

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.Id == id);
        }
    }
}
