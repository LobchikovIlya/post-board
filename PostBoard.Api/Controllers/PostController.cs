using Microsoft.AspNetCore.Mvc;
using PostBoard.Api.Models;
using PostBoard.Api.Validators;
using System.Collections.Generic;
using System.Linq;

namespace PostBoard.Api.Controllers;

[Route("api/posts")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly PostBoardContext _dbcontext;

    public PostController(PostBoardContext context)
    {
        _dbcontext = context;
    }
    

    [HttpGet]
    public IActionResult GetAll()
    {
        var posts = _dbcontext.Posts.ToList();
        return Ok(posts);
    }

    [HttpGet]
    [Route("{id:int}")]
    public IActionResult GetById([FromRoute] int id)
    {
        var post =_dbcontext.Posts.FirstOrDefault(p => p.Id == id);

        if (post == null)
        {
            return NotFound($"Post with Id={id} not found.");
        }

        return Ok(post);
    }

    [HttpPost]
    public IActionResult Create([FromBody] Post input)
    {
        var validator = new PostValidator();
        var validationResult = validator.Validate(input);

        if (!validationResult.IsValid)
        {
            return BadRequest("Validation failed.");
        }
        _dbcontext.Posts.Add(input);
        _dbcontext.SaveChanges();

        return Ok(input);
    }

    [HttpPut]
    [Route("{id:int}")]
    public IActionResult Update([FromRoute] int id, [FromBody] Post input)
    {
        var validator = new PostValidator();
        var validationResult = validator.Validate(input);

        if (!validationResult.IsValid)
        {
            return BadRequest("Validation failed.");
        }

        var post =_dbcontext.Posts.FirstOrDefault(p => p.Id == id);
        if (post == null)
        {
            return NotFound($"Post with Id={id} not found.");
        }

        post.Title = input.Title;
        post.Body = input.Body;
        _dbcontext.SaveChanges();
        
        return Ok(post);
    }

    [HttpDelete]
    [Route("{id:int}")]
    public IActionResult Delete([FromRoute] int id)
    {
        var post = _dbcontext.Posts.FirstOrDefault(p => p.Id == id);
        if (post == null)
        {
            return NotFound($"Post with Id={id} not found.");
        }

        _dbcontext.Remove(post);
        _dbcontext.SaveChanges();

        return Ok();
    }
}
