using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using WebApi.Models;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeviceSessionsController : ControllerBase
{
    private static int _sessionId = 1;
    private static List<DeviceSessionDtoToSend> _deviceSessions = new();
    private const string BackupFilePath = $"backup_device_sessions.json";

    [HttpPost]
    public IActionResult Post([FromBody] DeviceSessionDtoReceived deviceSessionDtoReceived)
    {
        var log = $"POST at {DateTime.Now}: ";
        var dto = new DeviceSessionDtoToSend()
        {
            SessionId = _sessionId++,
            StartTime = deviceSessionDtoReceived.StartTime,
            EndTime = deviceSessionDtoReceived.EndTime,
            Version = deviceSessionDtoReceived.Version,
            Id = deviceSessionDtoReceived.Id,
            Name = deviceSessionDtoReceived.Name
        };
        _deviceSessions.Add(dto);
        log += $"Successful. Device Session added. Now there's {_deviceSessions.Count} device sessions.";
        Console.WriteLine(log);
        return Ok(log);
    }

    [HttpDelete("backup")]
    public IActionResult DeleteBackup()
    {
        System.IO.File.Delete(BackupFilePath);
        return Ok();
    }

    [HttpPost("backup")]
    public IActionResult CreateBackup()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var jsonString = JsonSerializer.Serialize(_deviceSessions, options);
        System.IO.File.WriteAllText(BackupFilePath, jsonString);
        return Ok();
    }

    [HttpGet("backupFile")]
    public IActionResult GetBackupFile()
    {
        if (System.IO.File.Exists(BackupFilePath))
        {
            var fileBytes = System.IO.File.ReadAllBytes(BackupFilePath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, BackupFilePath);
        }

        return NotFound();
    }

    [HttpGet("backup")]
    public IActionResult LoadBackup()
    {
        if (System.IO.File.Exists(BackupFilePath))
        {
            try
            {
                Log.Information("Attempting to load backup from file: {BackupFilePath} at {Timestamp}", BackupFilePath,
                    DateTime.Now);

                var json = System.IO.File.ReadAllText(BackupFilePath);
                var deviceSessionsFromBackup = JsonSerializer.Deserialize<List<DeviceSessionDtoToSend>>(json);

                if (deviceSessionsFromBackup != null)
                {
                    Log.Information("Successfully loaded backup. Restoring {SessionCount} device sessions from backup.",
                        deviceSessionsFromBackup.Count);
                    _deviceSessions = deviceSessionsFromBackup;
                    return Ok(_deviceSessions);
                }
                else
                {
                    Log.Warning(
                        "Failed to parse backup file: Invalid JSON format in file {BackupFilePath} at {Timestamp}",
                        BackupFilePath, DateTime.Now);
                    return BadRequest("Error parsing JSON");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while loading backup from file: {BackupFilePath} at {Timestamp}",
                    BackupFilePath, DateTime.Now);
                return StatusCode(500, $"Error loading backup: {ex.Message}");
            }
        }
        else
        {
            Log.Warning("Backup file not found: {BackupFilePath} at {Timestamp}", BackupFilePath, DateTime.Now);
            return NotFound("Backup file not found.");
        }
    }


    [HttpGet("deviceIds")]
    public IActionResult GetNames()
    {
        var ids = _deviceSessions.Select(x => x.Id).Distinct().ToList();

        Log.Information("GET at {Timestamp}: Sent {DeviceCount} device names.", DateTime.Now, ids.Count);

        return Ok(ids);
    }


    [HttpGet("deviceId/{id}")]
    public IActionResult GetByDeviceId(Guid id)
    {
        var dSessions = _deviceSessions.Where(dSession => dSession.Id == id).ToList();

        if (dSessions.Any())
        {
            Log.Information("GET at {Timestamp}: Sent {SessionCount} device session(s) with ID: {DeviceId}.",
                DateTime.Now, dSessions.Count, id);
            return Ok(dSessions);
        }

        Log.Warning("GET at {Timestamp}: No device sessions with ID {DeviceId} were found.", DateTime.Now, id);
        return NotFound($"No device sessions with ID {id} were found.");
    }


    [HttpDelete("deviceId/{id}")]
    public IActionResult DeleteById(Guid id)
    {
        var deletedCount = _deviceSessions.RemoveAll(dSession => dSession.Id == id);

        if (deletedCount > 0)
        {
            Log.Information(
                "DELETE at {Timestamp}: Successfully deleted {DeletedCount} device session(s) with ID: {DeviceId}. {RemainingCount} device session(s) left.",
                DateTime.Now, deletedCount, id, _deviceSessions.Count);
            return Ok(_deviceSessions);
        }

        Log.Warning("DELETE at {Timestamp}: No device sessions with device ID {DeviceId} were found.", DateTime.Now,
            id);
        return NotFound($"No device sessions with device ID {id} were found.");
    }


    [HttpDelete("sessionId/{sessionId}")]
    public IActionResult DeleteBySessionId(int sessionId)
    {
        var deletedCount = _deviceSessions.RemoveAll(dSession => dSession.SessionId == sessionId);

        if (deletedCount > 0)
        {
            Log.Information(
                "DELETE at {Timestamp}: Successfully deleted session with ID: {SessionId}. {RemainingCount} device session(s) left.",
                DateTime.Now, sessionId, _deviceSessions.Count);
            return Ok(_deviceSessions);
        }

        Log.Warning("DELETE at {Timestamp}: No session with ID {SessionId} were found.", DateTime.Now, sessionId);
        return NotFound($"No session with ID {sessionId} were found.");
    }


    [HttpDelete("name/{name}")]
    public IActionResult DeleteByName(string name)
    {
        var deletedCount = _deviceSessions
            .RemoveAll(dSession => string.Equals(dSession.Name, name, StringComparison.CurrentCultureIgnoreCase));

        if (deletedCount > 0)
        {
            Log.Information(
                "DELETE at {Timestamp}: Successfully deleted {DeletedCount} device session(s) with name: \"{DeviceName}\". {RemainingCount} device session(s) left.",
                DateTime.Now, deletedCount, name, _deviceSessions.Count);
            return Ok(_deviceSessions);
        }

        Log.Warning("DELETE at {Timestamp}: No device sessions with name \"{DeviceName}\" were found.", DateTime.Now,
            name);
        return NotFound($"No device sessions with name {name} were found.");
    }


    [HttpGet("name/{name}")]
    public IActionResult GetByName(string name)
    {
        var dSessions = _deviceSessions
            .Where(dSession => string.Equals(dSession.Name, name, StringComparison.CurrentCultureIgnoreCase))
            .ToList();

        if (dSessions.Any())
        {
            Log.Information("GET at {Timestamp}: Sent {SessionCount} device session(s) with name: \"{DeviceName}\".",
                DateTime.Now, dSessions.Count, name);
            return Ok(dSessions);
        }

        Log.Warning("GET at {Timestamp}: No device sessions with name \"{DeviceName}\" were found.", DateTime.Now,
            name);
        return NotFound($"No device sessions with name \"{name}\" were found.");
    }

    [HttpGet]
    public IActionResult Get()
    {
        Log.Information("GET at {Timestamp}: Sent {DeviceSessionCount} Device Sessions.", DateTime.Now,
            _deviceSessions.Count());
        return Ok(_deviceSessions);
    }
}