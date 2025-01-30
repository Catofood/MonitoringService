using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeviceSessionController : ControllerBase
{
    private static List<DeviceSession> _deviceSessions = new();

    [HttpPost]
    public IActionResult Post([FromBody] DeviceSession deviceSession)
    {
        Console.WriteLine($"POST at {DateTime.Now}: Received Device Session.");
        var log = string.Empty;
        if (deviceSession == null)
        {
            log = $"POST at {DateTime.Now}: Bad request. Device Session is null.";
            Console.WriteLine(log);
            return BadRequest(log);
        }
        _deviceSessions.Add(deviceSession);
        log = $"POST at {DateTime.Now}: Successful.";
        Console.WriteLine(log);
        return Ok(log);
    }

    [HttpGet("id/{id}")]
    public IActionResult GetById(Guid id)
    {
        var dSessions = _deviceSessions.Where(dSession => dSession.Id == id).ToList();
        var log = string.Empty;
        if (dSessions.Any() == false)
        {
            log = $"GET: No device se   ssions with ID {id} were found.";
            Console.WriteLine(log);
            return NotFound(log);
        }
        log = $"GET: Sent {dSessions.Count} device sessions with ID: {id}.";
        Console.WriteLine(log);
        return Ok(dSessions);
    }

    [HttpGet("name/{name}")]
    public IActionResult GetByName(string name)
    {
        var dSessions = _deviceSessions.Where(dSession => dSession.Name == name).ToList();
        var log = string.Empty;
        if (dSessions.Any() == false)
        {
            log = $"GET: No device sessions with name {name} were found.";
            Console.WriteLine(log);
            return NotFound(log);
        }
        log = $"GET: Sent {dSessions.Count} device sessions with name: {name}.";
        Console.WriteLine(log);
        return Ok(dSessions);
    }

    [HttpGet]
    public IActionResult Get()
    {
        var log = $"GET at {DateTime.Now}: Sent {_deviceSessions.Count().ToString()} Device Sessions.";
        Console.WriteLine(log);
        return Ok(_deviceSessions);
    }
}