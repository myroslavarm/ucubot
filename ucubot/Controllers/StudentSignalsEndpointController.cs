using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using ucubot.StudentCode;

namespace ucubot.Controllers
{
    [Route("api/[controller]")]
    public class StudentSignalsEndpointController : Controller{
    
        private readonly IConfiguration _configuration;
        private readonly IStudentSignal _repository;
    
        public StudentSignalsEndpointController(IConfiguration configuration, IStudentSignal repository)
        {
            _configuration = configuration;
            _repository = repository;
        }
        
        [HttpGet]
        IEnumerable<StudentSignal> getStudentSignals(string conStr)
        {
            var connectionString = _configuration.GetConnectionString("BotDatabase");
            
            return _repository.getStudentSignals(connectionString);
        }

    }
}