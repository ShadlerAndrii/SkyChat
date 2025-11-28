using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skype.Data;
using Skype.Repositories;
using static Skype.DTOs.MessagesDTO;

namespace Skype.Controllers
{
    [ApiController]
    [Authorize]
    public class MessageDataController : ControllerBase
    {
        RepositoryMessages _repository;

        public MessageDataController(RepositoryMessages repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("[controller]/{chatId}")]
        public async Task<List<MessageWithItsUserNameDTO>> GetData(int chatId)
        {
            return await _repository.GetMessageData(chatId);
        }

        [HttpPost]
        [Route("[controller]")]
        public async Task<IActionResult> AddData(   [FromForm] int chatId,
                                                    [FromForm] int ownerId,
                                                    [FromForm] string text)
        {
            await _repository.AddMessageData(chatId, ownerId, text);

            return Ok();
        }
    }
}
