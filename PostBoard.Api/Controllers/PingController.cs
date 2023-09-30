﻿using Microsoft.AspNetCore.Mvc;

namespace PostBoard.Api.Controllers;

[Route("api/ping")]
[ApiController]
public class PingController : ControllerBase
{
    [HttpGet]
    public IActionResult Ping()
    {
        return  Ok();
    }
}
