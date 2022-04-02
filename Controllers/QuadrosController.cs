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
    public class QuadrosController : ControllerBase
    {
        private readonly Context _context;

        public QuadrosController(Context context)
        {
            _context = context;
        }

        // GET: api/Quadros
        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<Quadro>>> GetQuadros()
        // {
        //     return await _context.Quadros.ToListAsync();
        // }


        // GET: api/Quadros/5
        [HttpGet("{id}/{token}")]
        public async Task<ActionResult<Quadro>> GetQuadro(int id, string token)
        {

            User user = await _context.Users.FirstOrDefaultAsync(m => m.ApiToken == token) ;
            
            if(user != null){

                var quadro = _context.Quadros.Where(m => m.AmbienteId == id);

                if (quadro == null)
                {
                    return NotFound();
                }

                return Ok(quadro);
            }else return NotFound() ;
        }


        // PUT: api/Quadros/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}/{token}")]
        public async Task<IActionResult> PutQuadro(int id, string token, Quadro quadro)
        {

            User user = await _context.Users.FirstOrDefaultAsync(m => m.ApiToken == token) ;
            if(user == null)
                return NotFound() 
            ;

            Quadro_Permission permission = _context.Quadro_Permissions.Include(a => 
                a.PermissionType
            ).Single(m => 
                m.UserId == user.Id && 
                m.QuadroId == quadro.Id
            ) ;
            
            if(permission == null)
                return NotFound()
            ;
            if(!permission.PermissionType.EditQuadro)
                return NotFound() 
            ;


            if (id != quadro.Id)
            {
                return BadRequest();
            }

            _context.Entry(quadro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuadroExists(id))
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

        // POST: api/Quadros
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{token}")]
        public async Task<ActionResult<Quadro>> PostQuadro(string token, Quadro quadro)
        {

            User user = await _context.Users.FirstOrDefaultAsync(m => m.ApiToken == token) ;
            if(user == null)
                return NotFound() 
            ;


            _context.Quadros.Add(quadro);
            await _context.SaveChangesAsync();

            Quadro_Permission permission = new Quadro_Permission{
                UserId = user.Id,
                QuadroId = quadro.Id,
                PermissionTypeId = 5
            } ;
            _context.Quadro_Permissions.Add(permission) ;
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuadro", new { id = quadro.Id, token = token }, quadro);
        }

        // DELETE: api/Quadros/5
        [HttpDelete("{id}/{token}")]
        public async Task<IActionResult> DeleteQuadro(int id, string token)
        {

            User user = await _context.Users.FirstOrDefaultAsync(m => m.ApiToken == token) ;
            if(user == null)
                return NotFound("user null") 
            ;

            Quadro_Permission permission = _context.Quadro_Permissions.Include(a => 
                a.PermissionType
            ).Single(m => 
                m.UserId == user.Id && 
                m.QuadroId == id
            ) ;
            
            if(permission == null)
                return NotFound("permission null")
            ;
            if(!permission.PermissionType.DeleteQuadro)
                return NotFound("not allowed") 
            ;


            var quadro = await _context.Quadros.FindAsync(id);
            if (quadro == null)
            {
                return NotFound("not found quadro");
            }

            _context.Quadros.Remove(quadro);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuadroExists(int id)
        {
            return _context.Quadros.Any(e => e.Id == id);
        }
    }
}
