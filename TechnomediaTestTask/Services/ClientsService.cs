using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TechnomediaTestTask.DTOs.Assignment;
using TechnomediaTestTask.DTOs;
using TechnomediaTestTask.DTOs.Client;

namespace TechnomediaTestTask.Services
{
    public class ClientsService
    {
        private readonly TechnomediaTestTaskDBEntities _context;

        public ClientsService(TechnomediaTestTaskDBEntities context)
        {
            _context = context;
        }

        public IEnumerable<ClientDTO> GetAllClients()
        {
            return _context.clients
                .Select(c => new ClientDTO
                {
                    Id = c.id,
                    Name = c.name,
                    ContactInfo = c.contact_info,
                })
                .ToList();
        }

        public ClientDTO GetClientById(int id)
        {
            return _context.clients.
                Where(c => c.id == id)
                .Select(c => new ClientDTO
                {
                    Id = c.id,
                    Name = c.name,
                    ContactInfo = c.contact_info,
                })
                .FirstOrDefault();
        }

        public void CreateClient(CreateClientDTO newClientDTO)
        {
            var newClient = new clients
            {
                name = newClientDTO.Name,
                contact_info = newClientDTO.ContactInfo,
            };

            _context.clients.Add(newClient);
            _context.SaveChanges();
        }

        public void UpdateClient(int id, UpdateClientDTO updatedClientDTO)
        {
            var existingClient = _context.clients.Find(id);

            if (existingClient == null)
            {
                throw new ArgumentException("Invalid Client ID.");
            }

            existingClient.name = updatedClientDTO.Name;
            existingClient.contact_info = updatedClientDTO.ContactInfo;

            _context.Entry(existingClient).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteClient(int id)
        {
            var client = _context.clients.Find(id);

            if (client == null)
            {
                throw new ArgumentException("Invalid Client ID.");
            }

            _context.clients.Remove(client);
            _context.SaveChanges();
        }
    }
}