using Microsoft.AspNetCore.Mvc;
using PostBoard.Api.Data;

namespace PostBoard.Api.Controllers;

[Route("api/posts")]
[ApiController]
public class PostController : ControllerBase
{
    private static List<Post> Posts = new List<Post>();

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(Posts);
    }

    [HttpGet]
    [Route("{id:int}")]
    public IActionResult GetById([FromRoute] int id)
    {
        var post = Posts.FirstOrDefault(p => p.Id == id);

        if (post == null)
        {
            return NotFound($"Post with Id={id} not found.");
        }

        return Ok(post);
    }

    [HttpPost]
    public IActionResult Create([FromBody] Post input)
    {
        if (Posts.Count == 0)
        {
            input.Id = 1;
        }
        else
        {
            var maxId = Posts.Max(p => p.Id);
            input.Id = maxId + 1;
        }
            
        Posts.Add(input);

        return Ok(input);
    }

    [HttpPut]
    [Route("{id:int}")]
    public IActionResult Update([FromRoute] int id, [FromBody] Post input)
    {
        var post = Posts.FirstOrDefault(p => p.Id == id);
        if (post == null)
        {
            return NotFound($"Post with Id={id} not found.");
        }

        post.Title = input.Title;
        post.Body = input.Body;

        return Ok(post);
    }

}
