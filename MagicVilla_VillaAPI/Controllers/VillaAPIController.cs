using System.Text.RegularExpressions;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers;

// [Route("api/[controller]")]
[Route("api/VillaAPI")]
[ApiController]
public class VillaAPIController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<VillaDto>> GetVillas()
    {
        return Ok(VillaStore.villaList);
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

        var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == Id);
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

        if (VillaStore.villaList.FirstOrDefault(u => u.Name.ToLower() == villaDto.Name.ToLower()) != null) {
            ModelState.AddModelError("CustomError", "Villa already Exists!");
            return BadRequest(ModelState);
        }

        villaDto.Id = VillaStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
        VillaStore.villaList.Add(villaDto);

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

        var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == Id);
        if (villa == null) {
            return NotFound();
        }

        VillaStore.villaList.Remove(villa);

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

        var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == Id);
        if (villa == null) {
            return NotFound();
        }

        villa.Name = villaDto.Name;
        villa.Occupancy = villaDto.Occupancy;
        villa.Sqft = villaDto.Sqft;

        return NoContent();
    }
}