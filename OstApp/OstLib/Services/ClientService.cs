using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OstLib.Repository;

namespace OstLib.Services
{
    public class ClientService : IClient
    {
        private readonly ApplicationContext _context;
        public ClientService(ApplicationContext context)
        {
            _context = context;
        }

        public void Add(Client client)
        {
            _context.Client.Add(client);
            _context.SaveChanges();
        }

        public Client GetById(int id) => _context.Client.FirstOrDefault(c=>c.ID == id);

        public IEnumerable<Client> FindClientsBySurname(string surname)
        {
            try
            {
                surname = surname.ToLower();
                var firstLetter = surname.Substring(0, 1);
                firstLetter = firstLetter.ToUpper();
                surname = surname.Substring(1);
                surname = firstLetter + surname;
                return _context.Client.Where(l => l.Surname.Contains(surname)).ToList();
            }
            catch
            {
                return new List<Client>();
            }
        }

        public IEnumerable<Client> FindAll() => _context.Client.AsNoTracking();
    }
}
