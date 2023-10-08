using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PostBoard.Api.Models;
using PostBoard.Api.Services.Contracts;


namespace PostBoard.Api.Services;

public class PostService : IPostService
{
    private readonly PostBoardContext _dbContext;
  

    public PostService(PostBoardContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<Post>> GetAllAsync()
    {
        return await _dbContext.Posts.ToListAsync();
    }

    public async Task<Post> GetByIdAsync(int id)
    {
        return await _dbContext.Posts.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<int> CreateAsync(Post input)
    {
        _dbContext.Posts.Add(input);
        await _dbContext.SaveChangesAsync();
        return input.Id;
    }

    public Task UpdateAsync(int id, Post input)
    {
        throw new NotImplementedException();
    }


    public async Task UpDateAsync(int id,
        Post input)
    {
        var post = await _dbContext.Posts.FirstOrDefaultAsync(p => p.Id == id);
        if (post == null)
        {
            throw new InvalidOperationException("Post not found");
        }

        post.Title = input.Title;
        post.Body = input.Body;
        
        await _dbContext.SaveChangesAsync();
    }
    public async Task DeleteByIdAsync(int id)
    {
        var post = await _dbContext.Posts.FirstOrDefaultAsync(p => p.Id == id);
        if (post == null)
        {
            throw new InvalidOperationException("Post not found");
        }

        _dbContext.Posts.Remove(post);
        await _dbContext.SaveChangesAsync();
    }
}