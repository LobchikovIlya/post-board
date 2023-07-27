using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostBoard.Api.Models;
using PostBoard.Api.Validators;
using System.Collections.Generic;
using System.Linq;

namespace PostBoard.Api.Controllers;

[Route("api/birthdays")]
[ApiController]
public class BirthdayController : ControllerBase
{
    //private static List<Birthday> Birthdays = new List<Birthday>();
    private readonly PostBoardContext _context;
    public BirthdayController(PostBoardContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var birthdays = _context.Birthdays.ToList();
        
        return Ok(birthdays);
    }

    [HttpGet]
    [Route("{id:int}")]
    public IActionResult GetById([FromRoute] int id)
    {
        var birthday = _context.Birthdays.FirstOrDefault(b => b.Id == id);

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

    /*    if (Birthdays.Count == 0)
        {
            input.Id = 1;
        }
        else
        {
            var maxId = Birthdays.Max(b => b.Id);
            input.Id = maxId + 1;
        }*/


        _context.Birthdays.Add(input);
        _context.SaveChanges();

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

        var birthday = _context.Birthdays.FirstOrDefault(b => b.Id == id);
        if (birthday == null)
        {
            return NotFound($"Birthday with Id={id} not found.");
        }

        birthday.UserFullName = input.UserFullName;
        birthday.Date = input.Date;
        _context.Entry(birthday).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok(birthday);
    }

    [HttpDelete]
    [Route("{id:int}")]
    public IActionResult Delete([FromRoute] int id)
    {
        var birthday = _context.Birthdays.FirstOrDefault(b => b.Id == id);
        if (birthday == null)
        {
            return NotFound($"Birthday with Id={id} not found.");
        }

        _context.Birthdays.Remove(birthday);
        _context.SaveChanges();

        return Ok();
    }
}
