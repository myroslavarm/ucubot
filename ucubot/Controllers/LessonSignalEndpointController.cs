using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using ucubot.Model;
using Dapper;
using ucubot.DBCode;

namespace ucubot.Controllers
{
    [Route("api/[controller]")]
    public class LessonSignalEndpointController : Controller
    {
        private readonly ILessonRepository _repository;

        public LessonSignalEndpointController(ILessonRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<LessonSignalDto> ShowSignals()
        {
            return _repository.getLessons();
        }

        [HttpGet("{id}")]
        public LessonSignalDto ShowSignal(long id)
        {
            return _repository.getLesson(id);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSignal(SlackMessage message)
        {
            if (_repository.insertLesson(message) == 404)
            {
                return BadRequest();
            }
            return Accepted();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveSignal(long id)
        {
            if (_repository.deleteLesson(id) == 200)
            {
                return Accepted();
            }
            return BadRequest();
        }
    }
}
