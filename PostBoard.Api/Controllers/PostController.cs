using System;
using Microsoft.AspNetCore.Mvc;
using PostBoard.Api.Models;
using PostBoard.Api.Validators;
using System.Threading.Tasks;
using PostBoard.Api.Services.Contracts;

namespace PostBoard.Api.Controllers;

[Route("api/posts")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;

    public PostController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var posts = await _postService.GetAllAsync();
        
        return Ok(posts);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var post = await _postService.GetByIdAsync(id);

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
        var postId = await _postService.CreateAsync(input);
        // ReSharper disable once Mvc.ActionNotResolved
        return CreatedAtAction(nameof(GetByIdAsync), new { id = postId }, input);
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

        try
        {
            await _postService.UpdateAsync(id, input);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Post with Id={id} not found.");
        }
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        try
        {
            await _postService.DeleteByIdAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Post with Id={id} not found.");
        }
    }
}
