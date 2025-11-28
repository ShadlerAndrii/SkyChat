using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Skype.Data;
using Skype.Repositories;

namespace Skype.Controllers
{
    [Authorize]
    [ApiController]
    public class SettingDataController : ControllerBase
    {
        RepositorySettings _repository;

        public SettingDataController(RepositorySettings repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("[controller]")]
        public async Task<List<Setting>> GetData()
        {
            return await _repository.GetSettingData();
        }

        [HttpGet]
        [Route("[controller]/{userId}")]
        public async Task<List<Setting>> GetUserData(int userId)
        {
            var userSetting = await _repository.GetUserSettingData(userId);

            return userSetting;
        }

        [HttpPost]
        [Route("[controller]")]
        public async Task<IActionResult> AddData([FromForm] int userId)
        {
            await _repository.AddSettingData(userId);

            return Ok();
        }

        [HttpPut]
        [Route("[controller]")]
        public async Task<OkResult> ChangeData( [FromForm] int id,
                                                [FromForm] string bgColour,
                                                [FromForm] string fontSize)
        {
            await _repository.ChangeSettingData(id, bgColour, fontSize);

            return Ok();
        }
    }
}
