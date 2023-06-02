using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VillaAPI.Data;
using VillaAPI.Models;
using VillaAPI.Models.Dto;

namespace VillaAPI.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly ILogger<VillaAPIController> _logger;
        private readonly ApplicationDbContext _context;

        public VillaAPIController(ILogger<VillaAPIController> logger, ApplicationDbContext context) 
        {
            this._logger = logger;
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            _logger.LogInformation("Fetching Villas");
            return Ok(_context.Villas);
        }

        [HttpGet("{id:int}", Name="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogError("No such id " + id);
                return BadRequest();
            }
            var villa = _context.Villas.FirstOrDefault(x=> x.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villaDTO)
        {   
            if (_context.Villas.FirstOrDefault(x=>x.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Name already Exists");
                return BadRequest(ModelState);
            }
            if (villaDTO == null)
            {
                return BadRequest(villaDTO);
            }
            if (villaDTO.Id > 0) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            Villa villaToAdd = new()
            {
                Id = villaDTO.Id,
                Name = villaDTO.Name,
                Description = villaDTO.Description,
                Amenity = villaDTO.Amenity,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft,
                Occupancy = villaDTO.Occupancy,
                ImageUrl = villaDTO.ImageUrl,
            };
            _context.Villas.Add(villaToAdd);
            _context.SaveChanges();
            
            return CreatedAtRoute("GetVilla", new { id = villaDTO.Id }, villaDTO);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = _context.Villas.FirstOrDefault(x=>x.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            _context.Villas.Remove(villa);
            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        public IActionResult UpdateVilla(int id, [FromBody]VillaDTO villaDTO)
        {
            if (villaDTO == null || id != villaDTO.Id)
            {
                return BadRequest();
            }
            ;

            Villa villaToUpdate = new()
            {
                Id = villaDTO.Id,
                Name = villaDTO.Name,
                Description = villaDTO.Description,
                Amenity = villaDTO.Amenity,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft,
                Occupancy = villaDTO.Occupancy,
                ImageUrl = villaDTO.ImageUrl,
            };
            _context.Villas.Update(villaToUpdate);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "PatchVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PatchVilla(int id, JsonPatchDocument<VillaDTO> patch)
        {
            if (patch == null || id == 0)
            {
                return BadRequest();
            }
            var villa = _context.Villas.AsNoTracking().FirstOrDefault(x=> x.Id == id);
            VillaDTO villaToPatch = new()
            {
                Id = villa.Id,
                Name = villa.Name,
                Description = villa.Description,
                Amenity = villa.Amenity,
                Rate = villa.Rate,
                Sqft = villa.Sqft,
                Occupancy = villa.Occupancy,
                ImageUrl = villa.ImageUrl,
            };
            if (villa == null)
            {
                return BadRequest();
            }
            patch.ApplyTo(villaToPatch, ModelState);
            Villa model = new Villa()
            {
                Id = villaToPatch.Id,
                Name = villaToPatch.Name,
                Description = villaToPatch.Description,
                Amenity = villaToPatch.Amenity,
                Rate = villaToPatch.Rate,
                Sqft = villaToPatch.Sqft,
                Occupancy = villaToPatch.Occupancy,
                ImageUrl = villaToPatch.ImageUrl,
            };

            _context.Update(model);
            _context.SaveChanges();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }
}
