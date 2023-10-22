using System.Text.RegularExpressions;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Controllers;

// [Route("api/[controller]")]
[Route("api/VillaAPI")]
[ApiController]
public class VillaAPIController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public VillaAPIController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public ActionResult<IEnumerable<VillaDto>> GetVillas()
    {
        return Ok(_db.Villas.ToList());
    }

    [HttpGet("{Id:int}", Name = "GetVilla")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<VillaDto> GetVilla(int Id)
    {
        if (Id == 0){
            return BadRequest();
        }

        var villa = _db.Villas.FirstOrDefault(u => u.Id == Id);
        if (villa == null) {
            return NotFound();
        }
        return Ok(villa);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<VillaDto> CreateVilla([FromBody] VillaDto villaDto)
    {
        if (villaDto == null) {
            return BadRequest(villaDto);
        }

        if (villaDto.Id > 0) {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (_db.Villas.FirstOrDefault(u => u.Name.ToLower() == villaDto.Name.ToLower()) != null) {
            ModelState.AddModelError("CustomError", "Villa already Exists!");
            return BadRequest(ModelState);
        }

        Villa model = new(){
            Name = villaDto.Name,
            Details = villaDto.Details,
            ImageUrl = villaDto.ImageUrl,
            Occupancy = villaDto.Occupancy,
            Rate = villaDto.Rate,
            Sqft = villaDto.Sqft,
            Amenity = villaDto.Amenity,
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now
        };
        _db.Villas.Add(model);
        _db.SaveChanges();

        // return Ok(villaDto);
        return CreatedAtRoute("GetVilla", new {Id = villaDto.Id} , villaDto);
    }

    [HttpDelete("{Id:int}", Name = "DeleteVilla")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult DeleteVilla(int Id)
    {
        if (Id == 0) {
            return BadRequest();
        }

        var villa = _db.Villas.FirstOrDefault(u => u.Id == Id);
        if (villa == null) {
            return NotFound();
        }

        _db.Villas.Remove(villa);
        _db.SaveChanges();

        return NoContent();
    }

    [HttpPut("{Id:int}", Name = "UpdateVilla")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult UpdateVilla(int Id, [FromBody] VillaDto villaDto)
    {
        if (villaDto == null || Id != villaDto.Id) {
            return BadRequest();
        }

        // var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == Id);
        // if (villa == null) {
        //     return NotFound();
        // }
        // villa.Name = villaDto.Name;
        // villa.Occupancy = villaDto.Occupancy;
        // villa.Sqft = villaDto.Sqft;

        Villa model = new(){
            Id = villaDto.Id,
            Name = villaDto.Name,
            Details = villaDto.Details,
            ImageUrl = villaDto.ImageUrl,
            Occupancy = villaDto.Occupancy,
            Rate = villaDto.Rate,
            Sqft = villaDto.Sqft,
            Amenity = villaDto.Amenity,
            UpdatedDate = DateTime.Now
        };
        _db.Villas.Update(model);
        _db.SaveChanges();

        return NoContent();
    }

    [HttpPatch("{Id:int}", Name = "UpdatePartialVilla")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult UpdatePartialVilla(int Id, JsonPatchDocument <VillaDto> patchDto)
    {
        if (patchDto == null || Id == 0) {
            return BadRequest();
        }

        var villa = _db.Villas.AsNoTracking().FirstOrDefault(u => u.Id == Id);
        if (villa == null) {
            return NotFound();
        }

        VillaDto villaDto = new() {
            Id = villa.Id,
            Name = villa.Name,
            Details = villa.Details,
            ImageUrl = villa.ImageUrl,
            Occupancy = villa.Occupancy,
            Rate = villa.Rate,
            Sqft = villa.Sqft,
            Amenity = villa.Amenity
        };

        patchDto.ApplyTo(villaDto, ModelState);
        Villa model = new(){
            Id = villaDto.Id,
            Name = villaDto.Name,
            Details = villaDto.Details,
            ImageUrl = villaDto.ImageUrl,
            Occupancy = villaDto.Occupancy,
            Rate = villaDto.Rate,
            Sqft = villaDto.Sqft,
            Amenity = villaDto.Amenity,
            UpdatedDate = DateTime.Now
        };

        _db.Villas.Update(model);
        _db.SaveChanges();
        
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        return NoContent();
    }
}