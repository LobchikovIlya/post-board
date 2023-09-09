using Microsoft.AspNetCore.Mvc;
using PostBoard.Api.Models;
using PostBoard.Api.Validators;
using System.Collections.Generic;
using System.Linq;

namespace PostBoard.Api.Controllers;

[Route("api/birthdays")]
[ApiController]
public class BirthdayController : ControllerBase
{
    private readonly PostBoardContext _dbcontext;

    public BirthdayController(PostBoardContext context)
    {
        _dbcontext = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var birthdays = _dbcontext.Birthdays.ToList();
        return Ok(birthdays);
    }

    [HttpGet]
    [Route("{id:int}")]
    public IActionResult GetById([FromRoute] int id)
    {
        var birthday = _dbcontext.Birthdays.FirstOrDefault(b => b.Id == id);

        if (birthday == null)
        {
            return NotFound($"Birthday with Id={id} not found.");
        }

        return Ok(birthday);
    }

    [HttpPost]
    public IActionResult Create([FromBody] Birthday input)
    {
        var validator = new BirthdayValidator();
        var validationResult = validator.Validate(input);

        if (!validationResult.IsValid)
        {
            return BadRequest("Validation error.");
        }
        _dbcontext.Birthdays.Add(input);
        _dbcontext.SaveChanges();

        return Ok(input);
    }

    [HttpPut]
    [Route("{id:int}")]
    public IActionResult Update([FromRoute] int id, [FromBody] Birthday input)
    {
        var validator = new BirthdayValidator();
        var validationResult = validator.Validate(input);

        if (!validationResult.IsValid)
        {
            return BadRequest("Validation error.");
        }

        var birthday =_dbcontext. Birthdays.FirstOrDefault(b => b.Id == id);
        if (birthday == null)
        {
            return NotFound($"Birthday with Id={id} not found.");
        }
        
        birthday.UserFullName = input.UserFullName;
        birthday.Date = input.Date;
        _dbcontext.SaveChanges();
        
        return Ok(birthday);
    }

    [HttpDelete]
    [Route("{id:int}")]
    public IActionResult Delete([FromRoute] int id)
    {
        var birthday = _dbcontext.Birthdays.FirstOrDefault(b => b.Id == id);
        if (birthday == null)
        {
            return NotFound($"Birthday with Id={id} not found.");
        }

       _dbcontext.Birthdays.Remove(birthday);
       _dbcontext.SaveChanges();

        return Ok();
    }
}
