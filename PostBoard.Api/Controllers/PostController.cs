using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostBoard.Api.Models;
using PostBoard.Api.Validators;
using System.Collections.Generic;
using System.Linq;

namespace PostBoard.Api.Controllers;

[Route("api/posts")]
[ApiController]
public class PostController : ControllerBase
{
    //private static List<Post> Posts = new List<Post>();
    private  readonly PostBoardContext _context;

    public PostController(PostBoardContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var posts = _context.Posts.ToList();
        return Ok(posts);
    }

    [HttpGet]
    [Route("{id:int}")]
    public IActionResult GetById([FromRoute] int id)
    {
        var post =_context.Posts.FirstOrDefault(p => p.Id == id);

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

       /* if (Posts.Count == 0)
        {
            input.Id = 1;
        }
        else
        {
            var maxId = Posts.Max(p => p.Id);
            input.Id = maxId + 1;
        }*/
            
        _context.Add(input);
        _context.SaveChanges();

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

        var post = _context.Posts.FirstOrDefault(p => p.Id == id);
        if (post == null)
        {
            return NotFound($"Post with Id={id} not found.");
        }

        post.Title = input.Title;
        post.Body = input.Body;
        _context.Entry(post).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok(post);
    }

    [HttpDelete]
    [Route("{id:int}")]
    public IActionResult Delete([FromRoute] int id)
    {
        var post = _context.Posts.FirstOrDefault(p => p.Id == id);
        if (post == null)
        {
            return NotFound($"Post with Id={id} not found.");
        }

        _context.Posts.Remove(post);
        _context.SaveChanges();

        return Ok();
    }
}
