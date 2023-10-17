using Microsoft.AspNetCore.Mvc;
using OstLib;
using OstLib.Repository;

namespace HttpServer.Controllers
{
    [Route("Client")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClient _client;
        public ClientController(IClient client)
        {
            _client = client;
        }

        [HttpPost("getBySurname")]
        public IEnumerable<Client> GetClientsBySurname(SurnameModel surnameModel)
        {
            if (surnameModel.Surname == null || surnameModel.Surname == "")
                return _client.FindAll();
            var list = _client.FindClientsBySurname(surnameModel.Surname);
            return list;
        }
    }
}
