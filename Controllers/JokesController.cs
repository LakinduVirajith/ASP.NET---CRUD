using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASP.NET___CRUD.Data;
using ASP.NET___CRUD.Models;
using Microsoft.AspNetCore.Authorization;

namespace ASP.NET___CRUD.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = "Customer")]
    public class JokesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JokesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/v1/jokes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Joke>>> GetJokes()
        {
            return await _context.Joke.ToListAsync();
        }

        // GET: api/v1/jokes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Joke>> GetJoke(int id)
        {
            var joke = await _context.Joke.FindAsync(id);

            if (joke == null)
            {
                return NotFound();
            }

            return joke;
        }

        // POST: api/v1/jokes
        [HttpPost]
        public async Task<ActionResult<Joke>> CreateJoke(Joke joke)
        {
            if (ModelState.IsValid)
            {
                _context.Joke.Add(joke);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetJoke), new { id = joke.Id }, joke);
            }
            return BadRequest(ModelState);
        }

        // PUT: api/v1/jokes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJoke(int id, Joke joke)
        {
            if (id != joke.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _context.Entry(joke).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JokeExists(id))
                    {
                        return NotFound();
                    }
                    throw;
                }

                return NoContent();
            }
            return BadRequest(ModelState);
        }

        // DELETE: api/v1/jokes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJoke(int id)
        {
            var joke = await _context.Joke.FindAsync(id);
            if (joke == null)
            {
                return NotFound();
            }

            _context.Joke.Remove(joke);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool JokeExists(int id)
        {
            return _context.Joke.Any(e => e.Id == id);
        }
    }
}