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
        try
        {
            var post = await _postService.GetByIdAsync(id);

            return Ok(post);
        }
        catch (InvalidOperationException exception)
        {
            return NotFound(exception.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] Post input)
    {
        var validator = new PostValidator();
        var validationResult = await validator.ValidateAsync(input);

        if (!validationResult.IsValid)
        {
            return BadRequest("Validation failed.");
        }

        var postId = await _postService.CreateAsync(input);
        var createdPost = await _postService.GetByIdAsync(postId);

        return CreatedAtRoute("GetPostById", new { id = postId }, createdPost);
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] Post input)
    {
        var validator = new PostValidator();
        var validationResult = await validator.ValidateAsync(input);

        if (!validationResult.IsValid)
        {
            return BadRequest("Validation failed.");
        }

        try
        {
            await _postService.UpdateAsync(id, input);
            var updatePost = await _postService.GetByIdAsync(id);

            return Ok(updatePost);
        }
        catch (InvalidOperationException exception)
        {
            return NotFound(exception.Message);
        }
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        try
        {
            await _postService.DeleteByIdAsync(id);

            return Ok();
        }
        catch (InvalidOperationException exception)
        {
            return NotFound(exception.Message);
        }
    }
}