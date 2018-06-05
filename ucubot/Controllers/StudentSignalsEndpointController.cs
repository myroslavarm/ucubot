using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using ucubot.StudentCode;
using ucubot.Model;

namespace ucubot.Controllers
{
    [Route("api/[controller]")]
    public class StudentSignalsEndpointController : Controller{

        private readonly IStudentSignal _repository;

        public StudentSignalsEndpointController(IStudentSignal repository)
        {
            _repository = repository;
        }

        [HttpGet]
        IEnumerable<StudentSignal> getStudentSignals()
        {
            return _repository.getStudentSignals();
        }
    }
}
