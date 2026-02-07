using System.Collections.Generic;
using System.Linq;
using WebApi.Data;
using WebApi.DTOs;
using WebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services;


public class TeamService
{
    private readonly FootballForumDbContext _context;

    public TeamService(FootballForumDbContext context)
    {
        _context = context;
    }

   
    public void Create(CreateTeamDto dto, int userId)
    {
        var team = new Team
        {
            Name = dto.Name,
            Country = dto.Country,
            CreatorId = userId
        };
        _context.Teams.Add(team);
        _context.SaveChanges();
    }

    public List<TeamResponseDto> GetAllTeams()
    {
        return _context.Teams
            .Select(t => new TeamResponseDto
            {
                TeamId = t.Id,
                Name = t.Name,
                Country = t.Country,
                CreatorName = t.Creator.Username
               
            })
            .ToList();
    }

    public TeamResponseDto GetTeamById(int id)
    {
        return _context.Teams
            .Where(t => t.Id == id)
            .Select(t => new TeamResponseDto
            {
                TeamId = t.Id,
                Name = t.Name,
                Country = t.Country,
                CreatorName = t.Creator.Username
            })
            .FirstOrDefault();
    }

    
    public bool Update(int teamId, int userId, CreateTeamDto dto)
    {
        var team = _context.Teams.Find(teamId);

        
        if (team == null) return false;

       
        if (team.CreatorId != userId)
        {
            return false; 
        }

        team.Name = dto.Name;
        team.Country = dto.Country;
        
        _context.SaveChanges();
        return true;
    }

    
    public bool Delete(int teamId, int userId)
    {
        var team = _context.Teams.Find(teamId);

        if (team == null || team.CreatorId != userId)
        {
            return false;
        }

        _context.Teams.Remove(team);
        _context.SaveChanges();
        return true;
    }
}
    