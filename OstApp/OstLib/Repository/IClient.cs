namespace OstLib.Repository
{
    public interface IClient
    {
        void Add(Client client);
        Client GetById(int id);
    }
}
