using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PostBoard.Api.Models;
using PostBoard.Api.Services.Contracts;

namespace PostBoard.Api.Services;

public class BirthdayService : IBirthdayService
{
    private readonly PostBoardContext _dbContext;

    public BirthdayService(PostBoardContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<Birthday>> GetAllAsync()
    {
        var birthday = await _dbContext.Birthdays.ToListAsync();

        return birthday;
    }

    public async Task<Birthday> GetByIdAsync(int id)

    {
        var birthday = await _dbContext.Birthdays.FirstOrDefaultAsync(p => p.Id == id);
        if (birthday == null)
        {
            throw new InvalidOperationException("Birthday not found");
        }

        return birthday;
    }

    public async Task<int> CreateAsync(Birthday input)
    {
        await _dbContext.Birthdays.AddAsync(input);
        await _dbContext.SaveChangesAsync();

        return input.Id;
    }

    public async Task UpdateAsync(int id, Birthday input)
    {
        var birthday = await _dbContext.Birthdays.FirstOrDefaultAsync(p => p.Id == id);
        if (birthday == null)
        {
            throw new InvalidOperationException("Birthday not found");
        }

        birthday.UserFullName = input.UserFullName;
        birthday.Date = input.Date;

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteByIdAsync(int id)
    {
        var birthday = await _dbContext.Birthdays.FirstOrDefaultAsync(p => p.Id == id);
        if (birthday == null)
        {
            throw new InvalidOperationException("Birthday not found");
        }

        _dbContext.Birthdays.Remove(birthday);
        await _dbContext.SaveChangesAsync();
    }
}