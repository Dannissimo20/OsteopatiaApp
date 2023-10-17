using System.Collections.Generic;

namespace OstLib.Repository
{
    public interface IClient
    {
        void Add(Client client);
        Client GetById(int id);
        Client GetByTimeTableFields(TimeTableLineModel tableLineModel);
        IEnumerable<Client>? FindClientsBySurname(string surname);
        IEnumerable<Client> FindAll();
    }
}
