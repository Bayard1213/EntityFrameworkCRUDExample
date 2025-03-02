using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TechnomediaTestTask.DTOs;

namespace TechnomediaTestTask.Services
{
    public class TeamsService
    {
        private readonly TechnomediaTestTaskDBEntities _context;

        public TeamsService(TechnomediaTestTaskDBEntities context)
        {
            _context = context;
        }

        public IEnumerable<TeamDTO> GetAllTeams()
        {
            return _context.teams
                .Select(t => new TeamDTO
                {
                    Id = t.id,
                    Name = t.name,
                })
                .ToList();
        }

        public TeamDTO GetTeamById(int id)
        {
            return _context.teams.
                Where(t => t.id == id)
                .Select(t => new TeamDTO
                {
                    Id = t.id,
                    Name = t.name,
                })
                .FirstOrDefault();
        }

        public void CreateTeam(CreateTeamDTO newTeamDTO)
        {
            if (string.IsNullOrWhiteSpace(newTeamDTO.Name))
            {
                throw new ArgumentException("Team name cannot be null or empty.");
            }

            var newTeam = new teams
            {
                name = newTeamDTO.Name,
            };

            _context.teams.Add(newTeam);
            _context.SaveChanges();
        }

        public void UpdateTeam(int id, UpdateTeamDTO updatedTeamDTO)
        {
            var existingTeam = _context.teams.Find(id);

            if (existingTeam == null)
            {
                throw new ArgumentException("Invalid Team ID.");
            }

            existingTeam.name = updatedTeamDTO.Name;

            SetEntityStateModified(existingTeam);
            _context.SaveChanges();
        }

        internal virtual void SetEntityStateModified(teams team)
        {
            _context.Entry(team).State = EntityState.Modified;
        }

        public void DeleteTeam(int id)
        {
            var team = _context.teams.Find(id);

            if (team == null)
            {
                throw new ArgumentException("Invalid Team ID.");
            }

            _context.teams.Remove(team);
            _context.SaveChanges();
        }
    }
}