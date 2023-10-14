using Microsoft.AspNetCore.Mvc;
using OstLib;
using OstLib.Repository;

namespace HttpServer.Controllers
{
    [Route("Client")]
    [Controller]
    public class ClientController : ControllerBase
    {
        private readonly IClient _client;
        public ClientController(IClient client)
        {
            _client = client;
        }

        [HttpPost("GetBySurname")]
        public IEnumerable<Client> GetClientsBySurname(SurnameModel surnameModel)
        {
            /*
             * TODO: surnameModel возвращает null при не null-овом значении. Нужно исправить
             */
            if (surnameModel.S == null || surnameModel.S == "")
                return _client.FindAll();
            var list = _client.FindClientsBySurname(surnameModel.S);
            return list;
        }
    }
}
