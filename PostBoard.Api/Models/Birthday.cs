using System;

namespace PostBoard.Api.Models;

public class Birthday
{
    public int Id { get; set; }
    public string UserFullName { get; set; }
    public DateTime Date { get; set; }
}
