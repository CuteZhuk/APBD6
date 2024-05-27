namespace APBD6.Controllers;

using APBD6.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
    [Route("api/[controller]")]
    public class AnimalsController : ControllerBase
    {
        private readonly AnimalsContext _context;

        public AnimalsController(AnimalsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAnimals([FromQuery] string orderBy = "name")
        {
            var validColumns = new HashSet<string> { "name", "description", "category", "area" };
            if (!validColumns.Contains(orderBy.ToLower()))
            {
                orderBy = "name";
            }

            var animals = await _context.Animals
                .OrderBy(a => EF.Property<object>(a, orderBy))
                .ToListAsync();

            return Ok(animals);
        }

        [HttpPost]
        public async Task<IActionResult> AddAnimal([FromBody] Animal newAnimal)
        {
            if (newAnimal == null || string.IsNullOrWhiteSpace(newAnimal.Name) || string.IsNullOrWhiteSpace(newAnimal.Category) || string.IsNullOrWhiteSpace(newAnimal.Area))
            {
                return BadRequest("Invalid animal data");
            }

            _context.Animals.Add(newAnimal);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAnimals), new { id = newAnimal.IdAnimal }, newAnimal);
        }

        [HttpPut("{idAnimal}")]
        public async Task<IActionResult> UpdateAnimal(int idAnimal, [FromBody] Animal updatedAnimal)
        {
            if (updatedAnimal == null || idAnimal <= 0 || string.IsNullOrWhiteSpace(updatedAnimal.Name) || string.IsNullOrWhiteSpace(updatedAnimal.Category) || string.IsNullOrWhiteSpace(updatedAnimal.Area))
            {
                return BadRequest("Invalid animal data");
            }

            var animal = await _context.Animals.FindAsync(idAnimal);
            if (animal == null)
            {
                return NotFound("Animal not found");
            }

            animal.Name = updatedAnimal.Name;
            animal.Description = updatedAnimal.Description;
            animal.Category = updatedAnimal.Category;
            animal.Area = updatedAnimal.Area;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{idAnimal}")]
        public async Task<IActionResult> DeleteAnimal(int idAnimal)
        {
            if (idAnimal <= 0)
            {
                return BadRequest("Invalid animal ID");
            }

            var animal = await _context.Animals.FindAsync(idAnimal);
            if (animal == null)
            {
                return NotFound("Animal not found");
            }

            _context.Animals.Remove(animal);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }