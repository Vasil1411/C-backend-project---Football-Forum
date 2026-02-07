using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using WebApi.Services;

namespace WebApi.Controllers;


[Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly TeamService _teamService;

        
        public TeamsController(TeamService teamService)
        {
            _teamService = teamService;
        }

        
    [HttpPost]
    [Authorize] 
    public IActionResult Create(CreateTeamDto dto)
    {
        
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Unauthorized();

        var userId = int.Parse(userIdClaim.Value);

        _teamService.Create(dto, userId);
        return Ok(new { message = "Отборът е създаден успешно!" });
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_teamService.GetAllTeams());
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var team = _teamService.GetTeamById(id);
        if (team == null) return NotFound();
        return Ok(team);
    }

    [HttpPut("{id}")]
    [Authorize]
    public IActionResult Update(int id, CreateTeamDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var success = _teamService.Update(id, userId, dto);

        if (!success)
        {
            
            return Forbid(); 
        }

        return Ok(new { message = "Отборът е обновен!" });
    }

    [HttpDelete("{id}")]
    [Authorize]
    public IActionResult Delete(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var success = _teamService.Delete(id, userId);

        if (!success) return Forbid();

        return Ok(new { message = "Отборът е изтрит!" });
    }

    }