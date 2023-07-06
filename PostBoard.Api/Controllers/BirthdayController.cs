using Microsoft.AspNetCore.Mvc;
using PostBoard.Api.Data;
using PostBoard.Api.Validators;

namespace PostBoard.Api.Controllers;

[Route("api/birthdays")]
[ApiController]
public class BirthdayController : ControllerBase
{
    private static List<Birthday> Birthdays = new List<Birthday>();

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(Birthdays);
    }

    [HttpGet]
    [Route("{id:int}")]
    public IActionResult GetById([FromRoute] int id)
    {
        var birthday = Birthdays.FirstOrDefault(b => b.Id == id);

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
        var result = validator.Validate(input);

        if (!result.IsValid)
        {
            return BadRequest("Validation error.");
        }

        if (Birthdays.Count == 0)
        {
            input.Id = 1;
        }
        else
        {
            var maxId = Birthdays.Max(b => b.Id);
            input.Id = maxId + 1;
        }

        Birthdays.Add(input);

        return Ok(input);
    }

    [HttpPut]
    [Route("{id:int}")]
    public IActionResult Update([FromRoute] int id, [FromBody] Birthday input)
    {
        var validator = new BirthdayValidator();
        var result = validator.Validate(input);

        if (!result.IsValid)
        {
            return BadRequest("Validation error.");
        }

        var birthday = Birthdays.FirstOrDefault(b => b.Id == id);
        if (birthday == null)
        {
            return NotFound($"Birthday with Id={id} not found.");
        }

        birthday.UserFullName = input.UserFullName;
        birthday.Date = input.Date;
        
        return Ok(birthday);
    }

    [HttpDelete]
    [Route("{id:int}")]
    public IActionResult Delete([FromRoute] int id)
    {
        var birthday = Birthdays.FirstOrDefault(b => b.Id == id);
        if (birthday == null)
        {
            return NotFound($"Birthday with Id={id} not found.");
        }

        Birthdays.Remove(birthday);

        return Ok();
    }
}
