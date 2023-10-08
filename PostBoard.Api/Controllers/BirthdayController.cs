using System;
using Microsoft.AspNetCore.Mvc;
using PostBoard.Api.Models;
using PostBoard.Api.Validators;
using System.Threading.Tasks;
using PostBoard.Api.Services.Contracts;

namespace PostBoard.Api.Controllers;

[Route("api/birthdays")]
[ApiController]
public class BirthdayController : ControllerBase
{
    private readonly IBirthdayService _birthdayService;
 
    public BirthdayController(IBirthdayService birthdayService)
    {
        _birthdayService = birthdayService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var birthdays = await _birthdayService.GetAllAsync();
        
        return Ok(birthdays);
    }

    [HttpGet]
    [Route("{id:int}", Name = "GetByIdAsync")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var birthday = await _birthdayService.GetByIdAsync(id);

        if (birthday == null)
        {
            return NotFound($"Birthday with Id={id} not found.");
        }

        return Ok(birthday);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] Birthday input)
    {
        var validator = new BirthdayValidator();
        var validationResult = validator.Validate(input);

        if (!validationResult.IsValid)
        {
            return BadRequest("Validation error.");
        }
        var birthdayId = await _birthdayService.CreateAsync(input);
        
        
        // ReSharper disable once Mvc.ActionNotResolved
        return CreatedAtAction(nameof(GetByIdAsync), new { id = birthdayId }, input);
    }

    [HttpPut]
    [Route("{id:int}")]
    public  async Task<IActionResult>UpdateAsync([FromRoute] int id, [FromBody] Birthday input)
    {
        var validator = new BirthdayValidator();
        var validationResult = validator.Validate(input);

        if (!validationResult.IsValid)
        {
            return BadRequest("Validation error.");
        }

       
        try
        {
            await _birthdayService.UpdateAsync(id, input);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Birthday with Id={id} not found.");
        }
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        try
        {
            await _birthdayService.DeleteByIdAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Birthday with Id={id} not found.");
        }
    }
}
