using Microsoft.AspNetCore.Mvc;
using PostBoard.Api.Models;
using PostBoard.Api.Validators;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
    public async Task<IActionResult> GetAllAsync()
    {
        var posts =await _dbcontext.Posts.ToListAsync();
        
        return Ok(posts);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var post =await _dbcontext.Posts.FirstOrDefaultAsync(p => p.Id == id);

        if (post == null)
        {
            return NotFound($"Post with Id={id} not found.");
        }

        return Ok(post);
    }

    [HttpPost]
    public async Task<IActionResult>  CreateAsync([FromBody] Post input)
    {
        var validator = new PostValidator();
        var validationResult = validator.Validate(input);

        if (!validationResult.IsValid)
        {
            return BadRequest("Validation failed.");
        }
        _dbcontext.Posts.Add(input);
        await _dbcontext.SaveChangesAsync();

        return Ok(input);
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] Post input)
    {
        var validator = new PostValidator();
        var validationResult = validator.Validate(input);

        if (!validationResult.IsValid)
        {
            return BadRequest("Validation failed.");
        }

        var post = await _dbcontext.Posts.FirstOrDefaultAsync(p => p.Id == id);
        if (post == null)
        {
            return NotFound($"Post with Id={id} not found.");
        }

        post.Title = input.Title;
        post.Body = input.Body;
        await _dbcontext.SaveChangesAsync();
        
        return Ok(post);
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        var post =await _dbcontext.Posts.FirstOrDefaultAsync(p => p.Id == id);
        if (post == null)
        {
            return NotFound($"Post with Id={id} not found.");
        }

        _dbcontext.Remove(post);
       await _dbcontext.SaveChangesAsync();

        return Ok();
    }
}
