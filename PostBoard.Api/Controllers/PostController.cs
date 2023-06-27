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

    [HttpPost]
    public IActionResult Create([FromBody] Post input)
    {
        if (Posts.Count == 0)
        {
            input.Id = 1;
        }
        else
        {
            var maxId = Posts.Max(post => post.Id);
            input.Id = maxId + 1;
        }
            
        Posts.Add(input);

        return Ok(input);
    }
}
