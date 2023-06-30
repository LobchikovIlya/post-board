using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using PostBoard.Api.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PostBoard.Api.Controllers;

[Route("api/berthday")]
[ApiController]
public class BerthdayController : ControllerBase
{
    private static List<Birthday> Berthdays = new List<Birthday>();



    // GET: api/<Berthday>
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(Berthdays);
    }

    // GET api/<Berthday>/5
    [HttpGet]
    [Route("{id:int}")]
    public IActionResult GetById([FromRoute] int id)
    {
        var berthday = Berthdays.FirstOrDefault(d => d.Id == id);

        if (berthday == null)
        {
            return NotFound($"Berthday with Id={id} not found.");
        }
        return Ok(berthday);
    }

    // POST api/<Berthday>
    [HttpPost]
    public IActionResult Create([FromBody] Birthday input)
    {
        if (Berthdays.Count == 0)
        {
            input.Id = 1;
        }
        else
        {
            var maxId = Berthdays.Max(p => p.Id);
            input.Id = maxId + 1;
        }

        Berthdays.Add(input);

        return Ok(input);
    }

    // PUT api/<Berthday>/5
    [HttpPut]
    [Route("{id:int}")]
    public IActionResult Update([FromRoute] int id, [FromBody] Birthday input)
    {
        var day = Berthdays.FirstOrDefault(d => d.Id == id);
        if (day == null)
        {
            return NotFound($"berthday with Id={id} not found.");
        }

        day.UserFullName = input.UserFullName;
        day.Date = input.Date;

        return Ok(day);
    }

    // DELETE api/<Berthday>/5
    [HttpDelete]

    [Route("{id:int}")]
    public IActionResult Delete([FromRoute] int id)
    {
        var day = Berthdays.FirstOrDefault(d => d.Id == id);
        if (day == null)
        {
            return NotFound($"berthday with Id={id} not found.");
        }

        Berthdays.Remove(day);

        return Ok();
    }
}
