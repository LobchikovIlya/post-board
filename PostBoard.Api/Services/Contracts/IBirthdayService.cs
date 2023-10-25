using System.Collections.Generic;
using System.Threading.Tasks;
using PostBoard.Api.Models;

namespace PostBoard.Api.Services.Contracts;

public interface IBirthdayService
{
    Task<IList<Birthday>> GetAllAsync();
    Task<Birthday> GetByIdAsync(int id);
    Task<int> CreateAsync(Birthday input);
    Task UpdateAsync(int id, Birthday input);
    Task DeleteByIdAsync(int id);
}