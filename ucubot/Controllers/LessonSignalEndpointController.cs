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
    public class LessonSignalEndpointController : Controller
    {
        private readonly IConfiguration _configuration;

        public LessonSignalEndpointController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IEnumerable<LessonSignalDto> ShowSignals()
        {
            var connectionString = _configuration.GetConnectionString("BotDatabase");
            
            using (var connection = new MySqlConnection(connectionString)){
                try{
                    connection.Open();
                }
                catch(Exception e){
                    Console.WriteLine(e.ToString());
                }
                
                string query = "SELECT lesson_signal.id as Id, lesson_signal.time_stamp as Timestamp, " +
                               "lesson_signal.signal_type as Type, Student.user_id as UserId " +
                               "FROM lesson_signal LEFT JOIN Student ON lesson_signal.student_id = Student.id;";

                var lsnSign = connection.Query<LessonSignalDto>(query).ToList();
                return lsnSign;
            }
        }
        
        [HttpGet("{id}")]
        public LessonSignalDto ShowSignal(long id)
        {
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("BotDatabase")))
            {
                try{
                    connection.Open();
                }
                catch (Exception e){
                    Console.WriteLine(e.ToString());
                }
                
                string query = "SELECT lesson_signal.id as Id, lesson_signal.time_stamp as Timestamp, " +
                               "lesson_signal.signal_type as Type, Student.user_id as UserId " +
                               "FROM lesson_signal LEFT JOIN Student ON lesson_signal.student_id = Student.id where Id = @id";
                
                var newCommand = new MySqlCommand(query).Parameters.AddWithValue("id", id);
                var lsnSign = connection.Query<LessonSignalDto>(newCommand.ToString()).ToList();
                return lsnSign.First();
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateSignal(SlackMessage message)
        {
            var userId = message.user_id;
            var signalType = message.text.ConvertSlackMessageToSignalType();

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
                    "INSERT INTO lesson_signal (student_id, signal_type) VALUES (@studentId, @signalType);";
                command.Parameters.AddRange(new[]
                {
                    new MySqlParameter("studentId", userId),
                    new MySqlParameter("signalType", signalType)
                });
                command.ExecuteNonQuery();
            }
            return Accepted();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveSignal(long id)
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
                    "DELETE FROM lesson_signal WHERE ID = @id;";
                command.Parameters.Add(new MySqlParameter("id", id));
                command.ExecuteNonQuery();
            }
            return Accepted();
        }
    }
}

