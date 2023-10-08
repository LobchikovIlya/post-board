using System.Threading.Tasks;
using System.Collections.Generic;
using PostBoard.Api.Models;

namespace PostBoard.Api.Services.Contracts;

public interface IPostService
{
    Task<IList<Post>> GetAllAsync();
    Task<Post> GetByIdAsync(int id);
    Task<int> CreateAsync(Post input);
    Task UpdateAsync(int id, Post input);
    Task DeleteByIdAsync(int id);
}