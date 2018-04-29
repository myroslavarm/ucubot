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

namespace ucubot.Controllers
{
    [Route("api/[controller]")]
    public class StudentEndpointController : Controller
    {
        private readonly IConfiguration _configuration;

        public StudentEndpointController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IEnumerable<Student> ShowStudents()
        {
            var connectionString = _configuration.GetConnectionString("BotDatabase");
            string query = "SELECT Student.id as Id, Student.first_name as FirstName, " +
                           "Student.last_name as LastName, Student.user_id as UserId FROM Student;";
            
            using (var connection = new MySqlConnection(connectionString)){
                try{
                    connection.Open();
                }
                catch(Exception e){
                    Console.WriteLine(e.ToString());
                }

                var stdnt = connection.Query<Student>(query).ToList();
                return stdnt;
            }
        }
        
        [HttpGet("{id}")]
        public Student ShowStudent(long id)
        {
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("BotDatabase")))
            {
                try{
                    connection.Open();
                }
                catch (Exception e){
                    Console.WriteLine(e.ToString());
                }
                
                string query = "SELECT Student.id as Id, Student.first_name as FirstName, " +
				"Student.last_name as LastName, Student.user_id as UserId FROM Student WHERE Id = @id";
                
                var newCommand = new MySqlCommand(query).Parameters.AddWithValue("id", id);
                var stdnt = connection.Query<Student>(newCommand.ToString()).ToList();
                return stdnt.First();
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateStudent(Student student)
        {
            var userId = Student.UserId;
	    var firstName = Student.FirstName;
	    var lastName = Student.LastName;

            using (var connection = new MySqlConnection(_configuration.GetConnectionString("BotDatabase")))
            {
                try{
                    connection.Open();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                
                var command = connection.CreateCommand();
                var cnt = connection.CreateCommand();
                cnt.CommandText = "COUNT(*) FROM student WHERE user_id=" + userId + ";";
                if(cnt.ExecuteNonQuery()==0){
                    return BadRequest();
                }
                command.CommandText =
                    "INSERT INTO student (id, first_name, last_name) VALUES (@Id, @FirstName, @LastName);";
                command.Parameters.AddRange(new[]
                {
                    new MySqlParameter("userId", userId),
                    new MySqlParameter("firstName", firstName),
		    new MySqlParameter("lastName", lastName)
                });
                command.ExecuteNonQuery();
            }
            return Accepted();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveStudent(long id)
        {
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("BotDatabase")))
            {
                try{
                    connection.Open();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                
                var command = connection.CreateCommand();
                command.CommandText =
                    "DELETE FROM Student WHERE ID = @id;";
                command.Parameters.Add(new MySqlParameter("id", id));
                command.ExecuteNonQuery();
            }
            return Accepted();
        }
    }
}
