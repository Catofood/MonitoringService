using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeviceSessionsController : ControllerBase
{
    // Если бы в тз не было написано "Данные хранить in-memory", то я бы хранил их в PSQL
    // И создание Id для сессии было автоинкрементным на стороне БД:
    private static int _sessionId = 1;
    private static List<DeviceSessionDtoToSend> _deviceSessions = new();

    [HttpPost]
    public IActionResult Post([FromBody] DeviceSessionDtoReceived deviceSessionDtoReceived)
    {
        var log = $"POST at {DateTime.Now}: ";
        _deviceSessions.Add(new DeviceSessionDtoToSend(deviceSessionDtoReceived, _sessionId++));
        log += $"Successful. Device Session added. Now there's {_deviceSessions.Count} device sessions.";
        Console.WriteLine(log);
        return Ok(log);
    }

    [HttpGet("deviceIds")]
    public IActionResult GetNames()
    {
        var log = $"GET at {DateTime.Now}: ";
        var ids = _deviceSessions.Select(x => x.Id).Distinct().ToList();
        Console.WriteLine($"GET at {DateTime.Now}: Sent {ids.Count} device names.");
        return Ok(ids);
    }
    
    [HttpGet("deviceId/{id}")]
    public IActionResult GetByDeviceId(Guid id)
    {
        var log = $"GET at {DateTime.Now}: ";
        var dSessions = _deviceSessions.Where(dSession => dSession.Id == id).ToList();
        if (dSessions.Any())
        {
            log += $"Sent {dSessions.Count} device session(s) with ID: {id}.";
            Console.WriteLine(log);
            return Ok(dSessions);
        }

        log += $"No device sessions with ID {id} were found.";
        Console.WriteLine(log);
        return NotFound(log);
    }
    

    [HttpDelete("deviceId/{id}")]
    public IActionResult DeleteById(Guid id)
    {
        var log = $"DELETE at {DateTime.Now}: ";
        var deletedCount = _deviceSessions.RemoveAll(dSession => dSession.Id == id);
        if (deletedCount > 0)
        {
            log += $"Successfully deleted {deletedCount} device session(s) with ID: {id}. {_deviceSessions.Count} device session(s) left.";
            Console.WriteLine(log);
            return Ok(_deviceSessions);
        }
        log += $"No device sessions with device ID {id} were found.";
        Console.WriteLine(log);
        return NotFound(log);
    }
    
    [HttpDelete("sessionId/{sessionId}")]
    public IActionResult DeleteBySessionId(int sessionId)
    {
        var log = $"DELETE at {DateTime.Now}: ";
        var deletedCount = _deviceSessions.RemoveAll(dSession => dSession.SessionId == sessionId);
        if (deletedCount > 0)
        {
            log += $"Successfully deleted session with ID: {sessionId}. {_deviceSessions.Count} device session(s) left.";
            Console.WriteLine(log);
            return Ok(_deviceSessions);
        }
        log += $"No session with ID {sessionId} were found.";
        Console.WriteLine(log);
        return NotFound(log);
    }
    
    [HttpDelete("name/{name}")]
    public IActionResult DeleteByName(string name)
    {
        var log = $"DELETE at {DateTime.Now}: ";
        var deletedCount = _deviceSessions
            .RemoveAll(dSession => string.Equals(dSession.Name, name, StringComparison.CurrentCultureIgnoreCase));
        if (deletedCount > 0)
        {
            log += $"Successfully deleted {deletedCount} device session(s) with name: \"{name}\". Now there's {_deviceSessions.Count} device sessions.";
            Console.WriteLine(log);
            return Ok(_deviceSessions);
        }
        log += $"No device sessions with name {name} were found.";
        Console.WriteLine(log);
        return NotFound(log);
    }

    [HttpGet("name/{name}")]
    public IActionResult GetByName(string name)
    {
        var dSessions = _deviceSessions
            .Where(dSession => string.Equals(dSession.Name, name, StringComparison.CurrentCultureIgnoreCase))
            .ToList();
        var log = $"GET at {DateTime.Now}: ";
        if (dSessions.Any())
        {
            log += $"Sent {dSessions.Count} device sessions with name: \"{name}\".";
            Console.WriteLine(log);
            return Ok(dSessions);
        }
        log += $"No device sessions with name \"{name}\" were found.";
        Console.WriteLine(log);
        return NotFound(log);
    }

    [HttpGet]
    public IActionResult Get()
    {
        var log = $"GET at {DateTime.Now}: Sent {_deviceSessions.Count().ToString()} Device Sessions.";
        Console.WriteLine(log);
        return Ok(_deviceSessions);
    }
}