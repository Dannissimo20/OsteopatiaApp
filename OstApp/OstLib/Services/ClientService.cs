using System.Linq;
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

        public void Add(Client client) => _context.Client.Add(client);

        public Client GetById(int id) => _context.Client.FirstOrDefault(c=>c.ID == id);
    }
}
