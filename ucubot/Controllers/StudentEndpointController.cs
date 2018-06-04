using System;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using ucubot.Model;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using ucubot.DBCode;

namespace ucubot.Controllers
{
    [Route("api/[controller]")]
    public class StudentEndpointController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IStudentRepository _repository;
        public StudentEndpointController(IConfiguration configuration, IStudentRepository repository)
        {
            _configuration = configuration;
            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<Student> ShowStudents()
        {
            var connectionString = _configuration.GetConnectionString("BotDatabase");
            return _repository.getStudents(connectionString);
        }

        [HttpGet("{id}")]
        public Student ShowStudent(long id)
        {
            var connectionString = _configuration.GetConnectionString("BotDatabase");
            return _repository.getStudent(connectionString, id);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(Student student)
        {
            var connectionString = _configuration.GetConnectionString("BotDatabase");
            if(_repository.insertStudent(connectionString, student) == 200)
            {
                return Accepted();
            }
            return StatusCode(409);
        }


        [HttpPut]
        public async Task<IActionResult> UpdateStudent(Student student)
        {
            var connectionString = _configuration.GetConnectionString("BotDatabase");
            if (_repository.updStudent(connectionString, student) == 200)
            {
                return Accepted();
            }
            return StatusCode(409);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveStudent(long id)
        {
            var connectionString = _configuration.GetConnectionString("BotDatabase");
            if (_repository.deleteStudent(connectionString, id) == 200)
            {
                return Accepted();
            }
            return StatusCode(409);
        }
    }
}
