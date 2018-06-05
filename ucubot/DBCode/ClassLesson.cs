using System;
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
using ucubot.Controllers;

namespace ucubot.DBCode
{
    public class ClassLesson : ILessonRepository
    {
        private readonly IConfiguration _configuration;

        public ClassLesson(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IEnumerable<LessonSignalDto> getLessons()
        {
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("BotDatabase")))
            {
                try
                {
                    connection.Open();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                string query = "SELECT lesson_signal.id as Id, lesson_signal.time_stamp as Timestamp, " +
                               "lesson_signal.signal_type as Type, student.user_id as UserId " +
                               "FROM lesson_signal LEFT JOIN student ON lesson_signal.student_id = student.id;";

                var lsnSign = connection.Query<LessonSignalDto>(query).ToList();
                return lsnSign;
          }
        }

        public LessonSignalDto getLesson(long id)
        {
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("BotDatabase")))
            {
                try
                {
                    connection.Open();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                string query = "SELECT lesson_signal.id as Id, lesson_signal.time_stamp as Timestamp, " +
                               "lesson_signal.signal_type as Type, student.user_id as UserId " +
                               "FROM lesson_signal LEFT JOIN student ON lesson_signal.student_id = student.id where lesson_signal.id = @id";

                var lsnSign = connection.Query<LessonSignalDto>(query, new {Id = id}).SingleOrDefault();
                return lsnSign;
          }
        }

        public int insertLesson(SlackMessage message)
        {
            var userId = message.user_id;
            var signalType = message.text.ConvertSlackMessageToSignalType();
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("BotDatabase")))
            {
                try
                {
                    connection.Open();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                var query = @"SELECT * FROM student WHERE user_id=@id";
                var cnt = connection.Query<Student>(query, new {id = userId}).SingleOrThrowException(() => { return null; });

                if (cnt == null) { return 404; }
                var command = new MySqlCommand("INSERT INTO lesson_signal (student_id, signal_type) VALUES (@userId, @signalType)",
                    connection);
                command.Parameters.AddRange(new[]
                {
                    new MySqlParameter("userId", cnt.Id),
                    new MySqlParameter("signalType", signalType)
                });
                command.ExecuteNonQuery();
                return 200;
          }
        }

        public int deleteLesson(long id)
        {
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("BotDatabase")))
            {
                try
                {
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
                return 200;
          }
        }
    }
}
