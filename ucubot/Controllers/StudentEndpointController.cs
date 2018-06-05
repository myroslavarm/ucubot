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
        private readonly IStudentRepository _repository;
        public StudentEndpointController(IStudentRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<Student> ShowStudents()
        {
            return _repository.getStudents();
        }

        [HttpGet("{id}")]
        public Student ShowStudent(long id)
        {
            return _repository.getStudent(id);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(Student student)
        {
            if(_repository.insertStudent(student) == 200)
            {
                return Accepted();
            }
            return StatusCode(409);
        }


        [HttpPut]
        public async Task<IActionResult> UpdateStudent(Student student)
        {
            if (_repository.updStudent(student) == 200)
            {
                return Accepted();
            }
            return StatusCode(409);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveStudent(long id)
        {
            if (_repository.deleteStudent(id) == 200)
            {
                return Accepted();
            }
            return StatusCode(409);
        }
    }
}
