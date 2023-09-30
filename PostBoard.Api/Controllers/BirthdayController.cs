using Microsoft.AspNetCore.Mvc;
using PostBoard.Api.Models;
using PostBoard.Api.Validators;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PostBoard.Api.Controllers;

[Route("api/birthdays")]
[ApiController]
public class BirthdayController : ControllerBase
{
    private readonly PostBoardContext _dbcontext;

    public BirthdayController(PostBoardContext dbContext)
    {
        _dbcontext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var birthdays = await _dbcontext.Birthdays.ToListAsync();
        
        return Ok(birthdays);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var birthday = await _dbcontext.Birthdays.FirstOrDefaultAsync(b => b.Id == id);

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
        _dbcontext.Birthdays.Add(input);
       await _dbcontext.SaveChangesAsync();

        return Ok(input);
    }

    [HttpPut]
    [Route("{id:int}")]
    public  async Task<IActionResult>  UpdateAsync([FromRoute] int id, [FromBody] Birthday input)
    {
        var validator = new BirthdayValidator();
        var validationResult = validator.Validate(input);

        if (!validationResult.IsValid)
        {
            return BadRequest("Validation error.");
        }

        var birthday =await _dbcontext.Birthdays.FirstOrDefaultAsync(b => b.Id == id);
        if (birthday == null)
        {
            return NotFound($"Birthday with Id={id} not found.");
        }
        
        birthday.UserFullName = input.UserFullName;
        birthday.Date = input.Date;
       await _dbcontext.SaveChangesAsync();
        
        return  Ok(birthday);
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        var birthday = await _dbcontext.Birthdays.FirstOrDefaultAsync(b => b.Id == id);
        if (birthday == null)
        {
            return NotFound($"Birthday with Id={id} not found.");
        }

       _dbcontext.Birthdays.Remove(birthday);
       await _dbcontext.SaveChangesAsync();

        return Ok();
    }
}
