using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skype.Data;
using Skype.Repositories;
using static Skype.DTOs.ChatsDTO;

namespace Skype.Controllers
{
    [ApiController]
    [Authorize]
    public class ChatDataController : ControllerBase
    {
        RepositoryChats _repository;

        public ChatDataController(RepositoryChats repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("[controller]/{userId}")]
        public async Task<List<ChatWithItsUsernamesDTO>> GetData(int userId)
        {
            return await _repository.GetChatData(userId);
        }

        [HttpPost]
        [Route("[controller]")]
        public async Task<OkResult> AddData([FromForm] string name,
                                            [FromForm] string description,
                                            [FromForm] List<string> usernames)
        {
            await _repository.AddChatData(name, description, usernames);

            return Ok();
        }

        [HttpPut]
        [Route("[controller]")]
        public async Task<OkResult> ChangeData( [FromForm] int id,
                                                [FromForm] string name,
                                                [FromForm] string description,
                                                [FromForm] List<string> usernames)
        {
            await _repository.ChangeChatData(id, name, description, usernames);

            return Ok();
        }

        [HttpDelete]
        [Route("[controller]/{id}")]
        public async Task<OkResult> DeleteData(int id)
        {
            await _repository.RemoveChatData(id);

            return Ok();
        }
    }
}