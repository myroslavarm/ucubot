﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Ninject.Infrastructure.Language;
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
                               "lesson_signal.signal_type as Type, student.user_id as UserId " +
                               "FROM lesson_signal LEFT JOIN student ON lesson_signal.student_id = student.id";

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
                               "lesson_signal.signal_type as Type, student.user_id as UserId " +
                               "FROM lesson_signal LEFT JOIN student ON lesson_signal.student_id = student.id WHERE lesson_signal.id=@Id";

                var lsnSign = connection.Query<LessonSignalDto>(query, new {Id = id}).SingleOrDefault();
                return lsnSign;
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
                var query = @"SELECT * FROM student WHERE user_id=@id";
                var cnt = connection.Query<Student>(query, new {id = userId}).SingleOrThrowException(() => { return null; });

                if (cnt == null) { return BadRequest(); }
                var command = new MySqlCommand("INSERT INTO lesson_signal (student_id, signal_type) VALUES (@userId, @signalType)",
                    connection);
                command.Parameters.AddRange(new[]
                {
                    new MySqlParameter("userId", cnt.Id),
                    new MySqlParameter("signalType", signalType)
                });
                command.ExecuteNonQuery();
                return Accepted();
            }
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

                var command = new MySqlCommand("DELETE FROM lesson_signal WHERE id = @id",
                    connection);
                command.Parameters.Add(new MySqlParameter("id", id));
                command.ExecuteNonQuery();
                return Accepted();
            }
          }
    }
}
